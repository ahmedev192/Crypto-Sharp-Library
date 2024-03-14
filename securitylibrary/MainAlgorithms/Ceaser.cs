using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            plainText = plainText.ToUpper();
            string CYPHERTEXT = "";
            //loop on each character of the plaintext to convert every letter to ascii 
            for (int i = 0; i < plainText.Length; i++)
            {
                char ch = plainText[i];
                if (ch >= 'A' && ch <= 'Z')
                {
                    int numOfChar = ch - 'A';
                    int ASCII = ((numOfChar + key) % 26);
                    char encryptedCH = (char)(ASCII + 'A');
                    CYPHERTEXT += encryptedCH;
                }
                else
                {
                    //if the letter is number
                    CYPHERTEXT += ch;
                }
            }
            return CYPHERTEXT;
            //  throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, int key)
        {
            cipherText.ToUpper();
            string PLAINTEXT = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                char ch = cipherText[i];
                if (ch >= 'A' && ch <= 'Z')
                {
                    int numOfChar = ch - 'A';
                    int ASCII = ((numOfChar - key + 26) % 26);
                    char decryptedCH = (char)(ASCII + 'A');
                    PLAINTEXT += decryptedCH;
                }
                else
                {
                    PLAINTEXT += ch;
                }
            }
            return PLAINTEXT;
            // throw new NotImplementedException();
        }

        public int Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();
            int key = cipherText[0] - plainText[0];
            if (key < 0)
            {
                key += 26;
            }
            return key;
            //  throw new NotImplementedException();
        }
    }
}
