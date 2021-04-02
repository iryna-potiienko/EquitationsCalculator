using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using System.IO;
using Android;
using Android.Content.PM;
using Android.Content;

using Android.Views;

namespace EquitationsCalculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        string equationType;
        Equation equation;
        TextView dyhotomyResult, modNewtonResult, newtonResult, errorInterval;
        TextView dyhotomyIterationsNumber, modNewtonIterationsNumber, newtonIterationsNumber;
        EditText aNumb, bNumb, epsilon, k1Coef, k2Coef, k3Coef;
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("The equation is {0}", spinner.GetItemAtPosition(e.Position));
            equationType = spinner.GetItemAtPosition(e.Position).ToString();
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
                ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }
        public void Info()
        {
            var intent = new Intent(this, typeof(AboutMe));
            StartActivity(intent);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Equation Calculator";
            toolbar.InflateMenu(Resource.Menu.top_menus);
            toolbar.MenuItemClick += (sender, e) => Info();

            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.planets_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            // Get our UI controls from the loaded layout
            aNumb = FindViewById<EditText>(Resource.Id.CoefficientA);
            bNumb = FindViewById<EditText>(Resource.Id.CoefficientB);
            epsilon = FindViewById<EditText>(Resource.Id.Epsilon);

            k1Coef = FindViewById<EditText>(Resource.Id.Coef1);
            k2Coef = FindViewById<EditText>(Resource.Id.Coef2);
            k3Coef = FindViewById<EditText>(Resource.Id.Coef3);

            errorInterval = FindViewById<TextView>(Resource.Id.ErrorInterval);
            
            Button calculateButton = FindViewById<Button>(Resource.Id.CalculateButton);
            Button readButton = FindViewById<Button>(Resource.Id.ReadButton);
            Button plotButton = FindViewById<Button>(Resource.Id.PlotButton);
            Button clearAllButton = FindViewById<Button>(Resource.Id.ClearAllButton);

            dyhotomyResult = FindViewById<TextView>(Resource.Id.DyhotomyResult);
            dyhotomyIterationsNumber = FindViewById<TextView>(Resource.Id.DyhotomyIterationsNumber);
            modNewtonResult = FindViewById<TextView>(Resource.Id.Method2);
            modNewtonIterationsNumber = FindViewById<TextView>(Resource.Id.ModNewtonIterationsNumber);
            newtonResult = FindViewById<TextView>(Resource.Id.Method3);
            newtonIterationsNumber = FindViewById<TextView>(Resource.Id.NewtonIterationsNumber);

            calculateButton.Click += (sender, e) => Calculate();
            readButton.Click += (sender, e) => ReadFile();
            clearAllButton.Click += (sender, e) => ClearAll();
            plotButton.Click += async (sender, e) =>
            {
                if (EquationInitialize())
                {
                    await TabulateAsync();
                    var intent = new Intent(this, typeof(Visualization));
                    StartActivity(intent);
                }
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void Calculate()
        {
            
            ClearResults();

            if (EquationInitialize())
            {
                Methods methods = new Methods();
                methods.GetResult(equation);

                dyhotomyResult.Text = Methods.StringResult(methods.DyhotomyRoots);
                dyhotomyIterationsNumber.Text += methods.DyhotomyIterations;
                modNewtonResult.Text = Methods.StringResult(methods.ModNewtonRoots);
                modNewtonIterationsNumber.Text += methods.ModNewtonIterations;
                newtonResult.Text = Methods.StringResult(methods.NewtonRoots);
                newtonIterationsNumber.Text += methods.NewtonIterations;
            }
        }
        public bool EquationInitialize()
        {
            Double a, b, k1, k2, k3;

            Methods.epsilon = Convert.ToDouble(epsilon.Text);
            a = Convert.ToDouble(aNumb.Text);
            b = Convert.ToDouble(bNumb.Text);
            k1 = Convert.ToDouble(k1Coef.Text);
            k2 = Convert.ToDouble(k2Coef.Text);
            k3 = Convert.ToDouble(k3Coef.Text);

            if (a >= b)
            {
                errorInterval.Text = "Incorrect interval!";
                return false;
            }
            else
            {
                if (equationType == "sqrt(x+k1) + k2*x^k3 = 0") equation = new SqrtEquation(k1, k2, k3, a, b);
                else if (equationType == "k1*exp(x*k2) + k3 = 0") equation = new ExpEquation(k1, k2, k3, a, b);
                else equation = new SquareEquation(k1, k2, k3, a, b);
                return true;
            }
        }
        public async System.Threading.Tasks.Task TabulateAsync()
        {
            bool isWriteable = Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState);

            if (isWriteable)
            {
                Double x = equation.a, y;
                var backingFile = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath, "results.txt");

                using (var writer = File.CreateText(backingFile))
                {
                    do
                    {
                        y = equation.f(x);
                        await writer.WriteLineAsync(x + " " + y);
                        x = x + 0.1;
                    } while (x <= equation.b);
                }
                Toast.MakeText(this, "Saved to file", ToastLength.Short).Show();
            }
            else Toast.MakeText(this, "No access to External Memory", ToastLength.Short).Show();
        }
        public void ReadFile()
        {
            bool isReadonly = Android.OS.Environment.MediaMountedReadOnly.Equals(Android.OS.Environment.ExternalStorageState);
            bool isWriteable = Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState);

            if (isReadonly || isWriteable)
            {
                var backingFile = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath, "startData.txt");

                if (backingFile == null || !File.Exists(backingFile))
                {
                    Toast.MakeText(this, "File not exist", ToastLength.Long).Show();
                }
                else
                {
                    ClearAll();
                    Toast.MakeText(this, "Information from file", ToastLength.Short).Show();
                    using (var reader = new StreamReader(backingFile, true))
                    {
                        aNumb.Text = reader.ReadLine();
                        bNumb.Text = reader.ReadLine();
                        epsilon.Text = reader.ReadLine();
                        k1Coef.Text = reader.ReadLine();
                        k2Coef.Text = reader.ReadLine();
                        k3Coef.Text = reader.ReadLine();
                    }
                }
            }
            else Toast.MakeText(this, "No access to External Memory", ToastLength.Short).Show();
        }
        public void ClearResults()
        {
            dyhotomyResult.Text = string.Empty;
            modNewtonResult.Text = string.Empty;
            newtonResult.Text = string.Empty;
            dyhotomyIterationsNumber.Text = string.Empty;
            modNewtonIterationsNumber.Text = string.Empty;
            newtonIterationsNumber.Text = string.Empty;
            errorInterval.Text = string.Empty;
        }
        public void ClearAll()
        {
            aNumb.Text = string.Empty;
            bNumb.Text = string.Empty;
            epsilon.Text = string.Empty;
            k1Coef.Text = string.Empty;
            k2Coef.Text = string.Empty;
            k3Coef.Text = string.Empty;
            ClearResults();
        }
    }
}