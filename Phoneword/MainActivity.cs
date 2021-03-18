using Android.App;
using Android.OS;
using Android.Widget;
using Core;
using System;

namespace Phoneword
{
    [Activity(Label = "Equation Calculator", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our UI controls from the loaded layout
            var aNumb = FindViewById<EditText>(Resource.Id.CoefficientA);
            var bNumb = FindViewById<EditText>(Resource.Id.CoefficientB);

            EditText k1Coef = FindViewById<EditText>(Resource.Id.Coef1);
            EditText k2Coef = FindViewById<EditText>(Resource.Id.Coef2);
            EditText k3Coef = FindViewById<EditText>(Resource.Id.Coef3);
            //EditText k4Coef = FindViewById<EditText>(Resource.Id.Coef4);

            EditText epsilon = FindViewById<EditText>(Resource.Id.Epsilon);
            Button calculateButton = FindViewById<Button>(Resource.Id.CalculateButton);
			TextView dyhotomyResult = FindViewById<TextView>(Resource.Id.DyhotomyResult);
            TextView iterationsNumber = FindViewById<TextView>(Resource.Id.IterationsNumber);

			// Add code to translate number
			string dyhotomyResultNumber = string.Empty;
            Double a, b, k1, k2, k3;

            calculateButton.Click += (sender, e) =>
            {
                //Methods equation = new Methods();
                iterationsNumber.Text = "Number of iterations: ";
                Methods.epsilon = Convert.ToDouble(epsilon.Text);
                a = Convert.ToDouble(aNumb.Text);
                b = Convert.ToDouble(bNumb.Text);
                k1 = Convert.ToDouble(k1Coef.Text);
                k2 = Convert.ToDouble(k2Coef.Text);
                k3 = Convert.ToDouble(k3Coef.Text);
                //k4 = Convert.ToDouble(k4Coef.Text);

                Equation equation = new Equation(k1, k2, k3);
                dyhotomyResultNumber = Methods.Dyhotomy(a, b, equation).ToString();
                if (string.IsNullOrWhiteSpace(dyhotomyResultNumber))
                {
                    dyhotomyResult.Text = string.Empty;
                }   
                else
                {
                    dyhotomyResult.Text = dyhotomyResultNumber;
                    iterationsNumber.Text += Methods.iterations;
                }
            };
        }
    }
}