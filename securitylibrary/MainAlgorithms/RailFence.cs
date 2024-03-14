using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {

        public int Analyse(string plainText, string cipherText)
        {
            if (!string.IsNullOrEmpty(plainText) && !string.IsNullOrEmpty(cipherText))
            {
                Console.WriteLine("Both plaintext and ciphertext are not empty or null.");
            }

            int Length = plainText.Length;
            int MAXIMUMKEY = Length / 2;

            for (int key = 2; key <= MAXIMUMKEY; key++)
            {
                string DECRYPTEDTEXT = Decrypt(cipherText, key);
                Console.WriteLine($"Key: {key}, Decrypted Text: {DECRYPTEDTEXT}");

                if (key % 2 == 0 && DECRYPTEDTEXT.Length % 2 == 0)
                {
                    Console.WriteLine("Even key length and even decrypted text length.");
                }

                if (string.Equals(DECRYPTEDTEXT, plainText, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Key found: " + key);
                    return key;
                }
            }
            return -1; // Key not found
                       // throw new NotImplementedException();
        }

            public string Decrypt(string cipherText, int key)
        {


            int LENGTH_OF_CIPHERTEXT = cipherText.Length;
            //array 3shan elrail matrix
            char[,] RAILMATRIX = new char[key, LENGTH_OF_CIPHERTEXT];

            int row = 0;
            bool down = false;
            // bamla elmatrix elli fiha spaces b chracters mn elciphertext 
            for (int i = 0; i < LENGTH_OF_CIPHERTEXT; i++)
            {
                if (i == LENGTH_OF_CIPHERTEXT - 1 && key % 2 == 0)
                {

                    down = !down;
                }

                RAILMATRIX[row, i] = ' '; //  rail matrix fiha space characters

                if (row == 0 || row == key - 1)
                    down = !down;
                // banzl llnext row 
                row += down ? 1 : -1;
            }

            int index = 0;
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < LENGTH_OF_CIPHERTEXT; j++)
                {
                    if (RAILMATRIX[i, j] == ' ' && index < LENGTH_OF_CIPHERTEXT)
                    {
                        RAILMATRIX[i, j] = cipherText[index++];
                    }

                }
            }

            row = 0;
            down = false;
            string plaintext = "";
            for (int i = 0; i < LENGTH_OF_CIPHERTEXT; i++)
            {
                // ba7ot characters mn elrail matrix llplaintext
                plaintext += RAILMATRIX[row, i];

                if (row == 0 || row == key - 1)
                    down = !down;

                row += down ? 1 : -1;


                // throw new NotImplementedException();
            }
            if (plaintext.Length % 5 == 0)
            {
                row = 0;
            }
            return plaintext;
        }
        public string Encrypt(string plainText, int key)
        {
            int textLength = plainText.Length;
            //list 3shan a3ml represent ll rail matrix
            List<StringBuilder> RAILMATRIX = new List<StringBuilder>();
            for (int i = 0; i < key; i++)
            {
                RAILMATRIX.Add(new StringBuilder());
            }
            //variables 3shan ashof elcurrent row w eltraverse
            int row = 0;
            bool traverse = false;
            //balf 3la kol character
            foreach (char c in plainText)
            {
                // ba7ot elcurrent char fel plaintext fel rail matrix fel current row
                RAILMATRIX[row].Append(c);

                // bashof lw wslt lltop aw elbottom bta3 elrail aw bdlt eldirection y3ni ro7t fi str tany
                if (row == 0 || row == key - 1)
                    traverse = !traverse;

                // bat7rk l row gdid 7sb ekcurrent direction 
                row += traverse ? 1 : -1;
            }
            //  store the ciphertext
            StringBuilder ciphertext = new StringBuilder();


            foreach (StringBuilder rowString in RAILMATRIX)
            {
                // ba7ot characters fel current row fel ciphertext
                ciphertext.Append(rowString);
            }
            return ciphertext.ToString();

            // throw new NotImplementedException();
        }

    }
}
