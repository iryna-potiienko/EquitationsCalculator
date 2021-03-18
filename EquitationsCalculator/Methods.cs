using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquitationsCalculator
{
    public static class Methods
    {
        public static Double epsilon { get; set; }
        public static Double iterations;

        /*public static double DyhotomyN(double a, double b, Equation equation)
        {
            if (equation.NumberOfRoots() == 2)
            {

            }
        }*/
        public static double Dyhotomy(double a, double b, Equation equation)
        {
            double x;
            int i = 0;

            double apriori = Math.Log((b - a) / epsilon) + 1;
            while (b - a > 2 * epsilon)
            {
                x = (a + b) / 2;
                if (equation.f(x) != 0)
                {
                    if (equation.f(a) * equation.f(x) < 0)
                        b = x;
                    else
                        a = x;
                    i++;
                }
                else
                {
                    iterations = i;
                    return x;
                }
                //printf(" %i \t%f \t%f \t%f\n", i, x, b - a, f(x));
            }

            //printf("Оцінка кількості кроків: \n-апріорна: %i\n-апостаріорна: %i\n", apriori, i);
            iterations = i;
            return (a + b) / 2;
        }
    }
}