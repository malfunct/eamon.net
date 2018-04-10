using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppleTextFileConvertor
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "CHARACTERS";

            if (args.Length > 0)
            {
                fileName = args[0];
            }

            using (var reader = File.Open(fileName, FileMode.Open))
            {
                using (var writer = new FileStream($"{fileName}.txt", FileMode.CreateNew, FileAccess.Write))
                {
                    int b;
                    while ((b = reader.ReadByte()) >= 0)
                    {
                        if (b == 141) //enter
                        {
                            writer.WriteByte((byte)'\n');
                        }
                        else if (b >= 160)
                        {
                            writer.WriteByte((byte)(b-128));
                        }
                        else
                        {
                            writer.WriteByte((byte)b);
                        }
                    }
                }
            }
        }
    }
}
