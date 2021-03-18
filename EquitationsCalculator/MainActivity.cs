using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;

namespace EquitationsCalculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

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

            //PlotView view = FindViewById<PlotView>(Resource.Id.plot_view);
            //view.Model = CreatePlotModel();

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
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}