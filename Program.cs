using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lz77___ac
{
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
    public class Program
    {
        public static void Main()
        {
            string text = "enwik7.txt";///"enwik7.txt"
            string line;
            string wholeText = "";
            string reverse_text = "reverse_text.txt";
            using (StreamReader reader = new StreamReader(text, Encoding.UTF8))
            {
                using (StreamWriter writer = new StreamWriter(reverse_text))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        wholeText = line + 'ؐ';//
                        int[] p;
                        char[] symb;
                        int j;
                        string result;
                        int chunkSize = 10;
                        List<object> list_Res = lz77(wholeText, 5);
                        result = listToStr(list_Res);
                        for (j = 0; j < result.Length; j += chunkSize)
                        {
                            string chunk = result.Substring(j, Math.Min(chunkSize, result.Length - j));
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
                }
            }

            double re = FileCompressionRatio.GetCompressionRatio(reverse_text, text);

            Console.WriteLine(re);
            Console.WriteLine("Conversion in file");
            Console.ReadKey();

        }

       
        public struct Node
        {
            public int offset; // отступ влево
            public int length; // длина подстроки
            public char next; // следующий символ
            public Node(int offset, int length, char next)
            {
                this.offset = offset;
                this.length = length;
                this.next = next;
            }
            public void PrintNodeData() // для вывода
            {
                Console.WriteLine($"Offset: {offset}, Length: {length}, Next: {next}");
            }
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
        static List<object> lz77(string data, int buff_size)
        {
            List<object> triplet_list = new List<object>(); //список в котором и одиночные символы и ноды
            string buf = ""; //буфер 
            int pos = 0;
            int offset = 0;
            int lengthNode = 0;
            char nextChar = '\0';
            while (pos < data.Length)//смотрим все элементы строки 
            {
                int max = Math.Max(pos - buff_size, 0);
                buf = data.Substring(max, pos - max); //вычисляем буфер 
                int index_substr = buf.Length;
                int length = 0;
                for (int l = 1; l < buf.Length + 1; l++)
                {
                    string k = data.Substring(pos, l); //отделяем от строки 
                    if ((buf.LastIndexOf(data.Substring(pos, l)) == -1)) //ищем эту строку в буфере
                    {
                        break;
                    }
                    else
                    {
                        index_substr = buf.LastIndexOf(data.Substring(pos, l)); //если нашли то запоминаем и пытаемся найти строку на элемент больше
                        length = l;
                    }
                }
                offset = buf.Length - index_substr; //сохраняем сдвиг
                lengthNode = length; //длина подстроки
                int count = 0;
                int pos_new = pos;
                int lengthNode_new = length;
                if (length != 0)
                {
                    while (data[pos_new] == data[pos_new + length]) //проверяем повторки, если они есть записываем как можно длиннее
                    {
                        pos_new++;
                        lengthNode_new++;
                        count++;
                    }
                    if (count > 1) // если повторка 1 то это не повтор
                    {
                        pos = pos_new;
                        lengthNode = lengthNode_new;
                    }
                }
                nextChar = data[pos + length]; // записывает следующий символ

                pos += length;
                pos++;
                zeroDel(offset, lengthNode, nextChar, triplet_list); //удаляем 0, если новая буква

            }
            return triplet_list; // возвращаем список нодд и букв
        }

        static public bool IsChar(object obj) // проверяем нода или буква
        {
            return obj is char;
        }
        static public void zeroDel(int offset, int lengthNode, char nextChar, List<object> triplet_list) // удаляем нули чтобы их не хранить
        {
            if (offset == 0 && lengthNode == 0)
            {
                triplet_list.Add(nextChar);
            }
            else
            {
                Node triplet = new Node(offset, lengthNode, nextChar);
                triplet_list.Add(triplet);
            }
        }

        static public string listToStr(List<object> res_list)
        {
            string res = "";
            foreach (object list in res_list)
            {
                if (IsChar(list))
                {
                    res += list.ToString();
                }
                else
                {
                    Node node = (Node)list;
                    res += node.offset;
                    res += node.length;
                    res += node.next;
                }
            }
            return res;
        }
    }
}
