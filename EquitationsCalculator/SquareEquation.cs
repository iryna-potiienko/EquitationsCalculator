using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquitationsCalculator
{
    public class SquareEquation: Equation
    {
        public SquareEquation(double k1, double k2, double k3, double a, double b) : base(k1, k2, k3, a, b) { }
        public override double f(double x)
        {
            double ans = k1 * x * x + k2 * x + k3;
            return ans;
        }
        public override double f1Deriv(double x)
        {
            double ans = k1 * 2 * x + k2;
            return ans;
        }
        public override double f2Deriv(double x)
        {
            return k1*2;
        }
        public int NumberOfRoots()
        {
            int number;
            double d = D();
            if (d > 0) number = 2;
            else if (d == 0) number = 1;
            else number = 0;
            return number;
        }
        public double D()
        {
            return Math.Pow(k2, 2) - 4 * k1 * k3;
        }
        public double[] ApexCoordinates()
        {
            double[] apex = new double[2];
            apex[0] = -k2 / (2 * k1);
            apex[1] = -D() / (4 * k1);
            return apex;
        }
        public override int RootsAmountCheck()
        {
            if (NumberOfRoots() == 0) return 0;
            int baseNumb = base.RootsAmountCheck();
            if (baseNumb == 1) return 1;
            else if (k1 * f(a) > 0)
                if ((a < ApexCoordinates()[0] && b < ApexCoordinates()[0]) || (a > ApexCoordinates()[0] && b > ApexCoordinates()[0])) return 0;
                else return 2;
            else return 0;
        }
        public override void CreateOneRootIntervals()
        {
            double x;
            var intervals = new List<List<double>>();
            if (rootsNumber == 2)
            {
                x = ApexCoordinates()[0];
                intervals.Add(new List<double> { a, x });
                intervals.Add(new List<double> { x, b });
                OneRootIntervals = intervals;
            }
            else base.CreateOneRootIntervals();
        }
    }
}