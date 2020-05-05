using System;
using static Matrix; 

namespace ConsoleApp1
{
    public static class Program
    {

        static void Main()
        {
            int[][] _matrix2x2 = new int [][] { new int[]{ 1, 2 }, new int[] { 3, 4 } };
            int[][] _matrix3x3 = new int[][] { new int[] { 1, 0, 11 }, new int[] { 3, 6, 5 }, new int[] { 5, 6, 7 } };
            int[][] _matrix4x4 = new int[][] { new int[] { 1, 2, 11, 5 }, new int[] { 3, 6, 5, 6 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 2, 11, 0 } };

            Console.WriteLine(Determinant(_matrix2x2));
            Console.WriteLine(Determinant(_matrix3x3));
            Console.WriteLine(Determinant(_matrix4x4));
        }
    }
}