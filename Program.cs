using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stringLen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string text = "text.txt";
            if (!File.Exists(text)) // проверка существования
            {
                Console.WriteLine("Файл не найден.");
                return;
            }
            double result = 0;
            using (StreamReader reader = new StreamReader(text, Encoding.UTF8)) //чтение построчно
            {
                result = strLen(reader);
                Console.WriteLine(result.ToString("F2"));
            }
            Console.ReadKey();
        }
        static double strLen(StreamReader reader)
        {
            string line;
            double k = 0;
            double part = 0;
            double j = 0;
            double len = 0;
            while ((line = reader.ReadLine()) != null)
            {
                len += line.Length;
                for (int i = 0; i < line.Length - 1; i ++)
                {
                    if (line[i] == line[i + 1])
                    {
                        k++;
                        if (j == 0)
                        {
                            part++;
                            j++;
                        }
                    }
                    else 
                    {
                        j = 0;
                    }
                }
            }
            k = k + part;
            double lengthStr;
            Console.WriteLine(k);
            Console.WriteLine(part);
            lengthStr = (k - 2 * part) / len;
            return lengthStr;
        }
    }
}
