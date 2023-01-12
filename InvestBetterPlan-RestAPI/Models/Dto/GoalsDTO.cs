namespace InvestBetterPlan_RestAPI.Models.Dto
{
    public class GoalsDTO
    {
        public string TituloMeta { get; set; }
        public int Anios { get; set; }
        public int InversionInicial { get; set; }
        public int AporteMensual { get; set; }
        public int MontoObjetivo { get; set; }
        public string EntidadFinanciera { get; set; }
        public DateTime FechaCreacion { get; set; } 
        
        public PortfolioDTO Portfolio { get; set; }
    }
}
