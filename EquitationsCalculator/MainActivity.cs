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

namespace EquitationsCalculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        string equationType;
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("The equation is {0}", spinner.GetItemAtPosition(e.Position));
            equationType = spinner.GetItemAtPosition(e.Position).ToString();
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.planets_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            // Get our UI controls from the loaded layout
            var aNumb = FindViewById<EditText>(Resource.Id.CoefficientA);
            var bNumb = FindViewById<EditText>(Resource.Id.CoefficientB);

            EditText k1Coef = FindViewById<EditText>(Resource.Id.Coef1);
            EditText k2Coef = FindViewById<EditText>(Resource.Id.Coef2);
            EditText k3Coef = FindViewById<EditText>(Resource.Id.Coef3);
            //EditText k4Coef = FindViewById<EditText>(Resource.Id.Coef4);

            EditText epsilon = FindViewById<EditText>(Resource.Id.Epsilon);
            Button calculateButton = FindViewById<Button>(Resource.Id.CalculateButton);
            Button saveButton = FindViewById<Button>(Resource.Id.SaveButton);
            Button readButton = FindViewById<Button>(Resource.Id.ReadButton);
            Button plotButton = FindViewById<Button>(Resource.Id.PlotButton);

            TextView dyhotomyResult = FindViewById<TextView>(Resource.Id.DyhotomyResult);
            TextView iterationsNumber = FindViewById<TextView>(Resource.Id.IterationsNumber);
            TextView modNewtonResult = FindViewById<TextView>(Resource.Id.Method2);
            TextView modNewtonIterationsNumber = FindViewById<TextView>(Resource.Id.Method2iter);
            TextView newtonResult = FindViewById<TextView>(Resource.Id.Method3);
            TextView newtonIterationsNumber = FindViewById<TextView>(Resource.Id.Method3iter);

            //PlotView view = FindViewById<PlotView>(Resource.Id.plot_view);
            //view.Model = CreatePlotModel();
            /*Steema.TeeChart.TChart tChart1 = new Steema.TeeChart.TChart(this);
            Steema.TeeChart.Styles.Bar bar1 = new Steema.TeeChart.Styles.Bar();
            tChart1.Series.Add(bar1);
            bar1.Add(3, "Pears", Color.Red);
            bar1.Add(4, "Apples", Color.Blue);
            bar1.Add(2, "Oranges", Color.Green);
            Steema.TeeChart.Themes.BlackIsBackTheme theme = new Steema.TeeChart.Themes.BlackIsBackTheme(tChart1.Chart);
            theme.Apply();
            SetContentView(tChart1);*/

            // Add code to translate number
            
            string dyhotomyResultNumber = string.Empty;
            string modNewtonResultNumber = string.Empty;
            string newtonResultNumber = string.Empty;
            Double a=1, b=2, k1=1, k2=2, k3=-6;
            Equation equation = new SquareEquation(k1, k2, k3);
            //Files filesW = new Files();

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

                if (equationType == "k1*x^2 +k2*x+k3 = 0") equation = new SquareEquation(k1, k2, k3);
                else if (equationType == "k1*sin(x)^2 +k2*sin(x)+k3 = 0") {
                    k3 = 0;
                    k3Coef.Text = "0";
                    equation = new SinEquation(k1, k2, k3);
                }
                if(equationType== "k1*ln(x^k2) + k3 = 0") equation = new LogEquation(k1, k2, k3);
                else equation = new SquareEquation(k1, k2, k3);

                dyhotomyResultNumber = Methods.Dyhotomy(a, b, equation).ToString();
                modNewtonResultNumber = Methods.ModNewton(a, b, equation).ToString();
                newtonResultNumber = Methods.Newton(a, b, equation).ToString();

                if (string.IsNullOrWhiteSpace(dyhotomyResultNumber))
                {
                    dyhotomyResult.Text = string.Empty;
                    modNewtonResult.Text = string.Empty;
                    newtonResult.Text = string.Empty;
                }
                else
                {
                    dyhotomyResult.Text = dyhotomyResultNumber;
                    iterationsNumber.Text += Methods.iterations;
                    modNewtonResult.Text = modNewtonResultNumber;
                    modNewtonIterationsNumber.Text = Methods.iterations.ToString();
                    newtonResult.Text = newtonResultNumber;
                    newtonIterationsNumber.Text = Methods.iterations.ToString();

                    //await filesW.SaveCountAsync(int count);
                }
            };
            saveButton.Click += async (sender, e) =>
            {
                //bool isReadonly = Android.OS.Environment.MediaMountedReadOnly.Equals(Android.OS.Environment.ExternalStorageState);
                //bool isWriteable = Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState);

                //CheckAppPermissions();
                var backingFile = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath, "results.txt");
                //var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures), "results.txt");
                //var backingFile = Path.Combine(Android.Content.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads), "results.txt");

                
                using (var writer = File.CreateText(backingFile))
                {
                    Double x = a, y;

                    do
                    {
                        y = equation.f(x);
                        await writer.WriteLineAsync(x + " " + y);
                        x = x + 0.1;
                    } while (x <= b);
                }
                Toast.MakeText(this, "Saved to file", ToastLength.Short).Show();
            };
            readButton.Click += (sender, e) =>
            {
                var backingFile = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath, "startData.txt");
                //var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures), "results.txt");

                if (backingFile == null || !File.Exists(backingFile))
                {
                    Toast.MakeText(this, "File not exist", ToastLength.Long).Show();
                }
                else
                {
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
            };
            plotButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(Visualization));
                //intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
                StartActivity(intent);
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void CheckAppPermissions()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }
            else
            {
                if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
                {
                    var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                    RequestPermissions(permissions, 1);
                }
            }
        }
    }
}