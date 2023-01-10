using InvestBetterPlan_RestAPI.Models;
using InvestBetterPlan_RestAPI.Models.Dto;
using InvestBetterPlan_RestAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Linq.Expressions;

namespace InvestBetterPlan_RestAPI.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly challengeContext _db;

      

        public UserRepository(challengeContext db)
        {
            _db = db;
        }
        public async Task<User> GetUser(Expression<Func<User, bool>> filter = null)
        {
            IQueryable<User> query = _db.Users;
            if (filter != null)
            {
                query = query.Where(filter);

            }

            return await query.FirstOrDefaultAsync();
        }

        public string GetAdvisorFullNameById(int? idAdvisor)
        {
            string result = string.Empty;

            try
            {
                if (idAdvisor.HasValue == false)
                    return "Sin advisor";

                var user = _db.Users.Where(u => u.Id == idAdvisor.Value).FirstOrDefault();

                result = user == null ? "Sin advisor" : user.ToString();
            }
            catch (Exception ex)
            {

            }

            return result;

        }
    }
}
