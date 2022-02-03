using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Porter2Stemmer;

namespace InteligentnaAnalizaDanych
{
    public class MyArticle
    {

        public string Location { get; private set; }
        public string OrginalText { get; private set; }
        public int ID { get; set; }

        public string DesiredText { get; set; }
        public List<MyArticle> Articles { get; }
        public List<StemmedWord> OtherText { get; private set; }

        public int[] vector = new int[10];

        public MyArticle(string location, string text, List<StemmedWord> otherText, string desiredText, int id)
        {
            OrginalText = text;
            OtherText = otherText;
            DesiredText = desiredText;
            ID = id;
            Location = location;
        }

        public MyArticle(List<MyArticle> articles)
        {
            Articles = articles;
        }

        //CECHY
        public int C1() { return OrginalText.Length; }

        public int C2() { return OtherText.Count(); }

        public int C3()
        {
            string common = "a";
            string input = DesiredText;
            int sum = (from ch in input where common.Contains(ch) select ch).Count();
            return sum;
        }
        public int C4()
        {
            string common = "x";
            string input = DesiredText;
            int sum = (from ch in input where common.Contains(ch) select ch).Count();
            return sum;
        }
        public int C5()
        {
            string character;
            int sum = 0;
            foreach (var item in OtherText)
            {
                character = item.Unstemmed;
                for (int i = 0; i < character.Length; i++)
                {
                    if (char.IsUpper(character[i])) { sum++; }
                }
            }
            return sum;
        }
        public int C6()
        {
            int europe = 0;
            int asia = 0;
            int namerica = 0;
            int samerica = 0;
            int africa = 0;
            int australia = 0;
            int antarctica = 0;
            int sum = 0;
            foreach (var item in OtherText)
            {
                if (item.Value == "europe") { europe++; }
                else if (item.Value == "asia") { asia++; }
                else if (item.Value == "north-america") { namerica++; }
                else if (item.Value == "south-america") { samerica++; }
                else if (item.Value == "africa") { africa++; }
                else if (item.Value == "australia") { australia++; }
                else if (item.Value == "antarctica") { antarctica++; }
            }
            sum = europe + asia + africa + namerica + samerica + australia + antarctica;
            return sum;
        }
        public int C7()
        {
            int sum = 0;
            foreach (var item in OtherText)
            {
                string a = item.Value;
                if (Int32.TryParse(a, out int c))
                {
                    sum++;
                }
            }
            return sum;
        }
        public int C8() { return DesiredText.Length; }
    }
}
