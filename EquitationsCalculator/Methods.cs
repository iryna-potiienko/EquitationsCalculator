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
    public class Methods
    {
        public static Double epsilon { get; set; }
        public int DyhotomyIterations;
        public int ModNewtonIterations;
        public int NewtonIterations;
        public List<Double> DyhotomyRoots = new List<double>();
        public List<Double> ModNewtonRoots = new List<double>();
        public List<Double> NewtonRoots = new List<double>();
        
        public void GetResult(Equation equation)
        {
            //if (equation.rootsNumber == 0) return;
         
            foreach (var interval in equation.OneRootIntervals)
            {
                for (int i = 0; i <= 1; i++) {
                    if (equation.f(interval[i]) == 0)
                    {
                        DyhotomyRoots.Add(interval[i]);
                        ModNewtonRoots.Add(interval[i]);
                        NewtonRoots.Add(interval[i]);
                        return;
                    } 
                }
                DyhotomyRoots.Add(Dyhotomy(interval[0], interval[1], equation));
                ModNewtonRoots.Add(ModNewton(interval[0], interval[1], equation));
                NewtonRoots.Add(Newton(interval[0], interval[1], equation));
            }
        }
        public static string StringResult(List<double> roots)
        {
            string res="";
            foreach(var root in roots)
            {
                res += " " + root.ToString() + ";";
            }
            if (res == "") res = " no roots";
            return res;
        }
        public double Dyhotomy(double a, double b, Equation equation)
        {
            double x;
            int i = 0;

            //double apriori = Math.Log((b - a) / epsilon) + 1;
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
                    DyhotomyIterations = i;
                    return x;
                }
            }

            DyhotomyIterations = i;
            return (a + b) / 2;
        }

        public double ModNewton(double a, double b, Equation equation)
        {
            int i = 1;
            //double h;
            double x0 = equation.f(a) * equation.f2Deriv(a) > 0 ? a : b;
            double xn = x0 - equation.f(x0) / equation.f1Deriv(x0);
            //h = Math.Abs(xn - x0);
            while (Math.Abs(xn - x0) > epsilon)
            {
                x0 = xn;
                xn = x0 - equation.f(xn) / equation.f1Deriv(x0);
                i++;
                //h = Math.Abs(xn - x0);
            }
            NewtonIterations = i;
            return xn;
        }
        public double Newton(double a, double b, Equation equation)
        {
            int i = 1;
            double h;
            double x0 = equation.f(a) * equation.f2Deriv(a) > 0 ? a : b;
            double xn = x0 - equation.f(x0) / equation.f1Deriv(x0);
            h = Math.Abs(xn - x0);
            while (Math.Abs(xn - x0) > epsilon)
            {
                x0 = xn;
                xn = x0 - equation.f(xn) / equation.f1Deriv(xn);
                i++;
                //h = Math.Abs(xn - x0);
            }
            NewtonIterations = i;
            return xn;
        }
    }
}