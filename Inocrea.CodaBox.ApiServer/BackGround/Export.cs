using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiServer.Services;

namespace Inocrea.CodaBox.ApiServer.BackGround
{
    public static class Export
    {

        public static async Task WriteTsvAsync<T>(IEnumerable<T> data, string fileName)
        {
            string output = "";

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output += prop.DisplayName + '\t'; // header
            }
            output += '\n';
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output += prop.Converter.ConvertToString(prop.GetValue(item)) + '\t';
                }
                output += '\n';
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(output);
            writer.Flush();
            stream.Position = 0;


            var apiWD = new ApiWorkDrive();
            await apiWD.UploadXls(stream, fileName);


        }
    }
}
