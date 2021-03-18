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
    public abstract class Equation
    {
        //Double k = 0, k2, k3, D;
        //public rootsNumber;
        public abstract Double k1 { get; set; }
        public abstract Double k2 { get; set; }
        public abstract Double k3 { get; set; }
        public abstract double f(double x);
        public abstract double f1Deriv(double x);
        public abstract double f2Deriv(double x);
    }
}