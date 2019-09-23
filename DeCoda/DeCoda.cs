using System;
using System.IO;
using Inocrea.CodaBox.ApiModel.Models;

namespace DeCoda
{
    public class DeCoda
    {

        internal Statements getStatement(string fullName)
        {
            var txt = File.ReadAllLines(fullName);
            return getStatement(txt);
        }

        public Statements getStatement(string[] txt)
        {
            var record = new Record(txt);
            var st = Util.GetStatement(record);
            return st;
        }
    }
}
