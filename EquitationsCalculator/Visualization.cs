using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using System.IO;
using Android;
using Android.Content.PM;
using Com.Syncfusion.Charts;
using Android.Content;
using System.Collections.ObjectModel;
using Android.Graphics;
using System.Collections.Generic;

namespace EquitationsCalculator
{
    [Activity(Label = "Visualization")]
    public class Visualization : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDE1OTgzQDMxMzgyZTM0MmUzMFovQUh1STN4UVBoc0lsQkUvSGVEbmFXaFBUdVJHUzY5Yy83bnphSXgvK009");

            base.OnCreate(savedInstanceState);
           
            SfChart chart = new SfChart(this);
            chart.Title.Text = "Equation chart";
            chart.SetBackgroundColor(Color.White);

            //Initializing primary axis
            NumericalAxis primaryAxis = new NumericalAxis();
            chart.PrimaryAxis = primaryAxis;

            //Initializing secondary Axis
            NumericalAxis secondaryAxis = new NumericalAxis();
            chart.SecondaryAxis = secondaryAxis;

            Collection<ChartData> dt = ReadTabulation();

            ObservableCollection<ChartData> data = new ObservableCollection<ChartData>(); 
            foreach (var item in dt)
            {
                data.Add(new ChartData (item.X, item.Y) );
            }

            //Initializing column series
            LineSeries series = new LineSeries();
            series.ItemsSource = data;
            series.XBindingPath = "X";
            series.YBindingPath = "Y";
            
            series.TooltipEnabled = true;

            chart.Series.Add(series);
            SetContentView(chart);
        }
        public Collection<ChartData> ReadTabulation()
        {
            Collection<ChartData> dataList = new Collection<ChartData>();
            ChartData data;
            double x, y;

            var backingFile = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath, "results.txt");
            using (var reader = new StreamReader(backingFile, true))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] splitLine = line.Split(' ');

                    x = Convert.ToDouble(splitLine[0]);
                    y = Convert.ToDouble(splitLine[1]);
                    data = new ChartData(x, y);
                    dataList.Add(data);
                }
            }
            return dataList;
        }
    }
}