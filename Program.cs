using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace lz77
{
    internal class Program
    {
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
            //public void PrintNodeData() // для вывода
            //{
            //    Console.WriteLine($"Offset: {offset}, Length: {length}, Next: {next}");
            //}
            public string print()
            {
                string res = offset.ToString() + length.ToString() + next.ToString();
                return res;
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
        static void Main(string[] args)
        {
            /* string data = "abacabacabadaca$";
             Console.WriteLine("Начальная строка: {0}", data);
             Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
             int buff_size = 5;
             List<object> res_list = new List<object>();
             res_list = lz77(data, buff_size);
             foreach (object list in res_list) //это просто для вывода, если одиночный символ то просто выводим, а еслм нода, то выводим все поля
             {
                 if (IsChar(list))
                 {
                     Console.WriteLine(list.ToString());
                 }
                 else
                 {
                     Node node = (Node)list;
                     node.PrintNodeData();
                 } 
             }
             string res = "";
             res = decoder_lz77(res_list, buff_size); //обратное преобразование
             Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
             Console.Write("Преобразованная строка: ");
             Console.WriteLine(res);
             Console.Write("Получилось ли перобразование ");
             Console.WriteLine(res == data); //проверяем что правильно преобразует*/
             string text = "rusTolstoy.txt";
             string line;
             string data = "";
             using (StreamReader reader = new StreamReader(text, Encoding.UTF8))
             {
                 while ((line = reader.ReadLine()) != null)
                 {
                     data += line + " ";
                 }
             }
             data += "$";

             int buff_size = 3000;
            List<object> res_list = new List<object>();
            res_list = lz77(data, buff_size);
            string reverse_text = "reverse_text.txt";
            using (StreamWriter writer = new StreamWriter(reverse_text, false, Encoding.UTF8))
            {
                foreach (object list in res_list) //это просто для вывода, если одиночный символ то просто выводим, а еслм нода, то выводим все поля
                {
                    if (IsChar(list))
                    {
                        writer.Write(list.ToString());
                    }
                    else
                    {
                        Node node = (Node)list;
                        string tmp = node.print();
                        writer.Write(tmp);
                    }
                }

            }


            double ratio = 0;
            ratio = FileCompressionRatio.GetCompressionRatio(reverse_text, text);
            Console.WriteLine(ratio);
            File.WriteAllText(reverse_text, string.Empty);
            buff_size += 10000;
            Console.ReadKey();
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
                for (int l = 1; l < buf.Length+1; l++)
                {
                    string k = data.Substring(pos, l); //отделяем от строки 
                    if ((buf.LastIndexOf(data.Substring(pos,l)) == -1)) //ищем эту строку в буфере
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
        static string decoder_lz77(List<object> triplet_list, int buf_len)
        {
            string data = "";
            bool flag;
            int offset = 0;
            int lengthNode = 0;
            char nextChar = '\0';
            foreach (object triplet in triplet_list) { 
                flag = IsChar(triplet); // проверяем нода или буква
                if (flag)
                {
                    offset = 0;
                    lengthNode = 0;
                    nextChar = (char)triplet;
                }
                else // если нода
                {
                    Node node = (Node)triplet;
                    offset = node.offset; //сохраняем поля в переменные
                    lengthNode = node.length;
                    nextChar = node.next;
                }

                int tmp = 0;
                int new_lengthNode = lengthNode;
                if (lengthNode > offset) // если так получилось, значит мы идем по одному и тому же участку буфера несколько раз ( то есть были повторки )
                {
                    tmp = offset; // максимальный отступ который мы можем сначала получить это возвращение влево

                    int tmp_len = data.Length;
                    while (tmp > 0)
                    {

                        data = data + data.Substring(tmp_len - offset, tmp); // то есть мы проходимся по буфферу, пока не наберем столько символов сколько в сдвиге влево 
                        tmp = new_lengthNode - tmp;
                        new_lengthNode = tmp;
                        if (tmp > offset) //это позволяет переходить в начало буффера
                        {
                            tmp = offset;
                        }
                    }
                    
                    data = data + nextChar; // добавляем следующий элемент
                }
                else
                { 
                    data = data + data.Substring(data.Length - offset, lengthNode) + nextChar; // собираем строку
                }
                
            }
            return data;
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
    }
}
