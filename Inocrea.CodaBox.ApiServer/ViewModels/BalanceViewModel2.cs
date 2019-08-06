using Inocrea.CodaBox.ApiServer.Entities;

namespace Inocrea.CodaBox.ApiServer.ViewModels
{
    public class BalanceViewModel2 : CompteBancaire
    {
        public string Date { get; set; }
        public double InitialBalance { get;set; }
        public double NewBalance { get;set; }
        public double Difference { get;set; }
    }
}
