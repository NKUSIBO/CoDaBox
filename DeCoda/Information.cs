using System;

namespace DeCoda
{
    public class Information
    {
        private string Line1;
        private string Line2;
        private string Line3;

        //partie 1
        public string NumSequence1 { get; set; }
        public string NumDetail1 { get; set; }
        public string NumRef { get; set; }
        public string CodeOperation { get; set; }
        public string CodeCommunication { get; set; }
        public string Communication1 { get; set; }
        public string CodeSuite1 { get; set; }
        public string CodeLien1 { get; set; }

        //partie 2
        public string NumSequence2 { get; set; }
        public string NumDetail2 { get; set; }
        public string Communication2 { get; set; }
        public string CodeSuite2 { get; set; }
        public string CodeLien2 { get; set; }

        //partie 3
        public string NumSequence3 { get; set; }
        public string NumDetail3 { get; set; }
        public string Communication3 { get; set; }
        public string CodeSuite3 { get; set; }
        public string CodeLien3 { get; set; }

        public Information(string line)
        {
            if (line[0] != '3')
                throw new InvalidOperationException();
            if (line[1] != '1')
                throw new InvalidOperationException();
            Line1 = line;

            NumSequence1 = line.Substring(2, 4);
            NumDetail1 = line.Substring(6, 4);
            NumRef = line.Substring(10, 21);
            CodeOperation = line.Substring(31, 8);
            CodeCommunication = line.Substring(39, 1);
            Communication1 = line.Substring(40, 73);
            CodeSuite1 = line.Substring(125, 1);
            CodeLien1 = line.Substring(127, 1);
        }

        public void Complete2(string line)
        {
            if (line[0] != '3')
                throw new InvalidOperationException();
            if (line[1] != '2')
                throw new InvalidOperationException();
            Line2 = line;

            NumSequence2 = line.Substring(2, 4);
            NumDetail2 = line.Substring(6, 4);
            Communication2 = line.Substring(40, 73);
            CodeSuite2 = line.Substring(125, 1);
            CodeLien2 = line.Substring(127, 1);
        }

        public void Complete3(string line)
        {
            if (line[0] != '3')
                throw new InvalidOperationException();
            if (line[1] != '3')
                throw new InvalidOperationException();
            Line3 = line;

            NumSequence3 = line.Substring(2, 4);
            NumDetail3 = line.Substring(6, 4);
            Communication3 = line.Substring(40, 73);
            CodeSuite3 = line.Substring(125, 1);
            CodeLien3 = line.Substring(127, 1);
        }
    }
}