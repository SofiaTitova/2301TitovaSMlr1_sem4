using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace burrows_wheeler_transform
{
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
    internal class Program
    {

        public static int part = 50;
        static void Main(string[] args)
        {
            string inputStr = "абракадабра";
            int index = 0;
            string result;
            Console.WriteLine("Изначальная строка: {0}", inputStr);
            BWT(out result, inputStr, ref index); 
            Console.WriteLine("Строка после преобразования: {0}", result);
            Console.WriteLine("Индекс строки: {0}", index);
            string res = rBWT(result, index);
            Console.WriteLine("Обратное преобразование: {0}", res);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine();
            string text = "text.txt";
            string textRes = "textRes.txt";
            string chb = "chb.txt";
            string chbRes = "chbRes.txt";
            string grey = "grey.txt";
            string greyRes = "greyRes.txt";
            bigBWT(text, textRes);
            bigBWT(chb, chbRes);
            bigBWT(grey, greyRes);
            using (StreamReader reader = new StreamReader(text, Encoding.UTF8)) 
            {
                Console.WriteLine("текст до BWT: {0}", strLen(reader));
            }
            using (StreamReader reader = new StreamReader(textRes, Encoding.UTF8)) 
            {
                Console.WriteLine("текст после BWT: {0}", strLen(reader));
            }
            Console.WriteLine();
            using (StreamReader reader = new StreamReader(chb, Encoding.UTF8)) 
            {
                Console.WriteLine("чб до BWT: {0}", strLen(reader));
            }
            using (StreamReader reader = new StreamReader(chbRes, Encoding.UTF8)) 
            {
                Console.WriteLine("чб после BWT: {0}", strLen(reader));
            }
            Console.WriteLine();
            using (StreamReader reader = new StreamReader(grey, Encoding.UTF8)) 
            {
                Console.WriteLine("оттенки серого до BWT: {0}", strLen(reader));
            }
            using (StreamReader reader = new StreamReader(greyRes, Encoding.UTF8)) 
            {
                Console.WriteLine("оттенки серого после BWT: {0}", strLen(reader));
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
                for (int i = 0; i < line.Length - 1; i++)
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
            lengthStr = (k - 2 * part) / len;
            return lengthStr;
        }
        static void BWT(out string result, string inStr, ref int ind)
        {
            result = "";
            string[] bwt_matrix = new string [inStr.Length] ;
            string toAdd;
            string mainPart;
            string newStr = inStr;
            string part;
            int len = newStr.Length;
            for (int i = 0; i < len; i++)
            {
                toAdd = inStr.Substring(0, 1);
                mainPart = inStr.Substring(1, len - 1);
                inStr = mainPart + toAdd;
                bwt_matrix[i] = inStr;

            }
            string[] sorted_bwt = bwt_matrix.OrderBy(word => word).ToArray();
            for (int i = 0; i < len; i++)
            {
                if (sorted_bwt[i] == newStr)
                {
                    ind = i;
                }
            }
            for (int i = 0; i < len; i++)
            {
                part = sorted_bwt[i].Substring(len - 1);
                result += part;
            }

        }
        static string rBWT(string result, int ind)
        {
            int len = result.Length;
            string[] bwt_matrix = new string[len];
            for (int i = 0; i < len; i++)
            {
                bwt_matrix[i] = string.Empty; 
            }
            for (int j = 0; j < len - 1; j++)
            {
                for (int i = 0; i < len; i++)
                {
                    bwt_matrix[i] = result[i] + bwt_matrix[i];
                }
                bwt_matrix = bwt_matrix.OrderBy(bwt => bwt).ToArray();
            }
            for (int i = 0; i < len; i++)
            {
                bwt_matrix[i] = bwt_matrix[i] + result[i];
            }
            return bwt_matrix[ind];
        }

        static void bigBWT(string inp, string outp)
        {
            string line;
            using (StreamReader reader = new StreamReader(inp, Encoding.UTF8))  
            using (StreamWriter writer = new StreamWriter(outp))                 
            {
                while ((line = reader.ReadLine()) != null)
                {
                    int len = line.Length;          
                    int k = 0;                    

                    for (int i = len; i > 0; i -= part){
                        string textPart;           
                        int partLen;      

                        if (i - part < 0) 
                        {
                            textPart = line.Substring(part * k);                         
                            partLen = line.Length - part * k;   
                        }
                        else
                        {
                            textPart = line.Substring(part * k, part);       
                            k++;
                            partLen = part;                                
                        }
                        string result;
                        int ind = 0;
                        BWT(out result, textPart, ref ind);
                        writer.Write(result);           
                        writer.Write((char)(ind + part));     

                    }
                    writer.Write("\n");  
                }
            }
        }

    }
}
