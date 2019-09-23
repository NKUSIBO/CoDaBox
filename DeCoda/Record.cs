using System;
using System.Collections.Generic;
using System.Linq;
using Inocrea.CodaBox.ApiModel.Models;

namespace DeCoda
{
    public class Record
    {
        public List<Mouvement> mouvements;
        public Header header;
        public NewSolde newSolde;
        public OldSolde oldSolde;
        public EnregistrementFin fin;

        public Record(string[] file)
        {
            mouvements = new List<Mouvement>();

            foreach (var line in file)
            {
                if (line.Length <= 0)
                    continue;
                var code = line[0];
                switch (code)
                {
                    case '0':
                        header = new Header(line);
                        break;
                    case '1':
                        newSolde = new NewSolde(line);
                        break;
                    case '2':
                        if (line[1] == '1')
                            mouvements.Add(new Mouvement(line));
                        else if (line[1] == '2')
                            mouvements.Last().Complete2(line);
                        else if (line[1] == '3')
                            mouvements.Last().Complete3(line);
                        break;
                    case '3':
                        if (line[1] == '1')
                            mouvements.Last().Informations.Add(new Information(line));
                        else if (line[1] == '2')
                            mouvements.Last().Informations.Last().Complete2(line);
                        else if (line[1] == '3')
                            mouvements.Last().Informations.Last().Complete3(line);
                        break;
                    case '4':
                        //throw new Exception("Message libre dans oldSolde");
                    case '8':
                        oldSolde = new OldSolde(line);
                        break;
                    case '9':
                        fin = new EnregistrementFin(line);
                        break;
                    default:

                        break;
                }
            }
        }
    }
}
