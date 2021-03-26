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
    public class ExpEquation : Equation
    {
        public ExpEquation(double k1, double k2, double k3, double a, double b):base(k1,  k2,  k3,  a, b) { }
        private double exp(double x)
        {
            return Math.Exp(x);
        }
        public override double f(double x)
        {
            double ans = k1*exp (x*k2) +k3;
            return ans;
        }
        public override double f1Deriv(double x)
        {
            double ans = k1 * k2 * exp(x * k2);
            return ans;
        }
        public override double f2Deriv(double x)
        {
            return k2 * f1Deriv(x);
        }
    }
}