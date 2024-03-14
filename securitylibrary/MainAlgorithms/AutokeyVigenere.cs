using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            char[] a = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            string key = "";
            string txt = "";

            for (int i = 0; i < cipherText.Length; i++)
            {
                int x = ((Array.IndexOf(a, cipherText[i]) - Array.IndexOf(a, plainText[i])) + 26) % 26;
                key = key + a[x];
            }

            txt += key[0];

            for (int i = 1; i < key.Length; i++)
            {
                if (plainText == Decrypt(cipherText, txt))
                {
                    return txt;
                }
                txt += key[i];
            }

            key = key.ToLower();
            return key;
        }

        public string Decrypt(string cipherText, string key)
        {
            string plainText = "";
            char[] arr = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            string key_stem = key;
            char[,] matrix = new char[26, 26];
            cipherText = cipherText.ToLower();
            string b = "";

            int x = 0;

            for (int j = 0; j < plainText.Length; j++)
            {
                for (int i = 0; i < cipherText.Length; i++)
                {
                    if (arr[i] == cipherText[i] && arr[j] == plainText[j])
                    {
                        x = (i - j + 26) % 26;
                    }
                    key += arr[x];
                }
            }

            for (int j = 0; j < 26; j++)
            {
                for (int k = 0; k < 26; k++)
                {
                    matrix[j, k] = arr[(k + j) % 26];
                }
            }
            int bb = 0;
            for (int k = 0; k < cipherText.Length; k++)
            {
                for (int i = 0; i < 26; i++)
                {
                    for (int j = 0; j < 26; j++)
                    {

                        if (key_stem[k] == matrix[i, 0] && cipherText[k] == matrix[i, j])
                        {
                            key_stem += matrix[0, j];
                            plainText += matrix[0, j];
                        }
                    }
                }
            }

            return plainText;


        }

        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToLower();
            key = key.ToLower();
            string ciphertxt = "";
            int keyIdx = 0;

            for (int i = 0; i < plainText.Length; i++)
            {
                if (char.IsLetter(plainText[i]))
                {
                    int sub = plainText.Length - key.Length;
                    for (int j = 0; j < sub; j++)
                    {
                        key += plainText[j];
                    }
                    int numericaltxt = plainText[i] - 'a';
                    int numericalkey = key[keyIdx % key.Length] - 'a';
                    int encryptedValue = (numericaltxt + numericalkey) % 26;
                    char encryptedtxt = (char)(encryptedValue + 'a');
                    ciphertxt += encryptedtxt;

                    keyIdx++;
                }
                else
                {
                    ciphertxt += plainText[i];
                }
            }

            return ciphertxt;
        }
    }
}
