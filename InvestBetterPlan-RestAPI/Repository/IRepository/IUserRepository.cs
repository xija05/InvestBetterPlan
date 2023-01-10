using InvestBetterPlan_RestAPI.Models;
using System.Linq.Expressions;

namespace InvestBetterPlan_RestAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetUser(Expression<Func<User, bool>> filter = null);

        string GetAdvisorFullNameById(int? idAdvisor);
    }
}
