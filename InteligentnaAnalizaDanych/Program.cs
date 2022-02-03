using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Porter2Stemmer;

namespace InteligentnaAnalizaDanych
{
    public class WordToFind
    {
        public int FirstId { get; set; }
        public int SecondId { get; set; }
        public string Locations { get; set; }

        public WordToFind(int firstId, int secondId, string locations)
        {
            this.FirstId = firstId;
            this.SecondId = secondId;
            this.Locations = locations;

        }


    }
    public static class Excel
    {

        public static void ConvertToExcel(int a, int b, int c)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            if (excelApp != null)
            {
                Microsoft.Office.Interop.Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
                Microsoft.Office.Interop.Excel.Worksheet excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelWorkbook.Sheets.Add();
                excelWorksheet.Cells[1, 1] = "K-nn";
                excelWorksheet.Cells[2, 1] = "Trafione";
                excelWorksheet.Cells[3, 1] = "Pudła";
                excelWorksheet.Cells[4, 1] = "Suma";

                excelWorksheet.Cells[1, 2] = a;
                excelWorksheet.Cells[2, 2] = b;
                excelWorksheet.Cells[3, 2] = c;
                excelWorksheet.Cells[4, 2] = b + c;



                excelApp.ActiveWorkbook.SaveAs(@"C:\Users\Artem\OneDrive\Рабочий стол\TEST.xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault);

                excelWorkbook.Close();
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorksheet);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorkbook);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelApp);


            }
        }
    }



    class Program
    {
        private static string semtext;

        static void Main(string[] args)
        {
            string folderPath = @"C:\Users\Artem\OneDrive\Рабочий стол\учеба\InteligentnaAnalizaDanych\reuters21578";
            List<MyArticle> Articles = new List<MyArticle>();
            EnglishPorter2Stemmer englishPorter2Stemmer = new EnglishPorter2Stemmer();


            int k = 11;

            foreach (string file in Directory.EnumerateFiles(folderPath, "*.sgm"))
            {



                // Console.WriteLine(file);
                string xmlString = File.ReadAllText(file);
                xmlString = xmlString.Replace("<!DOCTYPE lewis SYSTEM \"lewis.dtd\">", string.Empty);
                xmlString = "<Articles>" + xmlString + "</Articles>";
                xmlString = ReplaceHexadecimalSymbols(xmlString);
                // xmlString = Regex.Replace(xmlString, "&#.*?;", string.Empty);
                XmlSerializer serializer = new XmlSerializer(typeof(Articles), new XmlRootAttribute("Articles"));
                StringReader stringReader = new StringReader(xmlString);
                Articles articles = (Articles)serializer.Deserialize(stringReader);

                foreach (ArticlesREUTERS articlesREUTERS in articles.REUTERS)
                {
                    if (articlesREUTERS.PLACES.Length == 1
                       && CheckAnalizedCountry(articlesREUTERS.PLACES[0])
                       && articlesREUTERS.TEXT.ODY != null)
                    {
                        string localBody = articlesREUTERS.TEXT.ODY;

                        List<StemmedWord> localStemmedWords = new List<StemmedWord>();
                        foreach (string word in localBody.Split(','))
                        {
                            StemmedWord stemmedWord = englishPorter2Stemmer.Stem(word);
                            localStemmedWords.Add(stemmedWord);
                            semtext += stemmedWord.Value + " ";
                        }
                        MyArticle article = new MyArticle(articlesREUTERS.PLACES.FirstOrDefault(), articlesREUTERS.TEXT.ODY, localStemmedWords, semtext, articlesREUTERS.NEWID);
                        semtext = "";
                        Articles.Add(article);

                        article.vector[0] = article.C1();
                        article.vector[1] = article.C2();
                        article.vector[2] = article.C3();
                        article.vector[3] = article.C4();
                        article.vector[4] = article.C5();
                        article.vector[5] = article.C6();
                        article.vector[6] = article.C7();
                        article.vector[7] = article.C8();
                        Console.WriteLine(article.vector[0]);
                    }
                }

                double M1 = 0;
                double M2 = 0;
                double M3 = 0;


                int firstPart = Articles.Count / 2;


                int struck = 0;
                int joint = 0;
                double suma = 0;






                double Euklides(int[] a, int[] b)
                {
                    double x = 0;
                    for (int i = 0; i < a.Length; i++)
                    {
                        x += Math.Pow((b[i] - a[i]), 2);

                    }
                    return Math.Sqrt(x);
                }




                double Manhattan(int[] a1, int[] b1)
                {
                    double x = 0;
                    for (int i = 0; i < a1.Length; i++)
                    {
                        for (int j = 0; j < b1.Length; j++)
                        {
                            {
                                return Math.Abs(a1[i] - b1[j]);
                            }
                        }
                    }
                    return x;
                }




                double Chebyshev(int[] a1, int[] b1)
                {
                    double x = 0;
                    for (int i = 0; i < a1.Length; i++)
                    {
                        for (int j = 0; j < b1.Length; j++)
                        {
                            {
                                double ax = Math.Abs(a1[i] - b1[j]);
                                return (ax) - Math.Sin(ax);

                            }
                        }
                    }
                    return x;
                }




                Dictionary<WordToFind, double> DICTIONARY = new Dictionary<WordToFind, double>();
                for (int index = 0; index < firstPart; index++)
                {

                    for (int i = firstPart; i < Articles.Count; i++)
                    {
                        M1 = Euklides(Articles[index].vector, Articles[i].vector);
                        M2 = Manhattan(Articles[index].vector, Articles[i].vector);
                        M3 = Chebyshev(Articles[index].vector, Articles[i].vector);
                        suma = M1;
                        DICTIONARY.Add(new WordToFind(Articles[index].ID, Articles[i].ID, Articles[i].Location), Math.Round(M1, 2));
                        DICTIONARY.Add(new WordToFind(Articles[index].ID, Articles[i].ID, Articles[i].Location), Math.Round(M2, 2));
                        DICTIONARY.Add(new WordToFind(Articles[index].ID, Articles[i].ID, Articles[i].Location), Math.Round(M3, 2));

                    }



                    List<KeyValuePair<WordToFind, double>> kmembers = DICTIONARY.OrderBy(key => key.Value).Take(k).ToList();


                    if (kmembers.OrderByDescending(articlesREUTERS => articlesREUTERS.Value).First().Key.Locations == Articles[index].Location)
                    {
                        struck++;
                    }
                    else
                    {
                        joint++;
                    }





                    Console.WriteLine("Tekst analizowany należy do kraju: " + Articles[index].Location);

                    Console.WriteLine("USA : " + kmembers.Where(articlesREUTERS => articlesREUTERS.Key.Locations == "usa").Count());
                    Console.WriteLine("France: " + kmembers.Where(articlesREUTERS => articlesREUTERS.Key.Locations == "France").Count());
                    Console.WriteLine("Japan: " + kmembers.Where(articlesREUTERS => articlesREUTERS.Key.Locations == "Japan").Count());
                    Console.WriteLine("Canada: " + kmembers.Where(articlesREUTERS => articlesREUTERS.Key.Locations == "canada").Count());
                    Console.WriteLine("west-germany: " + kmembers.Where(articlesREUTERS => articlesREUTERS.Key.Locations == "west-germany").Count());
                    Console.WriteLine("uk: " + kmembers.Where(articlesREUTERS => articlesREUTERS.Key.Locations == "uk").Count());

                    Console.WriteLine("Przeanalizowano tekst " + (index + 1));


                    //Console.WriteLine("Trafiłem: " + trafiony + "Spudłowałem: " + pudlo);



                }



                Console.WriteLine("Trafione: " + struck + "Spudłowane: " + joint);

                Excel.ConvertToExcel(k, struck, joint);

            }

            Console.WriteLine(Articles.Count);

            PrintCountryCount(Articles, "west-germany");
            PrintCountryCount(Articles, "usa");
            PrintCountryCount(Articles, "france");
            PrintCountryCount(Articles, "uk");
            PrintCountryCount(Articles, "canada");
            PrintCountryCount(Articles, "japan");

            Console.ReadKey();
        }
        static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }


        static bool CheckAnalizedCountry(string countryName)
        {
            return countryName == "west-germany"
                || countryName == "usa"
                || countryName == "france"
                || countryName == "uk"
                || countryName == "canada"
                || countryName == "japan";
        }
        public static void PrintCountryCount(List<MyArticle> allArticles, string countryName)
        {
            Console.WriteLine(countryName + ":" + allArticles.Where(item => item.Location == countryName).Count());

        }

    }

}
