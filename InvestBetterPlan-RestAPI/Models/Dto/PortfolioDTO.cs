namespace InvestBetterPlan_RestAPI.Models.Dto
{
    public class PortfolioDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double RangoMax { get; set; }
        public double RangoMin { get; set; }
        public double ComisionBP { get; set; }
        public double RentabilidadEstimada { get; set; }
        public string Rentabilidad { get; set; }
    }
}
