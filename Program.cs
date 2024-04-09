using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;

class Program
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
    static void Main()
    {
        string text = "rusTolstoy.txt"; // файл с изображением 
        string result = "convertRes.txt"; //сжатие чб
        string revers = "output_new.txt"; //разжатие чб

        if (!File.Exists(text)) // проверка существования
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        using (StreamReader reader = new StreamReader(text, Encoding.UTF8)) //чтение построчно
        {
            using (StreamWriter res = new StreamWriter(result))
            {
                string line;
                string hlp = "";
                char currentChar = '0';
                while ((line = reader.ReadLine()) != null) // пптриааа
                {
                    int count = 1;
                    int index = 0;
                    
                    while (index < (line.Length - 2))
                    {
                        currentChar = line[index];
                        if (currentChar == line[index + 1])
                        {
                            count++;
                            index++;
                        }
                        else
                        {
                            
                            if (count == 1)
                            {
                                index++;
                                hlp += line[index - 1];
                                while (line[index] != line[index-1] && index != line.Length -1)
                                {
                                    hlp += line[index];
                                    index++;
                                }
                                hlp = hlp.Substring(0, hlp.Length - 1);
                                res.Write('&');
                                res.Write(hlp);
                                res.Write('&');
                                hlp = "";
                                index--;
                            }
                            else
                            {
                                res.Write((char)(count));
                                res.Write(currentChar);
                                count = 1;
                                index++;
                            }
                        }

                    }
                    
                    res.Write((char)(count));
                    res.Write(currentChar);
                    res.Write('\n');
                }
            }
        }

        //using (StreamReader reader = new StreamReader(result)) //чтение построчно
        //{
        //    using (StreamWriter togrImg = new StreamWriter(revers))
        //    {
        //        string line;
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            int i = 0;
        //            while (i < line.Length - 1)
        //            {
        //                if (line[i] == '&')
        //                {
        //                    i++;
        //                    while (line[i] != '&')
        //                    {
        //                        togrImg.Write(line[i]);
        //                        i++;
        //                    }
        //                    i++;
        //                }
        //                else
        //                {
        //                    int count = (int)(line[i]);
        //                    char charecter = line[i + 1];

        //                    for (int j = 0; j < count; j++)
        //                    {
        //                        togrImg.Write(charecter);
        //                        i++;
        //                    }
        //                }
                        

        //            }
        //            togrImg.Write('\n');
        //        }
        //    }
        //}
        double ratio = 0;
        ratio = FileCompressionRatio.GetCompressionRatio(result, text);
        Console.WriteLine(ratio);
        Console.ReadKey();
    }
}
