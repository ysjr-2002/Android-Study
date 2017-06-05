using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using System.IO;

namespace FaceVisualExt
{
    class Log : Activity
    {
        int count = 0;
        string path = "";
        string filename = "";
        string Filename = "count.txt";
        private  async void Test()
        {
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            filename = Path.Combine(path, Filename);
            await loadFileAsync();
        }

        async Task<int> loadFileAsync()
        {
            if (File.Exists(filename))
            {
                using (var f = new StreamReader(OpenFileInput(Filename)))
                {
                    string line;
                    do
                    {
                        line = await f.ReadLineAsync();
                    } while (!f.EndOfStream);
                    Console.WriteLine("Load Finished");
                    return int.Parse(line);
                }
            }
            return 0;
        }

        async Task writeFileAsync()
        {
            using (var f = new StreamWriter(OpenFileOutput(Filename, FileCreationMode.Append | FileCreationMode.WorldReadable)))
            {
                await f.WriteLineAsync(count.ToString()).ConfigureAwait(false);
            }
            Console.WriteLine("Save Finished!");
        }
    }
}