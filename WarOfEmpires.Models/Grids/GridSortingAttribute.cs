﻿using System;

namespace WarOfEmpires.Models.Grids {
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class GridSortingAttribute : Attribute {
        public string Column { get; }
        public bool Descending { get; }

        public GridSortingAttribute(string column, bool descending) {
            Column = column;
            Descending = descending;
        }
    }
}