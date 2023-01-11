using InvestBetterPlan_RestAPI.Models.Dto;

namespace InvestBetterPlan_RestAPI.Repository.IRepository
{
    public interface ISummaryRepository
    {
        Task<List<SummaryDTO>> GetSummary(int userId);
    }
}
