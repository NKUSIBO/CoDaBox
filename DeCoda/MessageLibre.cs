using System;
namespace DeCoda
{
    public class MessageLibre
    {
        private string Line;

        public string NumSequence { get; set; }
        public string NumDetail { get; set; }
        public string Text { get; set; }
        public string CodeLien { get; set; }


        public MessageLibre(string line)
        {
            if (line[0] != '1')
                throw new InvalidOperationException();
            Line = line;
            NumSequence = line.Substring(2, 4);
            NumDetail = line.Substring(6, 4);
            Text = line.Substring(32, 80);
            CodeLien = line.Substring(127, 1);
        }
    }
}
