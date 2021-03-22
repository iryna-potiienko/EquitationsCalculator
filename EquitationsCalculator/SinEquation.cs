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
    class SinEquation: Equation
    {
        public override Double k1 { get; set; }
        public override Double k2 { get; set; }
        public override Double k3 { get; set; }
        public SinEquation(double k1, double k2, double k3)
        {
            this.k1 = k1;
            this.k2 = k2;
            this.k3 = k3;
        }
        private double sin(double x)
        {
            return Math.Sin(x);
        }
        private double cos(double x)
        {
            return Math.Cos(x);
        }
        public override double f(double x)
        {
            double ans = k1 * sin(x) * sin(x) + k2 * sin(x) + k3;
            return ans;
        }
        public override double f1Deriv(double x)
        {
            double ans = k1 * 2*sin(x)*cos(x) + k2*cos(x);
            return ans;
        }
        public override double f2Deriv(double x)
        {
            return -2 * k1 * sin(x) * sin(x) + 2 * k1 * cos(x) * cos(x) - k2 * sin(x);
        }
    }
}