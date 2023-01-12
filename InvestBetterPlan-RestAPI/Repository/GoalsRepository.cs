using InvestBetterPlan_RestAPI.Models;
using InvestBetterPlan_RestAPI.Models.Dto;
using InvestBetterPlan_RestAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace InvestBetterPlan_RestAPI.Repository
{
    public class GoalsRepository : IGoalsRepository
    {
        public challengeContext _db { get; }
        public GoalsRepository(challengeContext db)
        {
            _db = db;
        }


        public async Task<List<GoalsDTO>> GetGoals(int userId)
        {
            List<GoalsDTO> goals = new List<GoalsDTO>();

            try
            {
                var goalsByUserID = await (
                                                from g in _db.Goals
                                                where g.Userid == userId
                                                select new
                                                {
                                                    TituloMeta = g.Title,
                                                    Anios = g.Years,
                                                    InversionInicial = g.Initialinvestment,
                                                    AporteMensual = g.Monthlycontribution,
                                                    MontoObjetivo = g.Targetamount,
                                                    EntidadFinanciera = g.Financialentity == null? "Sin Entidad Financiera" : g.Financialentity.Title,
                                                    FechaCreacion = g.Created,
                                                    Portafolio = new
                                                    {
                                                        NombrePortafolio = g.Portfolio.Title,
                                                        Descripcion = g.Portfolio.Description,
                                                        RangoMin = g.Portfolio.Minrangeyear,
                                                        RangoMax = g.Portfolio.Maxrangeyear,
                                                        ComisionBP = g.Portfolio.Bpcomission,
                                                        RentabilidadEstimada = g.Portfolio.Estimatedprofitability,
                                                        Rentabilidad = g.Portfolio.Profitability
                                                    }
                                                }).ToListAsync();

                if(goalsByUserID == null || goalsByUserID.Count <= 0)
                {
                    GoalsDTO objGoal = new GoalsDTO();
                    objGoal.TituloMeta = "Sin Titulo";
                    objGoal.Anios = 0;
                    objGoal.InversionInicial = 0;
                    objGoal.AporteMensual = 0;
                    objGoal.MontoObjetivo = 0;
                    objGoal.EntidadFinanciera = "Sin Entidad Financiera";                   
                    objGoal.FechaCreacion = DateTime.Now;
                    objGoal.Portfolio = new PortfolioDTO();

                    goals.Add(objGoal);

                    return goals;
                }

                foreach (var item in goalsByUserID)
                {
                    var objGoal = new GoalsDTO();
                    objGoal.TituloMeta = item.TituloMeta;
                    objGoal.Anios = item.Anios;
                    objGoal.InversionInicial = item.InversionInicial;
                    objGoal.AporteMensual = item.AporteMensual;
                    objGoal.MontoObjetivo = item.MontoObjetivo;
                    objGoal.EntidadFinanciera = item.EntidadFinanciera;
                    objGoal.FechaCreacion = item.FechaCreacion;
                    objGoal.Portfolio = new PortfolioDTO();

                    if (objGoal.Portfolio != null)
                    {
                        
                        objGoal.Portfolio.Nombre = item.Portafolio.NombrePortafolio;
                        objGoal.Portfolio.Descripcion = item.Portafolio.Descripcion;
                        objGoal.Portfolio.RangoMax = item.Portafolio.RangoMax;
                        objGoal.Portfolio.RangoMin = item.Portafolio.RangoMin;
                        objGoal.Portfolio.ComisionBP = item.Portafolio.ComisionBP;
                        objGoal.Portfolio.RentabilidadEstimada = item.Portafolio.RentabilidadEstimada;
                        objGoal.Portfolio.Rentabilidad = item.Portafolio.Rentabilidad;
                    }

                    goals.Add(objGoal);
                }

            }
            catch (Exception ex)
            {
               return goals;
            }

            return goals;
        }

        public async Task<GoalDetailsDTO> GetGoalDetail(int userId, int goalId)
        {
            GoalDetailsDTO detailsDTO = new GoalDetailsDTO();
            try
            {
                var goalUserDetails = await (
                                      from g in _db.Goals
                                      where g.Userid == userId && g.Id == goalId
                                      select new
                                      {
                                          GoalId = g.Id,                                          
                                          TituloMeta = g.Title,
                                          Anios = g.Years,
                                          InversionInicial = g.Initialinvestment,
                                          AporteMensual = g.Monthlycontribution,
                                          MontoObjetivo = g.Targetamount,
                                          CategorioMeta = g.Goalcategory.Title,
                                          EntidadFinanciera = g.Financialentity.Title
                                      }
                                      ).ToListAsync();

                if (goalUserDetails == null)
                    return null;

                //Calcular balance
                double balance = GetBalanceByUserIdAndGoalId(userId, goalId);

                

                //calcular retiros
                //calcular aportes

            }
            catch (Exception ex)
            {
                return null;
            }

            return detailsDTO;
        }

        private double GetBalanceByUserIdAndGoalId(int userId, int goalId)
        {
            double totBalance = 0d;
            try
            {
                var goalTransForBalance = (from gtf in _db.Goaltransactionfundings
                                           join fs in _db.Fundingsharevalues on gtf.Fundingid equals fs.Fundingid
                                           where gtf.Ownerid == userId && gtf.Goalid == goalId
                                           && fs.Date.CompareTo(gtf.Date) == 0
                                           select new
                                           {

                                               Quotas = gtf.Funding.Isbox == true ? 1d : gtf.Quotas,
                                               FundingShareValue = fs.Value,
                                               DestinationCurrencyId = gtf.Goal.Displaycurrencyid,
                                               SourceCurrencyId = gtf.Goal.Currencyid,
                                               LastDate = gtf.Date
                                           }).ToList();

                if (goalTransForBalance == null || goalTransForBalance.Count <= 0)
                    return totBalance;

                foreach (var item in goalTransForBalance)
                {
                    BalanceDTO balance = new BalanceDTO();
                   // balance

                }
            }
            catch (Exception)
            {
                return 0d;
            }

            return totBalance;
        }
    }
}
