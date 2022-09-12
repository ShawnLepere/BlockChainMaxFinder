using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

//Tested in .NET Core 2.1
//Tested in .NET Core 3.1

namespace bitFlyerMaxCalculator
{
    class Program
    {
        //Delcare list of transaction as string and later parse them as long and decimal respectively
        static string[,] transacList = new string[,] { { "57247", "0.0887" }, { "98732", "0.1856" }, { "134928", "0.2307" }, { "77275", "0.1522" }, { "29240", "0.0532" }, { "15440", "0.025" }, { "70820", "0.1409" }, { "139603", "0.2541" }, { "63718", "0.1147" }, { "143807", "0.266" }, { "190457", "0.2933" }, { "40572", "0.0686" } };
        static string[,] orderedTransacList = new string[12, 2]; //order transact list to use largest first for quickSort method (NOT USED)

        //List of transaction combinations (if not using Linq)
        static List<List<int[]>> combinations = new List<List<int[]>>();

        //Max bytes of transactions per block
        static double maxTransac = 1000000; //1000000;//500000;

        //DataTable for transactions (if using Linq method)
        static DataTable transacDt;

        static void Main(string[] args)
        {
            bubbleSortList();

            try
            {

                double possibilityCount = Math.Pow(2, (transacList.Length / 2));    //Get number of possible combinations (2^number of transactions)
                List<string> matchedMaxPointer = new List<string>();    //List of bytes that meet reward criteria : does not save block combination that generated this result
                decimal reward = 12.5m; //Additiona BTC Reward
                //List<int> doneList = new List<int>(); //Used to log the calculator actions for performance analysis : commented out in release version
                //List<string> donebinary = new List<string>(); //Used to log the calculator actions for performance analysis : commented out in release version

                for (int i = 1; i < possibilityCount; i++)  //Loop For each possible block combination
                {
                    double pointerSum = 0;
                    string possCountBinary = Convert.ToString(i, 2).PadLeft(12, '0');    //Get binary tree equivalent of possibility
                    string binarylog = "";



                    for (int j = 0; j < possCountBinary.Length; j++)
                    {
                        binarylog = possCountBinary.Substring(0, j);

                        if (possCountBinary[j].Equals('1'))
                            pointerSum += double.Parse(transacList[j, 0]);

                        if (pointerSum >= maxTransac)   //When the block exceeds the maximum transactions : break the loop and save 
                        {
                            // --This Optimzations begins here--

                            // Optimize results lookup by skipping over next binary tree branch
                            // Only works in the following conditions : list is sorted properly + list is traversed in right direction
                            // This optimization assumes that the next set of results from the previous root will automatically exceed the maximum transaction limit

                            if (Convert.ToString(i, 2).Contains("0"))
                            {
                                StringBuilder sb = new StringBuilder(Convert.ToString(i, 2));
                                //sb.Replace('0', '1'); //Extended Optimization Test : does not work in current iteration
                                sb[Convert.ToString(i, 2).LastIndexOf("0")] = '1';
                                i = Convert.ToInt32(sb.ToString(), 2);
                            }

                            // --This Optimization ends here--

                            //break from loop
                            break;
                        }
                    }

                    //Used to log the calculator actions for performance analysis : commented out in release version
                    //doneList.Add(Convert.ToInt32(possCountBinary, 2));
                    //donebinary.Add(possCountBinary);

                    if (pointerSum >= maxTransac)
                    {
                        if (!matchedMaxPointer.Contains(binarylog))
                            matchedMaxPointer.Add(binarylog);
                    }
                }

                string max = Convert.ToString(matchedMaxPointer.Max(e => Convert.ToInt32(e, 2)), 2);
                List<decimal> rewardList = new List<decimal>();
                foreach (var item in matchedMaxPointer)
                {
                    reward = 12.5m;
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (item[i].Equals('1'))
                            reward += Decimal.Parse(transacList[i, 1]);
                    }
                    rewardList.Add(reward);
                }

                decimal maxrew = rewardList.Max();

                reward = 12.5m;

                for (int i = 0; i < max.Length; i++)
                {
                    if (max[i].Equals('1'))
                        reward += Decimal.Parse(transacList[i, 1]);
                }

                Console.WriteLine(maxrew);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        //Binary Tree Root Class :
        //Used in Binary Tree Data Structure Method
        //Not used in this implementation
        class BinaryTreeRoot
        {
            string value;
            BinaryTreeRoot left;
            BinaryTreeRoot right;
        }


        //Quicksort function : 
        //Should most likely sort faster than a bubblesort function for larger datasets
        //It will require a larger dataset to effectively test performance.
        //This quicksort function unfinished and requires proper testing
        static void quickSortList()
        {
            throw new NotImplementedException();

            List<int> indexList = new List<int>();
            int largestIndex = -1;
            string[,] sortedList = new string[,] { };
            for (int i = 0; i < transacList.Length / 2; i++)
            {
                largestIndex = i;

                for (int k = 0; k < (transacList.Length / 2); k++)
                {
                    if (indexList.Contains(k))
                        continue;
                    if (Double.Parse(transacList[k, 0]) > Double.Parse(transacList[largestIndex, 0]))
                    { largestIndex = k; }
                }
                indexList.Add(largestIndex);
            }

            for (int i = 0; i < indexList.Count; i++)
            {
                string[,] arr1 = new string[,] { { transacList[indexList[i], 0], transacList[indexList[i], 1] } };
                Array.Copy(arr1, 0, orderedTransacList, i * 2, arr1.Length);
            }
        }

        //Bubblesort function :
        //Sort the list using a bubblesort method
        //List will be sorted from smallest-to-largest
        //If the main program is reading the binary values from right to left, this sort should be changed to largest-to-smallest
        static void bubbleSortList()
        {
            string[] bubble = new string[] { }; //buffer for storing the bubble data

            for (int k = 0; k < (transacList.Length / 2); k++)  //Primary For loop to conduct inital pass of array
            {
                for (int i = 0; i < (transacList.Length / 2) - 2; i++)  //Nested for loop to go over array a second time for each item moved
                {
                    if (double.Parse(transacList[i, 0]) > double.Parse(transacList[i + 1, 0]))  //compare first item to second item and change their positions accodring to smallest and largest
                    {
                        bubble = new string[] { transacList[i, 0], transacList[i, 1] };
                        transacList[i, 0] = transacList[i + 1, 0];
                        transacList[i, 1] = transacList[i + 1, 1];
                        transacList[i + 1, 0] = bubble[0];
                        transacList[i + 1, 1] = bubble[1];
                    }
                }
            }


            //For Linq solution - Insert all tranasctions into transacDt
            //Note : Not used in current implementation
            void UpdateTransacDt() 
            {
                transacDt = new DataTable();
                transacDt.Columns.Add("ID", typeof(int));
                transacDt.Columns.Add("Size", typeof(int));
                transacDt.Columns.Add("Fee", typeof(decimal));
                for (int i = 0; i < transacList.Length / 2; i++)
                {
                    DataRow dr = transacDt.NewRow();
                    dr[0] = i + 1;
                    dr[1] = int.Parse(transacList[i, 0]);
                    dr[2] = decimal.Parse(transacList[i, 1]);
                    transacDt.Rows.Add(dr);
                }
            }
        }
    }
}
