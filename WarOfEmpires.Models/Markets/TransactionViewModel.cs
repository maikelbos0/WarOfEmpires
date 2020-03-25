using System;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Markets {
    public sealed class TransactionViewModel {
        public DateTime Date { get; set; }
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public string QuantityString { get { return Quantity.ToString(StringFormat.Integer); } }
        public int Price { get; set; }
        public string PriceString { get { return Price.ToString(StringFormat.Integer); } }
    }
}