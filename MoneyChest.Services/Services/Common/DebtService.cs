﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Model.Model;
using MoneyChest.Model.Extensions;
using System.Data.Entity;
using MoneyChest.Services.Converters;
using MoneyChest.Data.Enums;

namespace MoneyChest.Services.Services
{
    public interface IDebtService : IIdManagableUserableListServiceBase<DebtModel>
    {
        List<DebtModel> GetActive(int userId, params int?[] requiredIds);
    }

    public class DebtService : HistoricizedIdManageableUserableListServiceBase<Debt, DebtModel, DebtConverter>, IDebtService
    {
        public DebtService(ApplicationDbContext context) : base(context)
        { }

        #region IDebtService implementation

        public List<DebtModel> GetActive(int userId, params int?[] requiredIds)
        {
            var ids = requiredIds?.Where(e => e.HasValue)?.Select(e => (int)e).ToList() ?? new List<int>();
            return Scope.Where(e => e.UserId == userId && (!e.IsRepaid || ids.Contains(e.Id))).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        #region Overrides

        protected override IQueryable<Debt> Scope => Entities.Include(_ => _.Currency).Include(_ => _.Category).Include(_ => _.Storage).Include(_ => _.DebtPenalties);

        public override void OnAdded(DebtModel model, Debt entity)
        {
            base.OnAdded(model, entity);

            // add new penalties
            foreach (var newPenalty in model.Penalties.Where(e => !entity.DebtPenalties.Any(p => p.Id == e.Id)))
            {
                var debtPenalty = new DebtPenalty()
                {
                    Date = newPenalty.Date,
                    Description = newPenalty.Description,
                    Value = newPenalty.Value,
                    DebtId = entity.Id
                };

                _context.DebtPenalties.Add(debtPenalty);
                _historyService.WriteHistory(debtPenalty, Data.Enums.ActionType.Add, entity.UserId);
            }
            
            // update related storage
            if(model.StorageId.HasValue)
                AddValueToStorage(model.StorageId.Value, (model.DebtType == Model.Enums.DebtType.TakeBorrow ? 1 : -1) * (model.Value - model.InitialFee));
                
            // save changes
            SaveChanges();
        }

        public override void OnUpdated(DebtModel oldModel, DebtModel model)
        {
            base.OnUpdated(oldModel, model);

            // get from database
            var dbEntity = Entities.Include(_ => _.DebtPenalties).FirstOrDefault(_ => _.Id == model.Id);

            // update existing penalties
            foreach (var existingPenalty in dbEntity.DebtPenalties.ToList())
            {
                var penaltyModel = model.Penalties.FirstOrDefault(_ => _.Id == existingPenalty.Id);
                if (penaltyModel != null)
                {
                    existingPenalty.Date = penaltyModel.Date;
                    existingPenalty.Description = penaltyModel.Description;
                    existingPenalty.Value = penaltyModel.Value;
                    _historyService.WriteHistory(existingPenalty, Data.Enums.ActionType.Update, dbEntity.UserId);
                }
                else
                {
                    _historyService.WriteHistory(existingPenalty, Data.Enums.ActionType.Delete, dbEntity.UserId);
                    _context.DebtPenalties.Remove(existingPenalty);
                }
            }

            // add new penalties
            foreach (var newPenalty in model.Penalties.Where(e => !dbEntity.DebtPenalties.Any(p => p.Id == e.Id)))
            {
                var debtPenalty = new DebtPenalty()
                {
                    Date = newPenalty.Date,
                    Description = newPenalty.Description,
                    Value = newPenalty.Value,
                    DebtId = dbEntity.Id
                };

                _context.DebtPenalties.Add(debtPenalty);
                _historyService.WriteHistory(debtPenalty, Data.Enums.ActionType.Add, dbEntity.UserId);
            }
            
            // update related storages
            var oldValue = (oldModel.DebtType == Model.Enums.DebtType.TakeBorrow ? 1 : -1) * (oldModel.Value - oldModel.InitialFee);
            var newValue = (model.DebtType == Model.Enums.DebtType.TakeBorrow ? 1 : -1) * (model.Value - model.InitialFee);

            if (oldModel.StorageId != model.StorageId)
            {
                if(oldModel.StorageId.HasValue && oldModel.StorageId.Value > 0)
                    AddValueToStorage(oldModel.StorageId.Value, -oldValue);

                if (model.StorageId.HasValue && model.StorageId.Value > 0)
                    AddValueToStorage(model.StorageId.Value, newValue);
            }
            else if (model.StorageId.HasValue && model.StorageId.Value > 0 && oldValue != newValue)
                AddValueToStorage(model.StorageId.Value, newValue - oldValue);

            // save changes
            SaveChanges();
        }

        public override void OnDeleted(DebtModel model)
        {
            base.OnDeleted(model);

            // only remove repaid debts
            if (model.IsRepaid || model.ValueToBePaid <= 0) return;

            // update related storage
            if (model.StorageId.HasValue)
                AddValueToStorage(model.StorageId.Value, (model.DebtType == Model.Enums.DebtType.GiveBorrow ? 1 : -1) * (model.Value - model.InitialFee));

            // save changes
            SaveChanges();
        }

        #endregion

        #region Private methods

        private void AddValueToStorage(int storageId, decimal value)
        {
            var storage = _context.Storages.FirstOrDefault(_ => _.Id == storageId);
            storage.Value += value;
            _historyService.WriteHistory(storage, ActionType.Update, storage.UserId);
        }

        #endregion
    }
}
