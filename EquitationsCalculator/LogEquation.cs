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
    class LogEquation: Equation
    {
        public override Double k1 { get; set; }
        public override Double k2 { get; set; }
        public override Double k3 { get; set; }
        public LogEquation(double k1, double k2, double k3)
        {
            this.k1 = k1;
            this.k2 = k2;
            this.k3 = k3;
        }
        private double log(double x)
        {
            return Math.Log(x);
        }
        public override double f(double x)
        {
            double ans = k1*log (Math.Pow(x,k2)) +k3;
            return ans;
        }
        public override double f1Deriv(double x)
        {
            double ans = k1 * (k2/x);
            return ans;
        }
        public override double f2Deriv(double x)
        {
            return -k1 * k2 / Math.Pow(x, 2);
        }
    }
}