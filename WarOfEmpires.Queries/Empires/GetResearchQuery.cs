using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public sealed class GetResearchQuery : IQuery<ResearchModel> {
        public string Email { get; }

        public GetResearchQuery(string email) {
            Email = email;
        }
    }
}
