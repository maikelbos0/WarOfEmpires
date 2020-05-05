namespace WarOfEmpires.Models.Grids {
    public class DataGridViewMetaData {
#pragma warning disable IDE1006 // Naming Styles
        public string sortColumn { get; set; }
        public bool sortDescending { get; set; }
        public int totalRows { get; set; }
        public int page { get; set; }
        public int rowsPerPage { get; set; } = 25;
#pragma warning restore IDE1006 // Naming Styles
    }
}