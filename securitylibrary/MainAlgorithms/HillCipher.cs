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













        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            if (IsValidKey(key))
            {
                NormalizePlainText(plainText, key);
                int dimension = (int)Math.Sqrt(key.Count);
                int[,] keyMatrix = GenerateKeyMatrix(key, dimension);

                List<int> encryptedData = new List<int>();

                for (int i = 0; i < plainText.Count; i += dimension)
                {
                    int[] partialPlainText = GetPartialPlainText(plainText, i, dimension);
                    int[] encryptedValues = MultiplyMatrix(keyMatrix, partialPlainText);

                    foreach (int encryptedValue in encryptedValues)
                    {
                        encryptedData.Add(Modulo(encryptedValue));
                    }
                }
                return encryptedData;
            }

            return new List<int>();
        }

        private bool IsValidKey(List<int> key)
        {
            return key.Count == 4 || key.Count == 9;
        }

        private void NormalizePlainText(List<int> plainText, List<int> key)
        {
            int blockSize = (int)Math.Sqrt(key.Count);
            while (plainText.Count % blockSize != 0)
            {
                plainText.Add(23); // Padding with 'W' character
            }
        }

        private int[,] GenerateKeyMatrix(List<int> key, int dimension)
        {
            int[,] keyMatrix = new int[dimension, dimension];
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    keyMatrix[i, j] = key[i * dimension + j];
                }
            }
            return keyMatrix;
        }

        private int[] GetPartialPlainText(List<int> plainText, int startIndex, int dimension)
        {
            int[] partialPlainText = new int[dimension];
            for (int i = 0; i < dimension; i++)
            {
                partialPlainText[i] = plainText[startIndex + i];
            }
            return partialPlainText;
        }

        private int[] MultiplyMatrix(int[,] matrix, int[] vector)
        {
            int dimension = (int)Math.Sqrt(matrix.Length);
            int[] result = new int[dimension];
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    result[i] += matrix[i, j] * vector[j];
                }
            }
            return result;
        }

        private int Modulo(int value)
        {
            return value % 26; // Assuming we're working with English alphabets
        }









 

        public List<int> Analyse3By3Key(List<int> plain3, List<int> cipher3)
        {
            // Initialize variables and arrays
            int[,] cipherMatrix = new int[3, 3];
            int[,] plainMatrix = new int[3, 3];
            List<int> keyList = new List<int>();

            // Populate plainMatrix and cipherMatrix from the input lists
            PopulateMatrixFromList(plain3, plainMatrix);
            PopulateMatrixFromList(cipher3, cipherMatrix);

            // Calculate determinant of plainMatrix
            int det = CalculateDeterminant(plainMatrix);

            // Check if the determinant is valid
            ValidateDeterminant(det);

            // Calculate multiplicative inverse
            int multiplicativeInverse = CalculateMultiplicativeInverse(det);

            // Calculate inverse of plainMatrix
            List<int> inversePlain = CalculateInverseMatrix(plainMatrix, multiplicativeInverse);

            // Transpose inversePlain matrix
            int[,] transposedInversePlain = TransposeMatrix(inversePlain);

            // Calculate key matrix
            CalculateKeyMatrix(cipherMatrix, transposedInversePlain, keyList);

            return keyList;
        }

        // Populate a 3x3 matrix from a list
        private void PopulateMatrixFromList(List<int> list, int[,] matrix)
        {
            int index = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix[i, j] = list[index];
                    index++;
                }
            }
        }

        // Calculate determinant of a 3x3 matrix
        private int CalculateDeterminant(int[,] matrix)
        {
            int det = 0;
            for (int i = 0; i < 3; i++)
            {
                det += (matrix[0, i] * (matrix[1, (i + 1) % 3] * matrix[2, (i + 2) % 3] -
                        matrix[1, (i + 2) % 3] * matrix[2, (i + 1) % 3]));
            }
            if (det < 0)
                det = (26 - (-1 * det % 26));
            else
                det = det % 26;
            return det;
        }

        // Validate determinant
        private void ValidateDeterminant(int det)
        {
            int gcd = 1;
            for (int i = 2; i <= det; i++)
            {
                if (26 % i == 0 && det % i == 0)
                    gcd = i;
            }

            if (det == 0 || gcd != 1)
                throw new Exception("Invalid determinant");
        }

        // Calculate multiplicative inverse
        private int CalculateMultiplicativeInverse(int det)
        {
            int multiplicativeInverse = 0;
            for (int i = 1; i < 26; i++)
            {
                if (det * i % 26 == 1)
                {
                    multiplicativeInverse = i;
                    break;
                }
            }
            return multiplicativeInverse;
        }

        // Calculate inverse of plainMatrix
        private List<int> CalculateInverseMatrix(int[,] plainMatrix, int multiplicativeInverse)
        {
            List<int> inversePlain = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                int result = multiplicativeInverse * ((plainMatrix[1, (i + 1) % 3] * plainMatrix[2, (i + 2) % 3] -
                 plainMatrix[1, (i + 2) % 3] * plainMatrix[2, (i + 1) % 3]));
                if (result < 0)
                    result = (26 - (-1 * result) % 26);
                else
                    result %= 26;
                inversePlain.Add(result);
            }

            for (int i = 0; i < 3; i++)
            {
                int value = multiplicativeInverse;
                int result = value * ((plainMatrix[2, (i + 1) % 3] * plainMatrix[0, (i + 2) % 3] -
                 plainMatrix[2, (i + 2) % 3] * plainMatrix[0, (i + 1) % 3]));
                if (result < 0)
                    result = (26 - (-1 * result) % 26);
                else
                    result %= 26;
                inversePlain.Add(result);
            }

            for (int i = 0; i < 3; i++)
            {
                int value = multiplicativeInverse;
                int result = value * ((plainMatrix[0, (i + 1) % 3] * plainMatrix[1, (i + 2) % 3] -
                 plainMatrix[0, (i + 2) % 3] * plainMatrix[1, (i + 1) % 3]));
                if (result < 0)
                    result = (26 - (-1 * result) % 26);
                else
                    result %= 26;
                inversePlain.Add(result);
            }
            return inversePlain;
        }

        // Transpose a matrix
        private int[,] TransposeMatrix(List<int> matrixList)
        {
            int index = 0;
            int[,] transposedMatrix = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    transposedMatrix[j, i] = matrixList[index];
                    index++;
                }
            }
            return transposedMatrix;
        }

        // Calculate key matrix
        private void CalculateKeyMatrix(int[,] cipherMatrix, int[,] transposedInversePlain, List<int> keyList)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    int sum = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        sum += cipherMatrix[k, j] * transposedInversePlain[i, k];
                    }
                    if (sum < 0)
                    {
                        sum = 26 - (sum * -1) % 26;
                        keyList.Add(sum);
                    }
                    else
                    {
                        sum = sum % 26;
                        keyList.Add(sum);
                    }
                }
            }
        }



        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }


        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            throw new NotImplementedException();

        }

    }
}



