using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {



        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            // Define the key as a 2x2 matrix
            List<int> key = new List<int>();

            // Loop through all possible combinations of the key elements
            for (int a = 0; a < 26; a++)
            {
                for (int b = 0; b < 26; b++)
                {
                    for (int c = 0; c < 26; c++)
                    {
                        for (int d = 0; d < 26; d++)
                        {
                            // Set the elements of the key
                            key.Clear();
                            key.Add(a);
                            key.Add(b);
                            key.Add(c);
                            key.Add(d);


                            // Convert lists to strings
                            string encryptedPlainText = string.Join(",", Encrypt(plainText, key));
                            string encryptedCipherText = string.Join(",", cipherText);

                            // Compare strings
                            if (encryptedPlainText == encryptedCipherText)
                            {
                                // Check if the determinant is non-zero
                                int determinant = (key[0] * key[3] - key[1] * key[2] + 26) % 26;
                                if (determinant != 0 && Gcd(determinant, 26) == 1)
                                {
                                    // Return the guessed key
                                    return key;
                                }
                                else
                                {
                                    throw new InvalidAnlysisException();
                                }




                            }



                        }
                    }
                }
            }
            throw new InvalidAnlysisException();

        }

        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }






        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            if (key.Count == 4)
            {
                int[,] key2D = new int[2, 2];

                // Populate the 2D key array
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        key2D[i, j] = key[i * 2 + j];
                    }
                }
                int det = key2D[0, 0] * key2D[1, 1] - key2D[0, 1] * key2D[1, 0];






                int temp = key2D[1, 1];
                int b = 0;
                key2D[1, 1] = key2D[0, 0];
                key2D[0, 0] = temp;
                key2D[0, 1] *= -1;
                key2D[1, 0] *= -1;
                det = (det % 26 + 26) % 26;


                if (Gcd(det, 26) != 1 && det != 25)
                {
                    throw new Exception("Invalid key: Determinant is not relatively prime to the alphabet size.");
                }




                for (int x = 1; x < 26; x++)
                {
                    if ((det * x) % 26 == 1)
                    {
                        b = x;
                    }
                }

                // Take modulo 26 for all values in the matrix
                for (int i = 0; i < key2D.GetLength(0); i++)
                {
                    for (int j = 0; j < key2D.GetLength(1); j++)
                    {
                        // Take modulo 26
                        key2D[i, j] = (key2D[i, j] % 26 + 26) % 26;
                    }
                }


                // Multiply all values in the matrix by b
                for (int i = 0; i < key2D.GetLength(0); i++)
                {
                    for (int j = 0; j < key2D.GetLength(1); j++)
                    {
                        key2D[i, j] *= b;
                        key2D[i, j] %= 26; // Apply modulo 26

                    }
                }





                List<int> decryptedData = new List<int>();

                // Encrypt each pair of elements from plainText
                for (int w = 0; w < cipherText.Count; w += 2)
                {
                    int partialPT0 = cipherText[w];
                    int partialPT1 = cipherText[w + 1];

                    // Calculate the encrypted values
                    int encrypted0 = key2D[0, 0] * partialPT0 + key2D[0, 1] * partialPT1;
                    int encrypted1 = key2D[1, 0] * partialPT0 + key2D[1, 1] * partialPT1;

                    // Apply modulo operation to keep within range of valid characters
                    decryptedData.Add(encrypted0 % 26);
                    decryptedData.Add(encrypted1 % 26);
                }

                return decryptedData;









            }
            else if (key.Count == 9)
            {

                int[,] key3D = new int[3, 3];

                // Populate the 2D key array
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        key3D[i, j] = key[i * 3 + j];
                    }
                }

                List<int> decryptedData = new List<int>();


                int det = key3D[0, 0] * (key3D[1, 1] * key3D[2, 2] - key3D[1, 2] * key3D[2, 1])
            - key3D[0, 1] * (key3D[1, 0] * key3D[2, 2] - key3D[1, 2] * key3D[2, 0])
            + key3D[0, 2] * (key3D[1, 0] * key3D[2, 1] - key3D[1, 1] * key3D[2, 0]);

                det = (det % 26 + 26) % 26;

                int b = 0;
                for (int x = 1; x < 26; x++)
                {
                    if ((det * x) % 26 == 1)
                    {
                        b = x;
                        break;
                    }
                }
                Console.WriteLine(b);
                int[,] adj = new int[3, 3];
                adj[0, 0] = key3D[1, 1] * key3D[2, 2] - key3D[1, 2] * key3D[2, 1];
                adj[0, 1] = -(key3D[1, 0] * key3D[2, 2] - key3D[1, 2] * key3D[2, 0]);
                adj[0, 2] = key3D[1, 0] * key3D[2, 1] - key3D[1, 1] * key3D[2, 0];

                adj[1, 0] = -(key3D[0, 1] * key3D[2, 2] - key3D[0, 2] * key3D[2, 1]);
                adj[1, 1] = key3D[0, 0] * key3D[2, 2] - key3D[0, 2] * key3D[2, 0];
                adj[1, 2] = -(key3D[0, 0] * key3D[2, 1] - key3D[0, 1] * key3D[2, 0]);

                adj[2, 0] = key3D[0, 1] * key3D[1, 2] - key3D[0, 2] * key3D[1, 1];
                adj[2, 1] = -(key3D[0, 0] * key3D[1, 2] - key3D[0, 2] * key3D[1, 0]);
                adj[2, 2] = key3D[0, 0] * key3D[1, 1] - key3D[0, 1] * key3D[1, 0];

                int[,] inverse = new int[3, 3];

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        inverse[i, j] = (b * adj[i, j] % 26 + 26) % 26;
                    }
                }

                // Transpose the inverse matrix
                int[,] transposedInverse = new int[3, 3];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        transposedInverse[i, j] = inverse[j, i];
                    }
                }





                for (int w = 0; w < cipherText.Count; w += 3)
                {
                    int partialPT0 = cipherText[w];
                    int partialPT1 = cipherText[w + 1];
                    int partialPT2 = cipherText[w + 2];

                    // Calculate the encrypted values
                    int decrypted0 = transposedInverse[0, 0] * partialPT0 + transposedInverse[0, 1] * partialPT1 + transposedInverse[0, 2] * partialPT2;
                    int decrypted1 = transposedInverse[1, 0] * partialPT0 + transposedInverse[1, 1] * partialPT1 + transposedInverse[1, 2] * partialPT2;
                    int decrypted2 = transposedInverse[2, 0] * partialPT0 + transposedInverse[2, 1] * partialPT1 + transposedInverse[2, 2] * partialPT2;

                    // Apply modulo operation to keep within range of valid characters
                    decryptedData.Add(decrypted0 % 26);
                    decryptedData.Add(decrypted1 % 26);
                    decryptedData.Add(decrypted2 % 26);

                }
                return decryptedData;





            }
            return new List<int>();

        }




        // Function to calculate the greatest common divisor (GCD) of two integers
        private int Gcd(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }





        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }





        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            if (key.Count == 4)
            {
                // Ensure plainText length is even
                if (plainText.Count % 2 != 0)
                {
                    plainText.Add(23);
                }
                int[,] key2D = new int[2, 2];

                // Populate the 2D key array
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        key2D[i, j] = key[i * 2 + j];
                    }
                }

                List<int> encryptedData = new List<int>();

                // Encrypt each pair of elements from plainText
                for (int w = 0; w < plainText.Count; w += 2)
                {
                    int partialPT0 = plainText[w];
                    int partialPT1 = plainText[w + 1];

                    // Calculate the encrypted values
                    int encrypted0 = key2D[0, 0] * partialPT0 + key2D[0, 1] * partialPT1;
                    int encrypted1 = key2D[1, 0] * partialPT0 + key2D[1, 1] * partialPT1;

                    // Apply modulo operation to keep within range of valid characters
                    encryptedData.Add(encrypted0 % 26);
                    encryptedData.Add(encrypted1 % 26);
                }
                return encryptedData;

            }
            else if (key.Count == 9)
            {
                // Ensure plainText length is even
                while (plainText.Count % 3 != 0)
                {
                    plainText.Add(23);
                }

                int[,] key3D = new int[3, 3];

                // Populate the 2D key array
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        key3D[i, j] = key[i * 3 + j];
                    }
                }

                List<int> encryptedData = new List<int>();
                // Encrypt each pair of elements from plainText
                for (int w = 0; w < plainText.Count; w += 3)
                {
                    int partialPT0 = plainText[w];
                    int partialPT1 = plainText[w + 1];
                    int partialPT2 = plainText[w + 2];

                    // Calculate the encrypted values
                    int encrypted0 = key3D[0, 0] * partialPT0 + key3D[0, 1] * partialPT1 + key3D[0, 2] * partialPT2;
                    int encrypted1 = key3D[1, 0] * partialPT0 + key3D[1, 1] * partialPT1 + key3D[1, 2] * partialPT2;
                    int encrypted2 = key3D[2, 0] * partialPT0 + key3D[2, 1] * partialPT1 + key3D[2, 2] * partialPT2;

                    // Apply modulo operation to keep within range of valid characters
                    encryptedData.Add(encrypted0 % 26);
                    encryptedData.Add(encrypted1 % 26);
                    encryptedData.Add(encrypted2 % 26);

                }
                return encryptedData;


            }

            return new List<int>();
        }





        public string Encrypt(string plainText, string key)
        {
            throw new NotImplementedException();

        }


        public List<int> Analyse3By3Key(List<int> plain3, List<int> cipher3)
        {
            List<int> key = new List<int>();

            // Loop through all possible combinations of the key elements
            for (int a = 1; a < 26; a++)
            {
                for (int b = 10; b < 26; b++)
                {
                    for (int c = 0; c < 26; c++)
                    {
                        for (int d = 0; d < 26; d++)
                        {
                            for (int e = 19; e < 26; e++)
                            {
                                for (int f = 0; f < 26; f++)
                                {
                                    for (int g = 0; g < 26; g++)
                                    {
                                        for (int h = 0; h < 26; h++)
                                        {
                                            for (int i = 0; i < 26; i++)
                                            {
                                                // Set the elements of the key
                                                key.Clear();
                                                key.Add(a);
                                                key.Add(b);
                                                key.Add(c);
                                                key.Add(d);
                                                key.Add(e);
                                                key.Add(f);
                                                key.Add(g);
                                                key.Add(h);
                                                key.Add(i);

                                                // Convert lists to strings
                                                string encryptedPlainText = string.Join(",", Encrypt(plain3, key));
                                                string encryptedCipherText = string.Join(",", cipher3);

                                                // Compare strings
                                                if (encryptedPlainText == encryptedCipherText)
                                                {
                                                    // Check if the determinant is non-zero
                                                    int determinant = (a * (e * i - f * h) - b * (d * i - f * g) + c * (d * h - e * g) + 26) % 26;
                                                    if (determinant != 0 && Gcd(determinant, 26) == 1)
                                                    {
                                                        // Return the guessed key
                                                        return key;
                                                    }
                                                    else
                                                    {
                                                        throw new InvalidAnlysisException();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            throw new InvalidAnlysisException();
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }
    }
}



