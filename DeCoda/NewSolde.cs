using System;
namespace DeCoda
{
    public class NewSolde
    {
        private string Line;

        public string StructureCompte { get; set; }
        public string NumSequence { get; set; }
        public string NumCompte { get; set; }
        public string Devise { get; set; }
        public string Signe { get; set; }
        public string Solde { get; set; }
        public string Date { get; set; }
        public string NomTitulaire { get; set; }
        public string Libelle { get; set; }
        public string NumSeqence { get; set; }

        public NewSolde(string line)
        {
            if (line[0] != '1')
                throw new InvalidOperationException();
            Line = line;
            StructureCompte = line.Substring(1, 1);
            NumSequence = line.Substring(2, 3);
            NumCompte = line.Substring(5, 34);
            Devise = line.Substring(39, 3);
            Signe = line.Substring(42, 1);
            Solde = line.Substring(43, 15);
            Date = line.Substring(58, 6);
            NomTitulaire = line.Substring(64, 26);
            Libelle = line.Substring(90, 35);
            NumSeqence = line.Substring(125, 3);
        }
    }
}
