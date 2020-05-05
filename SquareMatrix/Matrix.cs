using System.Collections.Generic;

public static class Matrix
{
    public static int Determinant(int[][] matrix)   //  det(M) = a * det(a_minor) - b * det(b_minor) + c * det(c_minor) - d * det(d_minor)
    {
        var N = matrix.Length;
        if (N < 1)
        {
            return 0;
        }
        if (N == 1) 
        {
            return matrix[0][0];
        }
        if (N == 2) 
        {
            return matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];   //    [[a,b],[c,d]]     a*d - b*c
        }

        int _result = 0;

        for (int i = 0; i < matrix.Length; i++)
        {
            var current = matrix[0][i] * Determinant(CalcMinor(matrix, new int[] { 0, i }));

            if (i % 2 == 0)
            {
                _result += current;
            }
            else
            {
                _result -= current;
            }
        }
        return _result;
    }

    private static int[][] CalcMinor(int[][] matrix, int[] pos)
    {
        var N = matrix.Length-1;
        var _result = new List<int[]>(N);
        var _subResult = new List<int>(N);
        for (int row = 0; row < matrix.Length; row++)
        {
            if (row != pos[0])
            {
                for (int column = 0; column < matrix.Length; column++)
                {
                    if (column != pos[1])
                    {
                        _subResult.Add(matrix[row][column]);
                    }
                }
                _result.Add(_subResult.ToArray());
                _subResult.Clear();
            }
        }
        return _result.ToArray();
    }
}