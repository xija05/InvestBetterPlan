using InvestBetterPlan_RestAPI.Models;
using InvestBetterPlan_RestAPI.Models.Constants;
using InvestBetterPlan_RestAPI.Models.Dto;
using InvestBetterPlan_RestAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace InvestBetterPlan_RestAPI.Repository
{
    public class SummaryRepository: ISummaryRepository
    {
        public challengeContext _db { get; }

        public SummaryRepository(challengeContext db)
        {
            _db = db;
        }


        public async Task<List<SummaryDTO>> GetSummary(int userId)
        {

            List<SummaryDTO> lstSummary = new List<SummaryDTO>();
            try
            {
                var summaryBalanceYAporteActual = await (from gtf in _db.Goaltransactionfundings
                                                            join fs in _db.Fundingsharevalues on gtf.Fundingid equals fs.Fundingid
                                                            where gtf.Ownerid == userId && gtf.Transactionid.HasValue == true 
                                                            && gtf.Quotas.HasValue == true && gtf.Amount.HasValue == true
                                                            && fs.Date.CompareTo(gtf.Date) == 0
                                                            
                                                            select new BalanceAporte
                                                            {
                                                                SourceCurrencyId = (int)gtf.Transaction.Currencyid,
                                                                DestinationCurrencyId = (int) gtf.Goal.Displaycurrencyid,
                                                                LastDate = gtf.Date,
                                                                Quotas = (double)gtf.Quotas,
                                                                Amount = gtf.Transaction.Amount.Value,
                                                                FundingShareValue = fs.Value, 
                                                                IsBox= gtf.Funding.Isbox,
                                                                Type =gtf.Type,
                                                                CurrencyIndicator = 1d
                                                            }).ToListAsync();

                if (summaryBalanceYAporteActual == null || summaryBalanceYAporteActual.Count <= 0)
                {
                    SummaryDTO objSummary = new SummaryDTO();
                    objSummary.Balance = "Sin balance";
                    objSummary.AportesActuales = "Sin aportes";
                    lstSummary.Add(objSummary);
                    return lstSummary;
                }

              
                #region " Currency CLP "

                var summBalYApCLP = GetSummaryCurrencyByCurrencyId(summaryBalanceYAporteActual.FindAll(p => p.DestinationCurrencyId == Constants.c_CurrencyId_CLP), Constants.c_CurrencyId_CLP);

                if(summBalYApCLP != null)
                    lstSummary.Add(summBalYApCLP);
                #endregion

                #region " Currency CLF "

                var summBalYApCLF = GetSummaryCurrencyByCurrencyId(summaryBalanceYAporteActual.FindAll(p => p.DestinationCurrencyId == Constants.c_CurrencyId_CLF), Constants.c_CurrencyId_CLF);

                if (summBalYApCLF != null)
                    lstSummary.Add(summBalYApCLF);
                #endregion

                #region " Currency USD "

                var summBalYApUSD = GetSummaryCurrencyByCurrencyId(summaryBalanceYAporteActual.FindAll(p => p.DestinationCurrencyId == Constants.c_CurrencyId_USD), Constants.c_CurrencyId_USD);

                if (summBalYApUSD != null)
                    lstSummary.Add(summBalYApUSD);
                #endregion

                #region " Currency EUR "

                var summBalYApEUR = GetSummaryCurrencyByCurrencyId(summaryBalanceYAporteActual.FindAll(p => p.DestinationCurrencyId == Constants.c_CurrencyId_EUR), Constants.c_CurrencyId_EUR);

                if (summBalYApEUR != null)
                    lstSummary.Add(summBalYApEUR);
                #endregion


            }
            catch (Exception ex)
            {
                return lstSummary;
            }

            return lstSummary;
        }

        private SummaryDTO GetSummaryCurrencyByCurrencyId(List<BalanceAporte> balanceAportes, int currencyId)
        {
            try
            {
                SummaryDTO summBalanceYAporte = new SummaryDTO();
                string currencyServerFormatNumber = GetCurrencyServerFormatNumByCurrencyId(currencyId);

                if (balanceAportes.Count > 0)
                    return null;

                foreach (BalanceAporte item in balanceAportes)
                {
                    if (item.SourceCurrencyId == item.DestinationCurrencyId)
                        item.CurrencyIndicator = 1d;
                    else
                        item.CurrencyIndicator = GetCurencyIndicator(item.SourceCurrencyId, item.DestinationCurrencyId, item.LastDate);

                    if (item.IsBox == true)
                        item.Quotas = 1d;
                }

                summBalanceYAporte.Balance = Convert.ToDouble(
                                        balanceAportes.Sum(x => x.Quotas * x.FundingShareValue * x.CurrencyIndicator))
                                        .ToString(currencyServerFormatNumber);
                summBalanceYAporte.AportesActuales = Convert.ToDouble(
                                        balanceAportes.FindAll(x => x.Type == Constants.c_GoalTransactionType_Buy)
                                        .Sum( x => x.Amount * x.CurrencyIndicator))
                                        .ToString(currencyServerFormatNumber);

                return summBalanceYAporte;

            }
            catch (Exception ex)
            {
                return null; ;
            }
        }

      

        private double GetCurencyIndicator(int sourceCurrencyId, int destinationCurrencyId, DateOnly fecha)
        {
            try
            {
                var montoConvertido = from s in _db.Currencyindicators
                                      where s.Sourcecurrencyid == sourceCurrencyId
                                      && s.Destinationcurrencyid == destinationCurrencyId
                                      && s.Date.ToString(Constants.c_fechaFormatoSoloFecha) == fecha.ToString(Constants.c_fechaFormatoSoloFecha)
                                      select s.Value;

                //No existe valor del currencyIndicator
                if (montoConvertido == null)
                    return 1d;

                return Convert.ToDouble(montoConvertido);
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
