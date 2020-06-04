using WarOfEmpires.Models;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Container;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Services {
    [InterfaceInjectable]
    public sealed class DataGridViewService : IDataGridViewService {
        private IEnumerable<TEntityViewModel> Sort<TEntityViewModel>(IEnumerable<TEntityViewModel> query, DataGridViewMetaData metaData) where TEntityViewModel : EntityViewModel {
            Func<TEntityViewModel, object> orderBy = null;

            if (!string.IsNullOrEmpty(metaData.sortColumn)) {
                var property = typeof(TEntityViewModel).GetProperty(metaData.sortColumn);

                if (property != null) {
                    orderBy = i => property.GetValue(i);
                }
            }

            if (orderBy == null) {
                orderBy = i => i.Id;
            }

            if (metaData.sortDescending) {
                return query.OrderByDescending(orderBy);
            }
            else {
                return query.OrderBy(orderBy);
            }
        }

        private IEnumerable<TViewModel> Page<TViewModel>(IEnumerable<TViewModel> query, DataGridViewMetaData metaData) where TViewModel : EntityViewModel {
            return query.Skip(metaData.page * metaData.rowsPerPage).Take(metaData.rowsPerPage);
        }

        private DataGridViewMetaData GetMetaData<TViewModel>(IEnumerable<TViewModel> query, DataGridViewMetaData metaData) where TViewModel : EntityViewModel {
            return new DataGridViewMetaData() {
                sortColumn = metaData.sortColumn,
                sortDescending = metaData.sortDescending,
                page = metaData.page,
                rowsPerPage = metaData.rowsPerPage,
                totalRows = query.Count()
            };
        }

        public IEnumerable<TViewModel> ApplyMetaData<TViewModel>(IEnumerable<TViewModel> query, ref DataGridViewMetaData metaData) where TViewModel : EntityViewModel {
            metaData = GetMetaData(query, metaData);

            return Page(Sort(query, metaData), metaData);
        }
    }
}