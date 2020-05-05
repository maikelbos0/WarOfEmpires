using System;

namespace WarOfEmpires.Models.Grids {
    public sealed class GridColumnAttribute : Attribute {
        public int Index { get; }
        public int Width { get; }
        public string Header { get; }
        public string SortData { get; set; }

        public GridColumnAttribute(int index, int width, string header) {
            Index = index;
            Width = width;
            Header = header;
        }
    }
}