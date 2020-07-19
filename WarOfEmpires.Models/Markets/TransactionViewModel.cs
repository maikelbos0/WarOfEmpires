using System;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Markets {
    [GridSorting(nameof(Date), true)]
    public sealed class TransactionViewModel : EntityViewModel {
        public DateTime Date { get; set; }
        [GridColumn(0, 35, "Date", SortData = nameof(Date))]
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        [GridColumn(1, 20, "Type")]
        public string Type { get; set; }
        public int Quantity { get; set; }
        [GridColumn(2, 25, "Quantity", SortData = nameof(Quantity))]
        public string QuantityString { get { return Quantity.ToString(StringFormat.Integer); } }
        public int Price { get; set; }
        [GridColumn(3, 20, "Price", SortData = nameof(Price))]
        public string PriceString { get { return Price.ToString(StringFormat.Integer); } }
        public bool IsRead { get; set; }
    }
}