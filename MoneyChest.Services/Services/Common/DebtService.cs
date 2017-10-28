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
using MoneyChest.Model.Convert;
using System.Data.Entity;
using MoneyChest.Model.Converters;

namespace MoneyChest.Services.Services
{
    public interface IDebtService : IBaseIdManagableUserableListService<DebtModel>
    {
        List<DebtModel> GetActive(int userId);
    }

    public class DebtService : BaseHistoricizedIdManageableUserableListService<Debt, DebtModel, DebtConverter>, IDebtService
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

        protected override IQueryable<Debt> Scope => Entities.Include(_ => _.Currency);

        #endregion
    }
}
