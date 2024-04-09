using bwt___mft__ha;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bwt___mtf___AC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string graf = "enwik7.txt"; //"graph.txt"
            string graf_bwt = "bwt_graf.txt";
            string graf_res = "resgraph.txt";
            //bigBWT(graf, graf_bwt);
            BWTFast bwt = new BWTFast();
            string wholeText = "";
            string line = "";
            using (StreamWriter res = new StreamWriter(graf_bwt))
            {
                using (StreamReader reader = new StreamReader(graf, Encoding.UTF8))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        wholeText += line;

                    }
                }
                bwt.GetBWT(wholeText);
                res.Write(wholeText);
            }
            Console.WriteLine("+");

            List<char> newAlphabet = new List<char>();
            int index = 0;
            line = "";
            string result = "";

            while (index < wholeText.Length)
            {
                int k = Math.Min(100, wholeText.Length - index);
                line = wholeText.Substring(index, k);
                newAlphabet = alphabet(line);
                result += MTF(line, newAlphabet);
                index += k;

            }
            Console.WriteLine("+");
            int chunkSize = 10;
            int j;
            using (StreamWriter writer = new StreamWriter(graf_res))
            {
                for (j = 0; j < result.Length; j += chunkSize)
                {
                    string chunk = result.Substring(j, Math.Min(chunkSize, wholeText.Length - j));
                    char[] chars2;
                    double[] pos2;
                    textAlphabet(chunk, out chars2, out pos2);

                    double res2 = arifm_coding(chunk, pos2, chars2);
                    string res = res2.ToString();
                    res = res.Substring(2, res.Length - 2);
                    long number = long.Parse(res); // Преобразование строки в целое число
                    res = number.ToString("X");
                    writer.Write(res);

                }
            }
            double re = FileCompressionRatio.GetCompressionRatio(graf_res, graf);
            Console.WriteLine(re);
            Console.ReadKey();
        }
        static List<char> alphabet(string s)
        {
            List<char> chars = new List<char>();
            for (int i = 0; i < s.Length; i++)
            {
                if (!chars.Contains(s[i]))
                {
                    chars.Add(s[i]);
                }
            }
            chars.Sort((a, b) => a.CompareTo(b));
            return chars;
        }

        static string MTF(string s, List<char> alphabet)
        {
            string res = "";
            int ind;
            char symbol;
            string integerVal;
            char strEl;
            char removedChar;
            for (int i = 0; i < s.Length; i++)
            {
                symbol = s[i];
                ind = alphabet.IndexOf(symbol);
                integerVal = ind.ToString();
                strEl = (char)(ind + 34);
                res += strEl.ToString() /*integerVal*/;
                removedChar = alphabet[ind];
                alphabet.RemoveAt(ind);
                alphabet.Insert(0, removedChar);
            }
            return res;
        }
        static double arifm_coding(string s, double[] p, char[] chars)
        {
            double[] intervals = new double[p.Length + 1];
            double left_border = 0;
            double right_border = 1;
            double k = 0;
            for (int i = 1; i < p.Length + 1; i++)
            {
                k += p[i - 1];
                intervals[i] = k;
            }
            char toFind = ' ';
            int index = 0;

            foreach (char c in s)
            {
                double part_length = right_border - left_border;
                toFind = c;
                index = Array.IndexOf(chars, toFind);
                left_border += intervals[index] * part_length;
                right_border = left_border + p[index] * part_length;
                if (left_border == right_border)
                {
                    break;
                }
            }
            double res = (left_border + right_border) / 2;

            return res;
        }

        static void textAlphabet(string s, out char[] chars, out double[] pos)
        {
            var alphabet = s.Distinct()
                .OrderBy(c => (int)c)
                .ToArray();
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
    public class HeapNode
    {
        public char Sym { get; set; }
        public int Val { get; set; }
        public HeapNode LeftChild { get; set; }
        public HeapNode RightChild { get; set; }

        public HeapNode(char sym, int val)
        {
            Sym = sym;
            Val = val;
        }
    }

    public class FileCompressionRatio
    {
        public static double GetCompressionRatio(string filePath1, string filePath2)
        {
            FileInfo fileInfo1 = new FileInfo(filePath1);
            FileInfo fileInfo2 = new FileInfo(filePath2);

            long size1 = fileInfo1.Length;
            long size2 = fileInfo2.Length;

            if (size1 == 0 || size2 == 0)
            {
                throw new ArgumentException("File size cannot be zero.");
            }

            double ratio = (double)size1 / size2;

            return ratio;
        }
    }
}
