using System;
using System.Globalization;

namespace Polynoms
{
    public static class Cuesor
    {
        private static double GetValue(double[] coeffs, double x)
        {
            var ans = coeffs[coeffs.Length - 1];
            var index = coeffs.Length - 2;
            var powX = x;
            while (index >= 0)
            {
                ans += powX * coeffs[index];
                powX *= x;
                index--;
            }

            return ans;
        }

        private static double BinSearch(double leftBorder, double rightBorder, double[] coeffs)
        {
            var leftSign = Math.Sign(GetValue(coeffs, leftBorder));
            var rightSign = Math.Sign(GetValue(coeffs, rightBorder));
            for (var i = 0; i < 100; i++)
            {
                var middle = (leftBorder + rightBorder) / 2;
                var middleValue = GetValue(coeffs, middle);
                var middleSign = Math.Sign(middleValue);
                if (middleSign == leftSign)
                {
                    leftBorder = middle;
                }
                else
                {
                    rightBorder = middle;
                }
            }

            return leftBorder;
        }

        private static void WriteArray(double[] array)
        {
            foreach (var element in array)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine("");
        }

        private static double SolvePolynom(double[] coeffs)
        {
            var leftBorder = -1000.0;
            var leftValue = GetValue(coeffs, leftBorder);
            var derivative = GetDerivative(coeffs);
            var leftDer = GetValue(derivative, leftBorder);
            while (leftBorder < 1000.0)
            {
                var rightBorder = leftBorder + 0.00001;
                var rightValue = GetValue(coeffs, rightBorder);
                var rightDer = GetValue(derivative, rightBorder);

                if (Math.Sign(leftValue) == -Math.Sign(rightValue))
                {
                    return BinSearch(leftBorder, rightBorder, coeffs);
                }

                else if (Math.Sign(leftDer) == -Math.Sign(rightDer))
                {
                    var peak = BinSearch(leftBorder, rightBorder, derivative);
                    if (Math.Abs(GetValue(coeffs, peak)) < 1e-9)
                        return peak;
                }

                leftValue = rightValue;
                leftBorder = rightBorder;
                leftDer = rightDer;
            }

            return Double.NaN;
        }

        internal static string GetAnswer(string question)
        {
            throw new NotImplementedException();
        }

        private static double[] GetCoeffs(string[] args)
        {
            var coeffs = new double[(args.Length + 1) / 2];
            for (var i = 0; i < args.Length; i += 2)
            {
                var startIndex = 0;
                var sign = 1;
                if (args[i][0] == '(')
                {
                    startIndex = 2;
                    sign = -1;
                }

                var index = startIndex;
                while (index < args[i].Length && args[i][index] != ')' && args[i][index] != '*')
                {
                    index++;
                }

                var parsing = args[i].Substring(startIndex, index - startIndex);

                coeffs[i / 2] = sign * double.Parse(parsing,
                                    CultureInfo.InvariantCulture);
            }

            return coeffs;
        }

        private static double[] GetDerivative(double[] coeffs)
        {
            var power = coeffs.Length - 1;
            var derivative = new double[coeffs.Length - 1];
            for (var i = 0; i < derivative.Length; i++)
            {
                derivative[i] = power * coeffs[i];
                power--;
            }

            return derivative;
        }

        public static string GetRoot(string input)
        {
            var args = input.Split();
            var coeffs = GetCoeffs(args);
            var ans = SolvePolynom(coeffs);
            return Double.IsNaN(ans) ? "no roots" : ans.ToString(CultureInfo.InvariantCulture);
        }
    }
}
