using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Base
{
    public interface IHasId
    {
        int Id { get; set; }
    }

    public interface IHasName
    {
        string Name { get; set; }
    }

    public interface IHasDescription
    {
        string Description { get; set; }
    }

    public interface IHasRemark
    {
        string Remark { get; set; }
    }

    public interface IHasUserId
    {
        int UserId { get; set; }
    }

    public interface IHasCategory
    {
        int? CategoryId { get; set; }
    }

    public interface IHasExchangeRate
    {
        decimal CurrencyExchangeRate { get; set; }
        bool SwappedCurrenciesRate { get; set; }
    }

    public interface IHasPlannedExchangeRate : IHasExchangeRate
    {
        bool TakeExistingCurrencyExchangeRate { get; set; }
    }
}
