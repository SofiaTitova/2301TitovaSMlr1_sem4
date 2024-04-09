using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace arifmeticCoding
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string s = "eeedaaba";
            char[] chars = { 'a', 'b', 'c', 'd', 'e'};
            double[] pos = {0.5 , 0.25, 0.0625, 0.125, 0.0625};
            Console.WriteLine("Для алфавита: ");
            int i = 0;
            for (i = 0; i < pos.Length; i++)
            {
                Console.WriteLine("{0} - {1}", chars[i], pos[i]);
            }
            Console.WriteLine("Строка: {0}", s);
            double[] p = new double[s.Length];
            double res = arifm_coding(s, pos, chars);
            Console.WriteLine("Среднее значение {0}", res);
            string rev_str = reverse_arifm_coding(res, pos, chars, s.Length);
            Console.WriteLine("Обратное перобразование {0}", rev_str);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
            string text = "text.txt";
            string line;                
            string wholeText = "";
            using (StreamReader reader = new StreamReader(text, Encoding.UTF8))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    wholeText += line + " ";
                }
            }
            int chunkSize = 10;
            Console.WriteLine("Для произвольного текста из файла: ");
            int j;
            string reverse_text = "reverse_text.txt";
            using (StreamWriter writer = new StreamWriter(reverse_text))
            {
                for (j = 0; j < wholeText.Length; j += chunkSize)
                {
                    string chunk = wholeText.Substring(j, Math.Min(chunkSize, wholeText.Length - j));
                    char[] chars2;
                    double[] pos2;
                    textAlphabet(chunk, out chars2, out pos2);               
                    for (i = 0; i < pos2.Length; i++)
                    {
                        Console.WriteLine("{0} - {1}", chars2[i], pos2[i]);
                    }
                    double res2 = arifm_coding(chunk, pos2, chars2);
                    Console.WriteLine("Среднее значение {0}", res2);
                    
                    string rev_toFile = reverse_arifm_coding(res2, pos2, chars2, chunk.Length);
                    writer.Write(rev_toFile);
                }
            }
            Console.WriteLine("Обратное перобразование записано в файл");
            Console.ReadKey();
        }

        static double arifm_coding(string s, double[] p, char[] chars)
        {
            double[] intervals = new double [p.Length + 1];
            double left_border = 0;
            double right_border = 1;
            double k = 0;
            for (int i = 1; i < p.Length + 1; i++)
            {
                k += p[i-1];
                intervals[i] = k;
            }
            char toFind = ' ';
            int index = 0;
            
            foreach (char c in s)
            {
                double part_length = right_border - left_border;
                toFind = c;
                index = Array.IndexOf(chars, toFind);
                left_border += intervals[index]*part_length;
                right_border = left_border + p[index]*part_length;
                if (left_border == right_border)
                {
                    break;
                }
            }
            double res = (left_border + right_border) / 2;
            Console.WriteLine("Левая граница {0}", left_border);
            Console.WriteLine("Правая граница {0}", right_border);
            return res;
        }
        static string reverse_arifm_coding(double code, double[] p, char[] chars, int len)
        {
            double[] intervals = new double[p.Length + 1];
            double k = 0;
            for (int i = 1; i < p.Length + 1; i++)
            {
                k += p[i - 1];
                intervals[i] = k;
            }
            string result = "";
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < p.Length; j++)
                {
                    if (code >= intervals[j] && code < intervals[j+1])
                    {
                        result += chars[j];
                        code = (code - intervals[j]) / (intervals[j+1] - intervals[j]);
                        break;
                    }
                }
            }
            return result;
        }
        static void textAlphabet(string s, out char[] chars, out double[] pos)
        {
            var alphabet = s.Where(c => char.IsLetter(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c)).Distinct().OrderBy(c => c).ToArray();
            Array.Sort(alphabet);
            chars = alphabet;
            double[] freq = new double[chars.Length];
            pos = new double[chars.Length];
            int k = 0;
            foreach (char ch in s)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    if (ch == alphabet[i])
                    {
                        freq[i] += 1;
                        k++;
                        break;
                    }
                }
            }
            for (int i = 0; i < chars.Length; i++)
            {
                pos[i] += freq[i] / k;
            }
        }
    }
}
