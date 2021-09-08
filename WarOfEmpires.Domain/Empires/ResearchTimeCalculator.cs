using System;

namespace WarOfEmpires.Domain.Empires {
    public static class ResearchTimeCalculator {
        private const long ResearchTimePerCompletedResearch = 5000;
        private const long BaseResearchTime = 5000;

        public static long GetResearchTime(int completedResearchCount, int completedResearchLevelForType) {
            return ResearchTimePerCompletedResearch * completedResearchCount
                + BaseResearchTime * (long)(Math.Pow(3, completedResearchLevelForType) + 0.5);
        }
    }
}
