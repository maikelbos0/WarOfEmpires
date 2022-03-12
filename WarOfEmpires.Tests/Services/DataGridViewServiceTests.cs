using WarOfEmpires.Models;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Tests.Services {
    [TestClass]
    public sealed class DataGridViewServiceTests {
        public class TestViewModel : EntityViewModel {
            public string Name { get; set; }
        }

        [TestMethod]
        public void DataGridViewService_Pages_Correctly_First_Page() {
            var service = new DataGridViewService();
            var metaData = new DataGridViewMetaData() {
                rowsPerPage = 25
            };

            var result = service.ApplyMetaData(GenerateList(), ref metaData).ToList();

            result.Should().HaveCount(25);
        }

        [TestMethod]
        public void DataGridViewService_Pages_Correctly_Last_Page() {
            var service = new DataGridViewService();
            var metaData = new DataGridViewMetaData() {
                page = 5,
                rowsPerPage = 25
            };

            var result = service.ApplyMetaData(GenerateList(), ref metaData).ToList();

            result.Should().HaveCount(12);
        }

        [DataTestMethod]
        [DataRow("name", DisplayName = "Camel case")]
        [DataRow("Name", DisplayName = "Pascal case")]
        public void DataGridViewService_Sorts_Correctly(string column) {
            var service = new DataGridViewService();
            var metaData = new DataGridViewMetaData() {
                sortColumn = column,
                rowsPerPage = 25
            };

            var result = service.ApplyMetaData(GenerateList(), ref metaData).ToList();

            result.Select(i => i.Name).Should().BeInAscendingOrder();
        }

        [DataTestMethod]
        [DataRow("name", DisplayName = "Camel case")]
        [DataRow("Name", DisplayName = "Pascal case")]
        public void DataGridViewService_Sorts_Descending_Correctly(string column) {
            var service = new DataGridViewService();
            var metaData = new DataGridViewMetaData() {
                sortColumn = column,
                sortDescending = true,
                rowsPerPage = 25
            };

            var result = service.ApplyMetaData(GenerateList(), ref metaData).ToList();

            result.Select(i => i.Name).Should().BeInDescendingOrder();
        }

        [TestMethod]
        public void DataGridViewService_Sorts_By_Id_For_Invalid_Property() {
            var service = new DataGridViewService();
            var metaData = new DataGridViewMetaData() {
                sortColumn = "Wrong",
                rowsPerPage = 25
            };

            var result = service.ApplyMetaData(GenerateList(), ref metaData).ToList();

            result.Select(i => i.Id).Should().BeInAscendingOrder();
        }

        [TestMethod]
        public void DataGridViewService_Sorts_By_Id_For_Missing_SortOrder() {
            var service = new DataGridViewService();
            var metaData = new DataGridViewMetaData() {
                rowsPerPage = 25
            };

            var result = service.ApplyMetaData(GenerateList(), ref metaData).ToList();

            result.Select(i => i.Id).Should().BeInAscendingOrder();
        }

        [TestMethod]
        public void DataGridViewService_Updates_MetaData() {
            var service = new DataGridViewService();
            var metaData = new DataGridViewMetaData() {
                rowsPerPage = 25
            };

            service.ApplyMetaData(GenerateList(), ref metaData);

            metaData.totalRows.Should().Be(137);
        }

        private static IEnumerable<TestViewModel> GenerateList() {
            return Enumerable.Range(1, 137).Select(i => new TestViewModel() {
                Id = i,
                Name = $"Name {i}"
            });
        }
    }
}