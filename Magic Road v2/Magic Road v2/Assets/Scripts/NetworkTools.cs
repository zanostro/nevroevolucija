using System;
using System.Collections.Generic;
using System.Text;



    public class NetworkTools
    {
        private static Random random = new Random(Guid.NewGuid().GetHashCode());
        public static double[] createArray(int size, double init_value)
        {
            if (size < 1) return null;
            double[] ar = new double[size];
            for (int i = 0; i < size; i++) ar[i] = init_value;
            return ar;
        }
        public static double[] createRandomArray(int size, double lower_bound, double upper_bound)
        {
            if (size < 1) return null;
            double[] ar = new double[size];
            for (int i = 0; i < size; i++) ar[i] = randomValue(lower_bound, upper_bound);
            return ar;
        }
        public static double[][] createRandomArray(int sizeX, int sizeY, double lower_bound, double upper_bound)
        {
            if (sizeX < 1 || sizeY < 1) return null;
            double[][] ar = new double[sizeX][];
            for (int i = 0; i < sizeX; i++)
            {
                ar[i] = createRandomArray(sizeY, lower_bound, upper_bound);
            }
            return ar;
        }

        public static double randomValue(double lower_bound, double upper_bound)
        {
            return random.NextDouble() * (upper_bound - lower_bound) + lower_bound;
        }

        public static int[] randomValues(int lowerBound, int upperBound, int amount)
        {
            lowerBound--;
            if (amount > (upperBound - lowerBound))
            {
                return null;
            }

            int[] values = new int[amount];
            for (int i = 0; i < amount; i++)
            {
                int n = (int)(random.NextDouble() * (upperBound - lowerBound + 1) + lowerBound);
                while (containsValue(values, n))
                {
                    n = (int)(random.NextDouble() * (upperBound - lowerBound + 1) + lowerBound);
                }
                values[i] = n;
            }
            return values;

        }


        public static bool containsValue(int[] arr, int value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != 0)
                {
                    if (value == arr[i])
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        public static int indexOfHighestValue(double[] values)
        {
            int index = 0;
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] > values[index])
                {
                    index = i;
                }
            }
            return index;
        }
    }

