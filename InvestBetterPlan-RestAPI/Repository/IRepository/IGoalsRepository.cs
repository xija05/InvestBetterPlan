using InvestBetterPlan_RestAPI.Models.Dto;

namespace InvestBetterPlan_RestAPI.Repository.IRepository
{
    public interface IGoalsRepository
    {
        Task<List<GoalsDTO>> GetGoals(int userId);
    }
}
