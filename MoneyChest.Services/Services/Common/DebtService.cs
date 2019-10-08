using System;
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
using MoneyChest.Data.Extensions;
using MoneyChest.Model.Enums;
using MoneyChest.Services.Utils;

namespace MoneyChest.Services.Services
{
    public interface IDebtService : IIdManagableUserableListServiceBase<DebtModel>
    {
        List<DebtModel> GetActive(int userId, params int?[] requiredIds);
    }

    public class DebtService : HistoricizedIdManageableUserableListServiceBase<Debt, DebtModel, DebtConverter>, IDebtService
    {
        #region Private fields

        private IRecordService _recordService;

        #endregion

        #region Initialization

        public DebtService(ApplicationDbContext context) : base(context)
        {
            _recordService = new RecordService(context);
        }

        #endregion

        #region IDebtService implementation

        public List<DebtModel> GetActive(int userId, params int?[] requiredIds)
        {
            var ids = requiredIds?.Where(e => e.HasValue)?.Select(e => (int)e).ToList() ?? new List<int>();
            return Scope.Where(e => e.UserId == userId && (!e.IsRepaid || ids.Contains(e.Id))).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        #region Overrides

        public override DebtModel Add(DebtModel model)
        {
            // update description from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, model);

            return base.Add(model);
        }

        public override IEnumerable<DebtModel> Add(IEnumerable<DebtModel> models)
        {
            // update descriptions from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, models);

            return base.Add(models);
        }

        protected override IQueryable<Debt> Scope => Entities.Include(_ => _.Currency).Include(_ => _.Category).Include(_ => _.Storage).Include(_ => _.DebtPenalties);

        public override DebtModel PrepareNew(DebtModel model)
        {
            // base preparing
            base.PrepareNew(model);
            // set default currency and storage
            var mainCurrency = _context.Currencies.FirstOrDefault(x => x.IsMain && model.UserId == x.UserId);
            model.CurrencyId = mainCurrency?.Id ?? 0;
            model.Currency = mainCurrency?.ToReferenceView();
            
            // set default storage
            var storage = _context.Storages.FirstOrDefault(x => x.CurrencyId == model.CurrencyId && model.UserId == x.UserId);
            model.StorageId = storage?.Id ?? 0;
            model.Storage = storage?.ToReferenceView();

            return model;
        }

        public override void OnAdded(DebtModel model, Debt entity)
        {
            base.OnAdded(model, entity);

            // update Id, because it would be used for record
            model.Id = entity.Id;

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

            // create record for debt if storage is provided
            if (model.StorageId.HasValue && model.StorageId > 0)
                _recordService.CreateForDebt(model);

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

            // save changes
            SaveChanges();
        }

        #endregion
    }
}
