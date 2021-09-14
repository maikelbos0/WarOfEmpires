using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public sealed class GetResearchQuery : IQuery<ResearchViewModel> {
        public string Email { get; }
        public string ResearchType { get; }

        public GetResearchQuery(string email, string researchType) {
            Email = email;
            ResearchType = researchType;
        }
    }
}
