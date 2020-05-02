using System.Collections.Generic;

namespace WarOfEmpires.Models.Grids {
    public sealed class GridViewModel {
        public string Id { get; set; }
        public string DataUrl { get; set; }
        public string Renderer { get; set; }
        public string SearchFormId { get; set; }
        public List<GridColumnViewModel> Columns { get; set; }
        public string SortColumn { get; set; }
        public bool SortDescending { get; set; }
    }
}