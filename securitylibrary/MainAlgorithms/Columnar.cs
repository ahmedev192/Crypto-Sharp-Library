using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            SortedDictionary<int, int> sortedDictionary = new SortedDictionary<int, int>();
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            double plainTxtSize = plainText.Length;

            for (int z = 1; z < Int32.MaxValue; z++)
            {
                int c = 0;
                double columns = z;
                double rows = Math.Ceiling(plainTxtSize / z); ;
                string[,] pt = new string[(int)rows, (int)columns];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < z; j++)
                    {
                        if (c >= plainTxtSize)
                        {
                            pt[i, j] = "";
                        }
                        else
                        {
                            pt[i, j] = plainText[c].ToString();
                            c++;
                        }
                    }
                }

                List<string> mylist = new List<string>();
                for (int i = 0; i < z; i++)
                {
                    string word = "";
                    for (int j = 0; j < rows; j++)
                    {
                        word += pt[j, i];
                    }
                    mylist.Add(word);
                }

                if (mylist.Count == 7)
                {
                    string d = "";
                }


                bool correctkey = true;
                string cipherCopy = (string)cipherText.Clone();
                sortedDictionary = new SortedDictionary<int, int>();
                for (int i = 0; i < mylist.Count; i++)
                {
                    //get index of first substring occurance
                    int x = cipherCopy.IndexOf(mylist[i]);
                    if (x == -1)
                    {
                        correctkey = false;
                    }
                    else
                    {
                        sortedDictionary.Add(x, i + 1);
                        cipherCopy.Replace(mylist[i], "#");
                    }
                }
                if (correctkey)
                    break;
            }
            List<int> output = new List<int>();
            Dictionary<int, int> newDictionary = new Dictionary<int, int>();


            for (int i = 0; i < sortedDictionary.Count; i++)
            {
                newDictionary.Add(sortedDictionary.ElementAt(i).Value, i + 1);
            }

            for (int i = 1; i < newDictionary.Count + 1; i++)
            {
                output.Add(newDictionary[i]);
            }
            return output;

        }

        public string Decrypt(string cipherText, List<int> key)
        {
            int columns = key.Count;
            int rows = (int)Math.Ceiling((double)cipherText.Length / columns);
            int cnt = 0;


            char[,] matrix = new char[(int)rows, (int)columns];


            // filling the dictionary
            Dictionary<int, int> cipherDictionary = new Dictionary<int, int>();
            for (int i = 0; i < key.Count; i++)
            {
                cipherDictionary.Add(key[i] - 1, i);
            }

            int numberOfFullColumns = cipherText.Length % key.Count;

            for (int i = 0; i < key.Count; i++)
            {
                for (int k = 0; k < rows; k++)
                {
                    if (numberOfFullColumns != 0 && k == rows - 1 && cipherDictionary[i] >= numberOfFullColumns)
                        continue;

                    matrix[k, cipherDictionary[i]] = cipherText[cnt];
                    cnt++;
                }

            }


            StringBuilder temp = new StringBuilder();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    temp.Append(matrix[i, j]);
                }
            }
            string plainText = temp.ToString();
            return plainText.ToUpper();
        }


        public string Encrypt(string plainText, List<int> key)
        {
            int columns = key.Count;
            int rows = (int)Math.Ceiling((double)plainText.Length / columns);


            char[,] matrix = new char[(int)rows, (int)columns];

            int c = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (c >= plainText.Length)
                    {
                        matrix[i, j] = 'x';
                    }
                    else
                    {
                        matrix[i, j] = plainText[c];
                        c++;
                    }
                }
            }

            //filling the dictionary
            Dictionary<int, int> keyDictionary = new Dictionary<int, int>();
            for (int i = 0; i < key.Count; i++)
            {
                keyDictionary.Add(key[i] - 1, i);
            }

            string myciphertext = "";

            for (int i = 0; i < key.Count; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    myciphertext += matrix[j, keyDictionary[i]];
                }
            }
            return myciphertext.ToUpper();
        }

    }
}
