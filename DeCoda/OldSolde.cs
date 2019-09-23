using System;
using System.Collections.Generic;

namespace DeCoda
{
    public class OldSolde
    {
        private string Line;

        public string NumSequencePapier { get; set; }
        public string NumSequence { get; set; }
        public string NumCompte { get; set; }
        public string Devise { get; set; }
        public string Signe { get; set; }
        public string Solde { get; set; }
        public string Date { get; set; }
        public string TitulaireCompte { get; set; }
        public string LibelleCompte { get; set; }

        public List<MessageLibre> MessageLibres { get; set; }

        public OldSolde(string line)
        {
            if (line[0] != '8')
                throw new InvalidOperationException();
            Line = line;
            NumSequencePapier = line.Substring(1, 3);
            NumCompte = line.Substring(4, 34);
            Devise = line.Substring(38, 3);
            Signe = line.Substring(41, 1);
            Solde = line.Substring(42, 15);
            Date = line.Substring(57, 6);
            TitulaireCompte = line.Substring(54, 26);
            LibelleCompte = line.Substring(90, 35);
            NumSequence = line.Substring(125, 3);
        }
    }
}
