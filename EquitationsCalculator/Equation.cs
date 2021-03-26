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
    public abstract class Equation
    {
        public Equation(double k1, double k2, double k3, double a, double b)
        {
            this.k1 = k1;
            this.k2 = k2;
            this.k3 = k3;
            this.a = a;
            this.b = b;
            this.rootsNumber = RootsAmountCheck();
            CreateOneRootIntervals();
        }
        public Double k1 { get; set; }
        public Double k2 { get; set; }
        public Double k3 { get; set; }
        public Double a { get; set; }
        public Double b { get; set; }
        public int rootsNumber { get; set; }
        public List<List<double>> OneRootIntervals;
        public abstract double f(double x);
        public abstract double f1Deriv(double x);
        public abstract double f2Deriv(double x);

        public virtual int RootsAmountCheck()
        {
            double fa = f(a);
            double fb = f(b);
            if (fa *fb <= 0) return 1;
            else return 0;
        }
        public virtual void CreateOneRootIntervals()
        {
            var intervals = new List<List<double>>();
            if (rootsNumber == 1)
                intervals.Add(new List<double> { a, b });
            //if (rootsNumber == 0)
                //intervals.Add(new List<double> { });
            OneRootIntervals = intervals;
        }
    }
}