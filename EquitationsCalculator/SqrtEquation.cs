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
    class SqrtEquation : Equation
    {
        public SqrtEquation(double k1, double k2, double k3, double a, double b) : base(k1, k2, k3, a, b) { }
        private double sqrt(double x)
        {
            return Math.Sqrt(x);
        }
        public override double f(double x)
        {
            double ans = sqrt(x + k1) + k2 * Math.Pow(x, k3);
            return ans;
        }
        public override double f1Deriv(double x)
        {
            double ans = 1 / (2 * sqrt(x)) + k2 * k3 * Math.Pow(x, k3 - 1);
            return ans;
        }
        public override double f2Deriv(double x)
        {
            return -1 / (4 * Math.Pow(x, 3 / 2)) + k2 * k3 * (k3 - 1) * Math.Pow(x, k3 - 2);
        }
        public override int RootsAmountCheck()
        {
            if (a < k1) a = k1;
            if (a > b) return 0;
            return base.RootsAmountCheck();
        }
    }
}