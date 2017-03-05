using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Json;
using Android.Util;

namespace App5
{
    [Activity(Label = "WeatherREST", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            EditText lantitue = FindViewById<EditText>(Resource.Id.latText);
            EditText longitude = FindViewById<EditText>(Resource.Id.longText);
            Button button = FindViewById<Button>(Resource.Id.getWeatherButton);

            var tag = "myapp";
            Log.Info(tag, "this is an info message");
            Log.Warn(tag, "this is an warn message");
            Log.Error(tag, "this is an error message");

            button.Click += async (s, e) =>
            {
                string url = "http://api.geonames.org/findNearByWeatherJSON?lat=" +
                 lantitue.Text +
                 "&lng=" +
                 longitude.Text +
                 "&username=ysjr2002";

                JsonValue json = await FetchWeatherAsync(url);

                ParseAndDisplay(json);
            };


            Button subMain = FindViewById<Button>(Resource.Id.startSubMain);

            subMain.Click += (s, e) =>
            {
                Intent intent = new Intent(this, typeof(SubMainActivity));
                intent.PutExtra("MyData", "data from activity1");
                StartActivity(intent);
                //StartActivity(typeof(SubMainActivity));
            };

        }

        protected override void OnPause()
        {
            base.OnPause();
            Console.WriteLine("Main Activity被暂停?");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Console.WriteLine("Main Activity被销毁?");
        }

        private async Task<JsonValue> FetchWeatherAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    return jsonDoc;
                }
            }
        }

        private void ParseAndDisplay(JsonValue json)
        {
            // Get the weather reporting fields from the layout resource:
            TextView location = FindViewById<TextView>(Resource.Id.locationText);
            TextView temperature = FindViewById<TextView>(Resource.Id.tempText);
            TextView humidity = FindViewById<TextView>(Resource.Id.humidText);
            TextView conditions = FindViewById<TextView>(Resource.Id.condText);

            // Extract the array of name/value results for the field name "weatherObservation". 
            JsonValue weatherResults = json["weatherObservation"];
            // Extract the "stationName" (location string) and write it to the location TextBox:
            location.Text = weatherResults["stationName"];

            // The temperature is expressed in Celsius:
            double temp = weatherResults["temperature"];
            // Convert it to Fahrenheit:
            temp = ((9.0 / 5.0) * temp) + 32;
            // Write the temperature (one decimal place) to the temperature TextBox:
            temperature.Text = String.Format("{0:F1}", temp) + "° F";

            // Get the percent humidity and write it to the humidity TextBox:
            double humidPercent = weatherResults["humidity"];
            humidity.Text = humidPercent.ToString() + "%";

            // Get the "clouds" and "weatherConditions" strings and 
            // combine them. Ignore strings that are reported as "n/a":
            string cloudy = weatherResults["clouds"];
            if (cloudy.Equals("n/a"))
                cloudy = "";
            string cond = weatherResults["weatherCondition"];
            if (cond.Equals("n/a"))
                cond = "";

            // Write the result to the conditions TextBox:
            conditions.Text = cloudy + " " + cond;
        }
    }
}

