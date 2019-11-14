using System;
using System.Linq;
using System.Numerics;

namespace DeterminantCalculation
{
    class Program
    {
        public static BigInteger GetDet(int[,] mat)
        {
            if (mat.GetLength(0) == 2)
                return mat[0, 0] * mat[1, 1] - mat[1, 0] * mat[0, 1];
            BigInteger result = 0;
            for (var col = 0; col < mat.GetLength(0); col++)
            {
                var decomposedMat = new int[mat.GetLength(0) - 1, mat.GetLength(1) - 1];
                var colNum = 0;
                for (var decCol = 0; decCol < mat.GetLength(1); decCol++)
                {
                    if (decCol == col)
                        continue;
                    for (var decRow = 1; decRow < mat.GetLength(0); decRow++)
                    {
                        decomposedMat[decRow-1, colNum] = mat[decRow, decCol];
                    }
                    colNum++;
                }
                result += (col % 2 == 0 ? 1 : -1) * mat[0, col] * GetDet(decomposedMat);
            }
            return result;
        }

        static void Main()
        {
            var matStr = Console.ReadLine();

            var splMat = matStr.Split(" \\\\ ");
            var dim = splMat.Length;
            var mat = new int[dim, dim];

            for (var row = 0; row < dim; row++)
            {
                var rowContents = splMat[row].Split(" & ").Select(x => int.Parse(x)).ToArray();
                for (var col = 0; col < dim; col++)
                    mat[row, col] = rowContents[col];
            }
            
            var matDet = GetDet(mat);
            Console.WriteLine(matDet);
        }
    }
}
