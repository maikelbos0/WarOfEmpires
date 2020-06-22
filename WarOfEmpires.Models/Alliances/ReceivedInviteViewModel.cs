﻿using System;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Alliances {
    [GridSorting(nameof(Date), true)]
    public sealed class ReceivedInviteViewModel : EntityViewModel {
        public DateTime Date { get; set; }
        [GridColumn(0, 15, "Date", SortData = nameof(Date))]
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        public int AllianceId { get; set; }
        [GridColumn(1, 10, "Alliance code")]
        public string AllianceCode { get; set; }
        [GridColumn(2, 25, "Alliance name")]
        public string AllianceName { get; set; }
        [GridColumn(3, 40, "Subject")]
        public string Subject { get; set; }
        // TODO add isread
    }
}