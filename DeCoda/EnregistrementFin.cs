using System;
namespace DeCoda
{
    public class EnregistrementFin
    {
        private string Line;

        public string NbEnregistrement { get; set; }
        public string ChiffreAffaireDebit { get; set; }
        public string ChiffreAffaireCredit { get; set; }
        public string CodeMultiple { get; set; }

        public EnregistrementFin(string line)
        {
            if (line[0] != '9')
                throw new InvalidOperationException();
            Line = line;
            NbEnregistrement = line.Substring(16, 6);
            ChiffreAffaireDebit = line.Substring(22, 15);
            ChiffreAffaireCredit = line.Substring(37, 15);
            CodeMultiple = line.Substring(127, 1);
        }
    }
}
