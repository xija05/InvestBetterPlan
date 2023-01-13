using InvestBetterPlan_RestAPI.Models;
using InvestBetterPlan_RestAPI.Models.Constants;
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
            try
            {
                var goalUserDetails = await (
                                      from g in _db.Goals
                                      where g.Userid == userId && g.Id == goalId
                                      select new GoalDetailsDTO
                                      {
                                          TituloMeta = g.Title,
                                          Anios = g.Years,
                                          InversionInicial = g.Initialinvestment,
                                          AporteMensual = g.Monthlycontribution,
                                          MontoObjetivo = g.Targetamount,
                                          CategoriaMeta = g.Goalcategory.Title,
                                          EntidadFinanciera = g.Financialentity.Title,
                                          TotalAportes = String.Empty,
                                          TotalRetiros = String.Empty, 
                                          PorcentajeCumplimientoMeta = String.Empty
                                      }
                                      ).ToListAsync();

                if (goalUserDetails == null)
                    return null;

                DetalleMeta detalleMeta = GetBalanceRetiroAporte(userId, goalId);

                if (detalleMeta == null)
                    return goalUserDetails[0];

                string CurrencyServerFormatNumber = GetCurrencyServerFormatNumByCurrencyId(detalleMeta.DestinationCurrencyId);

                if (detalleMeta.TotalBalance != 0)
                {
                    goalUserDetails[0].TotalAportes = detalleMeta.TotalAportes.ToString(CurrencyServerFormatNumber);
                    goalUserDetails[0].TotalRetiros = detalleMeta.TotalRetiros.ToString(CurrencyServerFormatNumber);
                    goalUserDetails[0].PorcentajeCumplimientoMeta = string.Format("{0}%", Math.Round(Math.Abs(detalleMeta.TotalBalance) / goalUserDetails[0].MontoObjetivo * 100), 2);
                }

                return goalUserDetails[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DetalleMeta GetBalanceRetiroAporte(int userId, int goalId)
        {
            DetalleMeta detalle = new DetalleMeta();

            var balanceYAportes = (from gtf in _db.Goaltransactionfundings
                                   join fs in _db.Fundingsharevalues on gtf.Fundingid equals fs.Fundingid
                                   where gtf.Ownerid == userId && gtf.Goalid == goalId
                                   && gtf.Transactionid.HasValue == true
                                   && gtf.Quotas.HasValue == true && gtf.Amount.HasValue == true
                                   && fs.Date.CompareTo(gtf.Date) == 0

                                   select new BalanceAporte
                                   {
                                       SourceCurrencyId = (int)gtf.Transaction.Currencyid,
                                       DestinationCurrencyId = (int)gtf.Goal.Displaycurrencyid,
                                       LastDate = gtf.Date,
                                       Quotas = (double)gtf.Quotas,
                                       Amount = gtf.Transaction.Amount.Value,
                                       FundingShareValue = fs.Value,
                                       IsBox = gtf.Funding.Isbox,
                                       Type = gtf.Type,
                                       CurrencyIndicator = 1d
                                   }).ToList<BalanceAporte>();



            if (balanceYAportes == null && balanceYAportes.Count <= 0)
            {
                detalle.DestinationCurrencyId = 0;
                detalle.TotalBalance = 0d;
                detalle.TotalAportes = 0d;
                detalle.TotalRetiros = 0d;

                return detalle;
            }

            foreach (var item in balanceYAportes)
            {
                if (item.IsBox == true)
                    item.Quotas = 1;

                if (item.DestinationCurrencyId != item.SourceCurrencyId)
                    item.CurrencyIndicator = GetCurencyIndicator(item.SourceCurrencyId, item.DestinationCurrencyId, item.LastDate);
            }

            detalle.DestinationCurrencyId = balanceYAportes[0].DestinationCurrencyId;
            detalle.TotalBalance = Math.Round(balanceYAportes.Sum(x => x.Quotas * x.FundingShareValue * x.CurrencyIndicator), 2);
            detalle.TotalAportes = Math.Round(balanceYAportes.FindAll(p => p.Type == Constants.c_GoalTransactionType_Buy).Sum(x => x.Amount * x.CurrencyIndicator), 2);
            detalle.TotalRetiros = Math.Round(balanceYAportes.FindAll(p => p.Type == Constants.c_GoalTransactionType_Sale).Sum(x => x.Amount * x.CurrencyIndicator), 2);


            return detalle;
        }

        private double GetCurencyIndicator(int sourceCurrencyId, int destinationCurrencyId, DateOnly fecha)
        {
            try
            {
                var currencyIndicator = (from s in _db.Currencyindicators
                                         where s.Sourcecurrencyid == sourceCurrencyId
                                         && s.Destinationcurrencyid == destinationCurrencyId
                                         && s.Date.CompareTo(fecha) == 0
                                         select s).ToList();

                //No existe valor del currencyIndicator
                if (currencyIndicator == null || currencyIndicator.Count <= 0)
                    return 1d;

                return Convert.ToDouble(currencyIndicator[0].Value);
            }
            catch (Exception)
            {
                return 0d;
            }
        }

        private string GetCurrencyServerFormatNumByCurrencyId(int currencyId)
        {
            switch (currencyId)
            {
                case Constants.c_CurrencyId_CLP:
                    return Constants.c_CurrencyServerFormatNumber_CLP;
                case Constants.c_CurrencyId_CLF:
                    return Constants.c_CurrencyServerFormatNumber_CLF;
                case Constants.c_CurrencyId_USD:
                    return Constants.c_CurrencyServerFormatNumber_USD;
                case Constants.c_CurrencyId_EUR:
                    return Constants.c_CurrencyServerFormatNumber_EUR;
                default:
                    return string.Empty;
            }
        }
    }
}
