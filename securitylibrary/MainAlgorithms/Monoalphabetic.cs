using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            char[] arr = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            //char[] arr = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            char[] key = new char[26];

            string s1 = plainText.ToLower();
            string s2 = cipherText.ToLower();

            for (int i = 0; i < plainText.Length; i++)
            {
                for (int k = 0; k < 26; k++)
                {
                    if (s1[i] == arr[k])
                    {
                        key[k] = s2[i];
                    }
                }
            }

            for (int f = 0; f < key.Length; f++)
            {
                if (key[f] == '\0')
                {
                    for (int d = 0; d < 26; d++)
                    {
                        if (key.Contains(arr[d]))
                        {
                            continue;
                        }
                        else
                            key[f] = arr[d];
                    }
                }
            }
            //string v = new string(key.Distinct().ToArray());
            string l = new string(key);
            return l.ToLower();


        }

        public string Decrypt(string cipherText, string key)
        {
            char[] arr = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            string plainText = "";
            string w = key.ToUpper();
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (cipherText[i] == w[j])
                    {
                        plainText += arr[j];
                    }
                }
            }
            string s = plainText.ToLower();
            return s;

        }

        public string Encrypt(string plainText, string key)
        {
            char[] arr = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            string cipherText = "";
            string p = plainText.ToUpper();
            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (p[i] == arr[j])
                    {
                        cipherText += key[j];
                    }
                }
            }
            return cipherText;
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            char[] arr1 = { 'e', 't', 'a', 'o', 'i', 'n', 's', 'r', 'h', 'l', 'd', 'c', 'u', 'm', 'f', 'p', 'g', 'w', 'y', 'b', 'v', 'k', 'x', 'j', 'q', 'z' };
            char[] planText = new char[cipher.Length];
            string s1 = cipher.ToLower();
            int[] count = new int[s1.Length];
            Dictionary<char, int> dict = new Dictionary<char, int>();
            for (int i = 0; i < s1.Length; i++)
            {
                if (dict.ContainsKey(s1[i]) == false)
                {
                    dict.Add(s1[i], 1);
                }
                else
                {
                    dict[s1[i]]++;
                }
            }
            var sortedDict = from entry in dict orderby entry.Value descending select entry.Key;

            for (int k = 0; k < s1.Length; k++)
            {
                int index = Array.IndexOf(sortedDict.ToArray(), s1[k]);
                planText[k] = arr1[index];
            }
            return new string(planText);
        }
    }
}
