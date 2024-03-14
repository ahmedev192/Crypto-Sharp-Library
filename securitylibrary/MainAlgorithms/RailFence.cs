using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {

            cipherText = cipherText.ToLower();
            for (int i = 1; i < plainText.Length; i++)
            {
                if (Encrypt(plainText, i).ToLower().CompareTo(cipherText) == 0)
                {
                    return i;
                }
            }
            return -1;
        }
        public string Decrypt(string cipherText, int key)
        {
            string cipher = "";
            double s = (double)cipherText.Length / key;
            bool w = unchecked(s == (int)s);
            double q = 0;
            if (w)
            {
                q = s;
            }
            else
            {
                q = s + 1;
            }
            int f = (int)q;
            char[,] matrix = new char[key, f];
            int count = 0;
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < f; j++)
                {
                    if (count < cipherText.Length)
                    {
                        matrix[i, j] = cipherText[count];
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            int a = 0;
            for (int i = 0; i < f; i++)
            {
                for (int j = 0; j < key; j++)
                {

                    cipher += matrix[j, i];
                    a++;

                }
            }
            return cipher;

            //int textLength = cipherText.Length;
            //int[] fence = new int[textLength]; // Array to hold the fence indices
            //int i = 0;

            //// Build the fence indices
            //for (int j = 0; j < textLength; j++)
            //{
            //    fence[j] = i;

            //    // Change direction when reaching the top or bottom rail
            //    if (i == 0)
            //    {
            //        i = 1;
            //    }
            //    else if (i == key - 1)
            //    {
            //        i = key - 2;
            //    }
            //    else
            //    {
            //        i += (j % 2 == 0) ? -2 : 2; // Alternate between going up and down
            //    }
            //}

            //// Reorder the cipher text based on the fence indices
            //char[] plainChars = new char[textLength];
            //for (int j = 0; j < textLength; j++)
            //{
            //    plainChars[fence[j]] = cipherText[j];
            //}

            //return new string(plainChars).ToLower();

        }

        public string Encrypt(string plainText, int key)
        {

            string plan_text = plainText;
            string cipher = "";

            double s = plan_text.Length / key;
            double q = 0;
            bool w = unchecked(s == (int)s);
            // string p = "";
            if (!w)
            {
                q = s;
            }
            else
            {
                q = s + 1;
            }
            Console.WriteLine(s);
            char[,] matrix = new char[key, ((int)q)];
            int count = 0;
            for (int i = 0; i < q; i++)
            {
                for (int j = 0; j < key; j++)
                {
                    if (count < plan_text.Length)
                    {
                        matrix[j, i] = plan_text[count];
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            int a = 0;
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < q; j++)
                {

                    cipher += matrix[i, j];
                    a++;

                }
            }
            return cipher;
        }
    }
}
