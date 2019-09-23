using System;
namespace DeCoda
{
    public class Header
    {
        private string Line;

        public string Date { get; set; }
        public string BankId { get; set; }
        public string CodeApplication { get; set; }
        public string Duplicata { get; set; }
        public string RefFichier { get; set; }
        public string Nom { get; set; }
        public string BIC { get; set; }
        public string TitulaireId { get; set; }
        public string CodeApplicationDistincte { get; set; }
        public string RefTransaction { get; set; }
        public string RefRelated { get; set; }
        public string Version { get; set; }

        public Header(string line)
        {
            if (line[0] != '0')
                throw new InvalidOperationException();
            Line = line;
            Date = line.Substring(5, 6).TrimEnd();
            BankId = line.Substring(11, 3).TrimEnd();
            CodeApplication = line.Substring(14, 2).TrimEnd();
            Duplicata = line.Substring(16, 1).TrimEnd();
            RefFichier = line.Substring(24, 10).TrimEnd();
            Nom = line.Substring(34, 26).TrimEnd();
            BIC = line.Substring(60, 11).TrimEnd();
            TitulaireId = line.Substring(71, 11).TrimEnd();
            CodeApplicationDistincte = line.Substring(83, 5).TrimEnd();
            RefTransaction = line.Substring(88, 16).TrimEnd();
            RefRelated = line.Substring(104, 16).TrimEnd();
            Version = line.Substring(127, 1).TrimEnd();
        }
    }
}
