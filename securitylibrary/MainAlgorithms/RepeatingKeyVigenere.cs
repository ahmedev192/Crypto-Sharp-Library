using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            string key = FindKey(plainText, cipherText);

            if (!string.IsNullOrEmpty(key))
            {
                return key;
            }

            return "Key not found";
        }

        private string FindKey(string plainText, string cipherText)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            int cipherLength = cipherText.Length;
            string key = "";

            for (int i = 0; i < cipherLength; i++)
            {
                int keyIndex = ((alphabet.IndexOf(cipherText[i]) - alphabet.IndexOf(plainText[i])) + 26) % 26;
                key += alphabet[keyIndex];
            }

            return TryMatchKey(plainText, cipherText, key);
        }

        private string TryMatchKey(string plainText, string cipherText, string key)
        {
            string temp = key[0].ToString();
            int keyLength = key.Length;

            for (int i = 1; i < keyLength; i++)
            {
                if (cipherText.Equals(Encrypt(plainText, temp)))
                {
                    return temp;
                }

                temp += key[i];
            }

            return key;
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            string plaintxt = "";
            int keyIdx = 0;

            for (int i = 0; i < cipherText.Length; i++)
            {
                if (char.IsLetter(cipherText[i]))
                {
                    int numericalciphertxt = cipherText[i] - 'a';
                    int numericalkey = key[keyIdx % key.Length] - 'a';
                    int decryptedvalue = (numericalciphertxt - numericalkey + 26) % 26;
                    char decryptedtxt = (char)(decryptedvalue + 'a');
                    plaintxt += decryptedtxt;

                    keyIdx++;
                }
                else
                {
                    plaintxt += cipherText[i];
                }
            }

            return plaintxt;
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