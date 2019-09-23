using System;
using System.Collections.Generic;

namespace DeCoda
{
    public class Mouvement
    {

        //partie 1
        public string CodeArticle { get; set; }
        public string NumsSequence { get; set; }
        public string NumDetail1 { get; set; }
        public string NumRef { get; set; }
        public string Signe { get; set; }
        public string Montant { get; set; }
        public string Date { get; set; }
        public string CodeOperation { get; set; }
        public string TypeDeCommunication { get; set; }
        public string ZoneDeCommunicationNumCompte { get; set; }
        public string DateComptabilisation { get; set; }
        public string NumsSequenceExtrait { get; set; }
        public string CodeGlobalisation { get; set; }
        public string CodeSuite1 { get; set; }
        public string CodeLien1 { get; set; }

        //partie 2
        public string NumsSequencePermanent2 { get; set; }
        public string NumDetail2 { get; set; }
        public string RefClient { get; set; }
        public string Bic { get; set; }
        public string TypeTransaction { get; set; }
        public string Iso { get; set; }
        public string CategoryPurpose { get; set; }
        public string Purpose { get; set; }
        public string CodeSuite2 { get; set; }
        public string CodeLien2 { get; set; }

        //partie 3
        public string NumsSequencePermanent3 { get; set; }
        public string NumDetail3 { get; set; }
        public string NumCompteContrepartie { get; set; }
        public string DeviseCompteContrepartie { get; set; }
        public string NomCompteContrepartie { get; set; }
        public string CodeLien3 { get; set; }

        //autre
        public string CommunicationStructuree { get; set; }
        public List<Information> Informations { get; set; }

        public Mouvement(string line)
        {
            if (line[0] != '2')
                throw new InvalidOperationException();
            if (line[1] != '1')
                throw new InvalidOperationException();

            Informations = new List<Information>();
            CodeArticle = line.Substring(1, 1);
            NumsSequence = line.Substring(2, 4);
            NumDetail1 = line.Substring(6, 4);
            NumRef = line.Substring(10, 21);
            Signe = line.Substring(31, 1);
            Montant = line.Substring(32, 15);
            Date = line.Substring(47, 6);
            CodeOperation = line.Substring(53, 8);
            TypeDeCommunication = line.Substring(61, 1);
            ZoneDeCommunicationNumCompte = line.Substring(62, 53);
            DateComptabilisation = line.Substring(115, 6);
            NumsSequenceExtrait = line.Substring(121, 3);
            CodeGlobalisation = line.Substring(124, 1);
            CodeSuite1 = line.Substring(125, 1);
            CodeLien1 = line.Substring(127, 1);
        }

        public void Complete2(string line)
        {
            if (line[0] != '2')
                throw new InvalidOperationException();
            if (line[1] != '2')
                throw new InvalidOperationException();

            NumsSequencePermanent2 = line.Substring(2, 4);
            NumDetail2 = line.Substring(6, 4);
            ZoneDeCommunicationNumCompte += line.Substring(10, 53);
            RefClient = line.Substring(63, 35);
            Bic = line.Substring(98, 11);
            TypeTransaction = line.Substring(112, 1);
            Iso = line.Substring(113, 4);
            CategoryPurpose = line.Substring(117, 4);
            Purpose = line.Substring(121, 4);
            CodeSuite2 = line.Substring(125, 1);
            CodeLien2 = line.Substring(127, 1);
        }

        public void Complete3(string line)
        {
            if (line[0] != '2')
                throw new InvalidOperationException();
            if (line[1] != '3')
                throw new InvalidOperationException();

            NumsSequencePermanent3 = line.Substring(2, 4);
            NumDetail3 = line.Substring(6, 4);
            NumCompteContrepartie = line.Substring(10, 34).TrimEnd();
            DeviseCompteContrepartie = line.Substring(44, 3);
            NomCompteContrepartie = line.Substring(47, 35).TrimEnd();
            ZoneDeCommunicationNumCompte += line.Substring(83, 43);
            CodeLien3 = line.Substring(127, 1);

            if (TypeDeCommunication == "1")
            {
                CommunicationStructuree = ZoneDeCommunicationNumCompte.Substring(82, 61);
                CommunicationStructuree.TrimEnd();
            }
        }

        public string GetCommunication()
        {
            var txt = string.Empty;
            txt += ZoneDeCommunicationNumCompte;
            foreach (var info in Informations)
            {
                txt += '\n' + info.Communication1;
                txt += '\n' + info.Communication2;
                txt += '\n' + info.Communication3;
            }

            return txt;
        }


        public override string ToString()
        {
            var txt = string.Empty;

            txt += "com\n";
            txt += ZoneDeCommunicationNumCompte;
            foreach(var info in Informations)
            {
                txt += "\ninfo com";
                txt += '\n' + info.Communication1;
                txt += '\n' + info.Communication2;
                txt += '\n' + info.Communication3;
            }

            return txt;
        }

    }
}
