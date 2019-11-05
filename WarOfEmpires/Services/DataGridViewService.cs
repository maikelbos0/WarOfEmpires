using WarOfEmpires.Models;
using WarOfEmpires.Utilities.Container;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Services {
    [InterfaceInjectable]
    public sealed class DataGridViewService : IDataGridViewService {
        private IEnumerable<TViewModel> Sort<TViewModel>(IEnumerable<TViewModel> query, DataGridViewMetaData metaData) where TViewModel : ViewModel {
            Func<TViewModel, object> orderBy = null;

            if (metaData.sortColumn != null) {
                var property = typeof(TViewModel).GetProperty(metaData.sortColumn);

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

        private IEnumerable<TViewModel> Page<TViewModel>(IEnumerable<TViewModel> query, DataGridViewMetaData metaData) where TViewModel : ViewModel {
            return query.Skip(metaData.page * metaData.rowsPerPage).Take(metaData.rowsPerPage);
        }

        private DataGridViewMetaData GetMetaData<TViewModel>(IEnumerable<TViewModel> query, DataGridViewMetaData metaData) where TViewModel : ViewModel {
            return new DataGridViewMetaData() {
                sortColumn = metaData.sortColumn,
                sortDescending = metaData.sortDescending,
                page = metaData.page,
                rowsPerPage = metaData.rowsPerPage,
                totalRows = query.Count()
            };
        }

        public IEnumerable<TViewModel> ApplyMetaData<TViewModel>(IEnumerable<TViewModel> query, ref DataGridViewMetaData metaData) where TViewModel : ViewModel {
            metaData = GetMetaData(query, metaData);

            return Page(Sort(query, metaData), metaData);
        }
    }
}