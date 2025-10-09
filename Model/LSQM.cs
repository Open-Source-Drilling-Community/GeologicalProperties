using System;
using System.Collections.Generic;
namespace GeologicalProperties.Model
{
    public static class LSQM
    {
        static public double[] CalculateCoefficientsLine(List<double> X, List<double> Y)
        {
            if (X.Count > 2)
            {
                double sumX = 0.0;
                double sumY = 0.0;
                double sumX2 = 0.0;
                double sumY2 = 0.0;
                double sumXY = 0.0;
                double N = (double)X.Count;
                for (int i = 0; i < X.Count; i++)
                {
                    double x = X[i];
                    double y = Y[i];
                    sumX += x;
                    sumY += y;
                    sumX2 += x * x;
                    sumY2 += y * y;
                    sumXY += x * y;
                }
                double[] coefficients = new double[2];
                coefficients[0] = 0.0;
                coefficients[1] = 0.0;

                if (Math.Abs(sumX * sumX - N * sumX2) < 1E-5)
                {
                    return coefficients;
                }
                else
                {
                    coefficients[0] = (N * sumXY - sumY * sumX) / (N * sumX2 - sumX * sumX);
                    coefficients[1] = (sumY - coefficients[0] * sumX) / N;
                    return coefficients;
                }
            }
            else if (X.Count == 2)
            {
                double[] coefficients = new double[2];
                coefficients[0] = (Y[1] - Y[0]) / (X[1] - X[0]);
                coefficients[1] = Y[1] - coefficients[0] * X[1];
                return coefficients;
            }
            else
            {
                return new double[2];
            }


        }
    }
}