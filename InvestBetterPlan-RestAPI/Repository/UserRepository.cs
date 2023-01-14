using InvestBetterPlan_RestAPI.Models;
using InvestBetterPlan_RestAPI.Models.Constants;
using InvestBetterPlan_RestAPI.Models.Dto;
using InvestBetterPlan_RestAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

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
        public async Task<UserDTO> GetUser(int id)
        {
            try
            {
                var user = await (
                                    from u in _db.Users
                                    where u.Id == id
                                    select new UserDTO
                                    {
                                        NombreCompleto = u.ToString(),
                                        NombreCompletoAdvisor = u.Advisorid.HasValue? u.Advisor.ToString(): "Sin Advisor",
                                        FechaCreacion = new DateTime(u.Created.Year, u.Created.Month, u.Created.Day)
                                    }
                                    ).ToListAsync();

                if (user == null || user.Count <= 0)
                    return null;

                return user[0];
            }
            catch (Exception ex)
            {
                return null;
            }
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
                return String.Empty;
            }

            return result;

        }
    }
}
