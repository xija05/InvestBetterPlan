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
                                                            && gtf.Transaction.Type == Constants.c_GoalTransactionType_Buy
                                                            && gtf.Quotas.HasValue == true && gtf.Amount.HasValue == true
                                                            && fs.Date.CompareTo(gtf.Date) == 0 && gtf.Amount > 0 && gtf.Quotas > 0
                                                            
                                                            select new
                                                            {
                                                                IdUser = gtf.Ownerid,
                                                                DestinationCurrencyId = gtf.Goal.Displaycurrencyid,
                                                                SourceCurrencyId = gtf.Goal.Currencyid,
                                                                DateGTF = gtf.Date,
                                                                Quotas = gtf.Funding.Isbox == true? 1d: gtf.Quotas,
                                                                GTAmount = gtf.Transaction.Amount.Value,
                                                                FSValue = fs.Value
                                                            }).ToListAsync();

                if (summaryBalanceYAporteActual == null || summaryBalanceYAporteActual.Count <= 0)
                {
                    SummaryDTO objSummary = new SummaryDTO();
                    objSummary.Balance = "Sin balance";
                    objSummary.AportesActuales = "Sin aportes";
                    lstSummary.Add(objSummary);
                    return lstSummary;
                }

              
                List<BalanceDTO> lstBalanceYAportes = new List<BalanceDTO>();

                #region " Currency CLP "

                var lstSummBalYApCLP =  summaryBalanceYAporteActual.FindAll(p => p.DestinationCurrencyId == p.SourceCurrencyId && p.DestinationCurrencyId == Constants.c_CurrencyId_CLP);

                if (lstSummBalYApCLP != null && lstSummBalYApCLP.Count > 0)
                {
                    SummaryDTO objSummaryCLP = new SummaryDTO();

                    objSummaryCLP.Balance = Convert.ToDouble(lstSummBalYApCLP.Sum(x => x.Quotas * x.FSValue * 1d)).ToString(Constants.c_CurrencyServerFormatNumber_CLP);
                    objSummaryCLP.AportesActuales = Convert.ToDouble(lstSummBalYApCLP.Sum(x => x.GTAmount * 1d)).ToString(Constants.c_CurrencyServerFormatNumber_CLP);
                    lstSummary.Add(objSummaryCLP);
                }

                #endregion

                #region " Currency CLF "

                var lstSummBalYApCLF = summaryBalanceYAporteActual.FindAll(p => p.DestinationCurrencyId == p.SourceCurrencyId && p.DestinationCurrencyId == Constants.c_CurrencyId_CLF);

                if (lstSummBalYApCLF != null && lstSummBalYApCLF.Count > 0)
                {
                    SummaryDTO objSummaryCLF = new SummaryDTO();

                    objSummaryCLF.Balance = Convert.ToDouble(lstSummBalYApCLF.Sum(x => x.Quotas * x.FSValue * 1d)).ToString(Constants.c_CurrencyServerFormatNumber_CLF);
                    objSummaryCLF.AportesActuales = Convert.ToDouble(lstSummBalYApCLF.Sum(x => x.GTAmount * 1d)).ToString(Constants.c_CurrencyServerFormatNumber_CLF);
                    lstSummary.Add(objSummaryCLF);
                }
                #endregion

                #region " Currency USD "

                var lstSummBalYApUSD = summaryBalanceYAporteActual.FindAll(p => p.DestinationCurrencyId == p.SourceCurrencyId && p.DestinationCurrencyId == Constants.c_CurrencyId_USD);

                if (lstSummBalYApUSD != null && lstSummBalYApUSD.Count > 0)
                {
                    SummaryDTO objSummaryUSD = new SummaryDTO();

                    objSummaryUSD.Balance = Convert.ToDouble(lstSummBalYApUSD.Sum(x => x.Quotas * x.FSValue * 1d)).ToString(Constants.c_CurrencyServerFormatNumber_USD);
                    objSummaryUSD.AportesActuales = Convert.ToDouble(lstSummBalYApUSD.Sum(x => x.GTAmount * 1d)).ToString(Constants.c_CurrencyServerFormatNumber_USD);
                    lstSummary.Add(objSummaryUSD);
                }
                #endregion

                #region " Currency EUR "

                var lstSummBalYApEUR = summaryBalanceYAporteActual.FindAll(p => p.DestinationCurrencyId == p.SourceCurrencyId && p.DestinationCurrencyId == Constants.c_CurrencyId_EUR);

                if (lstSummBalYApEUR != null && lstSummBalYApEUR.Count > 0)
                {
                    SummaryDTO objSummaryEUR = new SummaryDTO();

                    objSummaryEUR.Balance = Convert.ToDouble(lstSummBalYApEUR.Sum(x => x.Quotas * x.FSValue * 1d)).ToString(Constants.c_CurrencyServerFormatNumber_EUR);
                    objSummaryEUR.AportesActuales = Convert.ToDouble(lstSummBalYApEUR.Sum(x => x.GTAmount * 1d)).ToString(Constants.c_CurrencyServerFormatNumber_EUR);
                    lstSummary.Add(objSummaryEUR);
                }
                #endregion

                var lstSummBalyAportParaConvertir = summaryBalanceYAporteActual.FindAll(p => p.DestinationCurrencyId != p.SourceCurrencyId);

              
              
                foreach (var summ in lstSummBalyAportParaConvertir)
                {
                    BalanceDTO balanceDTO = new BalanceDTO();
                    balanceDTO.UserId = summ.IdUser;
                    balanceDTO.DestinationCurrency = summ.DestinationCurrencyId.Value;
                    balanceDTO.SourceCurrency = summ.SourceCurrencyId.Value;
                    balanceDTO.Amount = summ.GTAmount;
                    balanceDTO.Quotas = (double)summ.Quotas;
                    balanceDTO.LastDate = summ.DateGTF;
                    balanceDTO.FundingShareValue = summ.FSValue;
                    balanceDTO.CurrencyIndicator = GetCurencyIndicator(balanceDTO.SourceCurrency, balanceDTO.DestinationCurrency, summ.DateGTF);

                    lstBalanceYAportes.Add(balanceDTO);
                }

                if(lstBalanceYAportes.Count > 0)
                {
                    SummaryDTO objSummary = new SummaryDTO();

                    objSummary.Balance = Convert.ToDouble(lstBalanceYAportes.Sum(x => x.Quotas * x.FundingShareValue * x.CurrencyIndicator)).ToString();
                    objSummary.AportesActuales = Convert.ToDouble(lstBalanceYAportes.Sum(x => x.FundingShareValue * x.CurrencyIndicator)).ToString();
                    lstSummary.Add(objSummary);
                }
            }
            catch (Exception ex)
            {
                return lstSummary;
            }

            return lstSummary;
        }


        private string GetCurrencyNameById(int? currencyId)
        {
            if (!currencyId.HasValue)
                return String.Empty;

            switch (currencyId.Value)
            {
                case Constants.c_CurrencyId_CLP:
                    return Constants.c_CurrencyName_CLP;
                case Constants.c_CurrencyId_USD:
                    return Constants.c_CurrencyName_USD;
                case Constants.c_CurrencyId_CLF:
                    return Constants.c_CurrencyName_CLF;
                case Constants.c_CurrencyId_EUR:
                    return Constants.c_CurrencyName_EUR;
                default:
                    return String.Empty;
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

       
    }
}
