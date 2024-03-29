﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Attributes
{
    public class HistoricizedAttribute : Attribute
    {
        public HistoricizedAttribute(Type historicalType)
        {
            HistoricalType = historicalType;
        }

        public Type HistoricalType { get; }
    }
}
