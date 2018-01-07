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

namespace MoneyChest.Services.Services
{
    public interface IDebtService : IIdManagableUserableListServiceBase<DebtModel>
    {
        List<DebtModel> GetActive(int userId);
    }

    public class DebtService : HistoricizedIdManageableUserableListServiceBase<Debt, DebtModel, DebtConverter>, IDebtService
    {
        public DebtService(ApplicationDbContext context) : base(context)
        { }

        #region IDebtService implementation

        public List<DebtModel> GetActive(int userId)
        {
            return Scope.Where(e => e.UserId == userId && !e.IsRepaid).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        #region Overrides

        protected override IQueryable<Debt> Scope => Entities.Include(_ => _.Currency).Include(_ => _.Category).Include(_ => _.Storage).Include(_ => _.DebtPenalties);

        public override DebtModel Add(DebtModel model)
        {
            // convert to Db entity
            var entity = _converter.ToEntity(model);
            // add to database
            entity = Add(entity);
            // save changes
            SaveChanges();
            // TODO: add OnAdd method. Check UserService

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
                
            // save changes
            SaveChanges();

            return _converter.UpdateModel(GetDbDetailedEntity(entity), model);
        }

        public override DebtModel Update(DebtModel model)
        {
            // get from database
            var dbEntity = GetDbEntity(model);
            // update entity by converter
            dbEntity = _converter.UpdateEntity(dbEntity, model);
            // update entity in database
            dbEntity = Update(dbEntity);

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
            // TODO: add OnUpdate method

            // TODO: check if related entity foreign key was changed related entity will be updated automatically or not. For now implementation like "not"
            return _converter.UpdateModel(GetDbDetailedEntity(dbEntity), model);
        }

        #endregion
    }
}
