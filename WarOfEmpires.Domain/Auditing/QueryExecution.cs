using System;

namespace WarOfEmpires.Domain.Auditing {
    public class QueryExecution : AggregateRoot {
        public DateTime Date { get; protected set; }
        public string QueryType { get; protected set; }
        public string QueryData { get; protected set; }
        public double ElapsedMilliseconds { get; protected set; }

        public QueryExecution(string queryType, string queryData, double elapsedMilliseconds) {
            Date = DateTime.UtcNow;
            QueryType = queryType;
            QueryData = queryData;
            ElapsedMilliseconds = elapsedMilliseconds;
        }
    }
}