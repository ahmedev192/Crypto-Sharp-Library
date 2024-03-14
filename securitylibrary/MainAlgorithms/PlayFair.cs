using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographicTechnique<string, string>
    {
        /// <summary>
        /// The most common diagrams in english (sorted): TH, HE, AN, IN, ER, ON, RE, ED, ND, HA, AT, EN, ES, OF, NT, EA, TI, TO, IO, LE, IS, OU, AR, AS, DE, RT, VE
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public string Analyse(string plainText)
        {
            throw new NotImplementedException();
        }

        public string Analyse(string plainText, string cipherText)
        {
            throw new NotSupportedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            string new_key = key.ToUpper();
            char[] arr = new_key.ToCharArray();
            char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' }; // Removed 'J' from the alphabet
            char[] total_arr = arr.Concat(alphabet).ToArray();
            HashSet<char> unique_arr = new HashSet<char>();
            foreach (char x in total_arr)
            {
                if (!unique_arr.Contains(x))
                    unique_arr.Add(x);
            }

            char[][] matrix = new char[5][];
            for (int i = 0; i < 5; i++)
            {
                matrix[i] = new char[5];
                for (int j = 0; j < 5; j++)
                {
                    matrix[i][j] = unique_arr.ElementAt(i * 5 + j);
                }
            }
            cipherText = cipherText.ToUpper();
            List<string> pairs = new List<string>();
            for (int i = 0; i < cipherText.Length; i += 2)
            {
                char first = cipherText[i];
                char second = cipherText[i + 1];
                pairs.Add(first.ToString() + second.ToString());
            }

            StringBuilder plainText = new StringBuilder();
            foreach (string pair in pairs)
            {
                char first = pair[0];
                char second = pair[1];
                int row1 = 0, col1 = 0, row2 = 0, col2 = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (matrix[i][j] == first)
                        {
                            row1 = i;
                            col1 = j;
                        }
                        if (matrix[i][j] == second)
                        {
                            row2 = i;
                            col2 = j;
                        }
                    }
                }
                if (row1 == row2)
                {
                    plainText.Append(matrix[row1][(col1 - 1 + 5) % 5]);
                    plainText.Append(matrix[row2][(col2 - 1 + 5) % 5]);
                }
                else if (col1 == col2)
                {
                    plainText.Append(matrix[(row1 - 1 + 5) % 5][col1]);
                    plainText.Append(matrix[(row2 - 1 + 5) % 5][col2]);
                }
                else
                {
                    plainText.Append(matrix[row1][col2]);
                    plainText.Append(matrix[row2][col1]);
                }
            }

            string plaintextString = plainText.ToString();
            int length = plaintextString.Length;

            for (int i = length - 3; i >= 1; i--)
            {
                if (plaintextString[i] == 'X' && plaintextString[i - 1] == plaintextString[i + 1] && (i) % 2 != 0)
                {
                    plaintextString = plaintextString.Remove(i, 1);

                }
            }
            if (plaintextString.EndsWith("X"))
            {
                plaintextString = plaintextString.Remove(plaintextString.Length - 1);
            }

            return plaintextString;

            throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            string new_key = key.ToUpper();
            char[] arr = new_key.ToCharArray();
            char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' }; // Removed 'J' from the alphabet
            char[] total_arr = arr.Concat(alphabet).ToArray();
            HashSet<char> unique_arr = new HashSet<char>();
            foreach (char x in total_arr)
            {
                if (!unique_arr.Contains(x))
                    unique_arr.Add(x);
            }

            char[][] matrix = new char[5][];
            for (int i = 0; i < 5; i++)
            {
                matrix[i] = new char[5];
                for (int j = 0; j < 5; j++)
                {
                    matrix[i][j] = unique_arr.ElementAt(i * 5 + j);
                }
            }
            plainText = plainText.ToUpper();
            List<string> pairs = new List<string>();
            for (int i = 0; i < plainText.Length; i += 2)
            {
                char first = plainText[i];
                char second = (i + 1 < plainText.Length) ? plainText[i + 1] : 'X'; //  one character left

                if (first == second) //the same two character
                {
                    pairs.Add(first + "X");
                    i--;
                }
                else
                {
                    pairs.Add(first.ToString() + second.ToString());
                }
            }

            StringBuilder cipherText = new StringBuilder();
            foreach (string pair in pairs)
            {
                char first = pair[0];
                char second = pair[1];
                int row1 = 0, col1 = 0, row2 = 0, col2 = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (matrix[i][j] == first)
                        {
                            row1 = i;
                            col1 = j;
                        }
                        if (matrix[i][j] == second)
                        {
                            row2 = i;
                            col2 = j;
                        }
                    }
                }

                if (row1 == row2)
                {
                    cipherText.Append(matrix[row1][(col1 + 1) % 5]);
                    cipherText.Append(matrix[row2][(col2 + 1) % 5]);
                }
                else if (col1 == col2)
                {
                    cipherText.Append(matrix[(row1 + 1) % 5][col1]);
                    cipherText.Append(matrix[(row2 + 1) % 5][col2]);
                }
                else
                {
                    cipherText.Append(matrix[row1][col2]);
                    cipherText.Append(matrix[row2][col1]);
                }
            }
            return cipherText.ToString();
        }
        //   throw new NotImplementedException();
    }
}

