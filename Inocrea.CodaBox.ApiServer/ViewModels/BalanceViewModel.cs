using Inocrea.CodaBox.ApiServer.Entities;

namespace Inocrea.CodaBox.ApiServer.ViewModels
{
    public class BalanceViewModel
    {
        public CompteBancaire CompteBancaire {get;set;}
        public string Date { get; set; }
        public double InitialBalance { get;set; }
        public double NewBalance { get;set; }
        public double Difference { get;set; }
    }
}
