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
            throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            string encryptedText = "";

            for (int i = 0; i < plainText.Length; i++)
            {
                int plainCharIndex = alphabet.IndexOf(plainText[i]);
                int keyCharIndex = alphabet.IndexOf(key[i % key.Length]);
                int encryptedCharIndex = (plainCharIndex + keyCharIndex) % 26;
                encryptedText += alphabet[encryptedCharIndex];
            }

            return encryptedText;
        }
    }
}