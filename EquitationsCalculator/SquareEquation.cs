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
    public class SquareEquation: Equation
    {
        public Double D;
        public int rootsNumber;
        public double[] apex;
        public override Double k1 { get; set; }
        public override Double k2 { get; set; }
        public override Double k3 { get; set; }
        public SquareEquation(double k1, double k2, double k3) {
            this.k1 = k1;
            this.k2 = k2;
            this.k3 = k3;
        }

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
            double d = Math.Pow(k2, 2) - 4 * k1 * k3;
            if (d > 0) number = 2;
            else if (d == 0) number = 1;
            else number = 0;
            D = d;
            rootsNumber = number;
            return number;
        }
        public void ApexCoordinates()
        {
            apex[0] = -k2 / (2 * k1);
            apex[1] = -D / (4 * k1);
        }
    }
}