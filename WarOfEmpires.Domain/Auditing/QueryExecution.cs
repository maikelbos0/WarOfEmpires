using System;

namespace WarOfEmpires.Domain.Auditing {
    public class QueryExecution : AggregateRoot {
        public DateTime Date { get; protected set; }
        public string QueryType { get; protected set; }
        public string QueryData { get; protected set; }

        public QueryExecution(string queryType, string queryData) {
            Date = DateTime.UtcNow;
            QueryType = queryType;
            QueryData = queryData;
        }
    }
}