namespace InvestBetterPlan_RestAPI.Models.Constants
{
    public class Constants
    {
        #region CurrencyId
        public const int c_CurrencyId_CLP = 1;
        public const int c_CurrencyId_USD = 2;
        public const int c_CurrencyId_CLF = 3;
        public const int c_CurrencyId_EUR = 4;
        #endregion

        #region " CurrencyName "
        public const string c_CurrencyName_CLP = "Pesos";
        public const string c_CurrencyName_USD = "Dólares";
        public const string c_CurrencyName_CLF = "Unidad de Fomento";
        public const string c_CurrencyName_EUR = "Euro";

        #endregion
        
        #region " GoalTransaction Types "
        public const string c_GoalTransactionType_Buy = "buy";
        public const string c_GoalTransactionType_Sale = "sale";
        public const string c_GoalTransactionType_TransGoalBuy = "transferGoalBuy";
        public const string c_GoalTransactionType_TransGoalSale = "transferGoalSale";
        public const string c_GoalTransactionType_ChangePortfolioBuy = "changePortfolioBuy";
        public const string c_GoalTransactionType_ChangePortfolioSale = "changePortfolioSale";

        #endregion

        #region "  Formatos de Fecha "
        public const string c_fechaFormatoSoloFecha = "yyyy-MM-dd";
        public const string c_fechaFormato = "dd-MM-yyyy";
        #endregion

        #region Formatos de Currency
        public const string  c_CurrencyServerFormatNumber_CLP = "$#,##0.";
        public const string c_CurrencyServerFormatNumber_USD = "USD #,##0.#";
        public const string c_CurrencyServerFormatNumber_CLF = "UF #,##0.#"; 
        public const string c_CurrencyServerFormatNumber_EUR = "EUR #,##0.#";


        #endregion


    }
}
