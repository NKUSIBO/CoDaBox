using Inocrea.CodaBox.ApiModel.Models;

namespace Inocrea.CodaBox.ApiServer.Entities2.ViewModel
{
    public class BalanceViewModel2 : CompteBancaire
    {
        public string Date { get; set; }
        public double InitialBalance { get;set; }
        public double NewBalance { get;set; }
        public double Difference { get;set; }
    }
}
