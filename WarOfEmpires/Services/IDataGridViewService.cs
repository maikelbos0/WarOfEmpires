using System.Collections.Generic;
using WarOfEmpires.Models;
using WarOfEmpires.Models.Grids;

namespace WarOfEmpires.Services {
    public interface IDataGridViewService {
        IEnumerable<TViewModel> ApplyMetaData<TViewModel>(IEnumerable<TViewModel> query, ref DataGridViewMetaData metaData) where TViewModel : EntityViewModel;
    }
}