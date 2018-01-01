using MoneyChest.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class DebtPenaltyModel : IHasId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DebtPenaltyModel()
        {
            Date = DateTime.Now;
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }  // value in currency of debt

        public int DebtId { get; set; }
    }
}
