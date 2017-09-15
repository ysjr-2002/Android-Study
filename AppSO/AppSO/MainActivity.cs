using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Runtime.InteropServices;
using SQLite;
using AppSO.data;
using Android.Util;
using System.Threading.Tasks;
using Android.Net;
using ExcelDataReader;
using Android.Database;
using Android.Provider;

namespace AppSO
{
    [Activity(Label = "AppSO", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        string dbRoot = "";
        string dbPath = "";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            //dbRoot = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "mydata");
            //dbPath = System.IO.Path.Combine(dbRoot, "mls.db");

            dbRoot = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "CanFindLocation");
            System.IO.Directory.CreateDirectory(dbRoot);

            dbPath = System.IO.Path.Combine(dbRoot, "mls.db");

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate
            {
                var id = getpid();
                dialog(id.ToString());

                var str = getString();
                dialog(str);
            };

            Button btnCreate = FindViewById<Button>(Resource.Id.Create);
            btnCreate.Click += delegate
            {
                var result = createDatabase(dbPath);
                if (System.IO.File.Exists(dbPath))
                {
                    dialog("创建成功");
                }
                else
                    dialog("创建失败");
            };

            Button btnInsert = FindViewById<Button>(Resource.Id.Insert);
            btnInsert.Click += Button2_Click;

            Button btnSearch = FindViewById<Button>(Resource.Id.Search);
            btnSearch.Click += Button3_Click;

            Button btnClear = FindViewById<Button>(Resource.Id.Clear);
            btnClear.Click += async delegate
           {
               var connection = new SQLiteAsyncConnection(dbPath);
               {
                   var count = await connection.ExecuteAsync("delete from person");
                   dialog("delete count->" + count);
               };
           };


            Button btnFind = FindViewById<Button>(Resource.Id.Find);
            btnFind.Click += async delegate
            {
                var connection = new SQLiteAsyncConnection(dbPath);
                {
                    var list = await connection.QueryAsync<Person>("select * from person where Name='杨绍杰'");
                    if (list == null)
                    {
                        dialog("查询失败！");
                        return;
                    }
                    dialog("find count->" + list.Count);
                    var person = list[0];
                    dialog(person.Name);
                };
            };

            Button btnImport = FindViewById<Button>(Resource.Id.Import);
            btnImport.Click += btnImport_click;
        }

        private void btnImport_click(object sender, System.EventArgs e)
        {
            showFileChooser();
        }

        private static int FILE_SELECT_CODE = 0;
        private void showFileChooser()
        {
            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("*/*");
            intent.AddCategory(Intent.CategoryOpenable);
            try
            {
                StartActivityForResult(Intent.CreateChooser(intent, "Select a File to Upload"), FILE_SELECT_CODE);
            }
            catch (Android.Content.ActivityNotFoundException ex)
            {
                // Potentially direct the user to the Market with a Dialog 
                Toast.MakeText(this, "Please install a File Manager.", ToastLength.Short).Show();
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == FILE_SELECT_CODE)
            {
                if (resultCode == Result.Ok)
                {
                    Uri uri = data.Data;
                    doImport(uri);
                }
                base.OnActivityResult(requestCode, resultCode, data);
            }
        }

        Handler handle = null;

        void change()
        {

        }

        private string GetPathToImage(Android.Net.Uri uri)
        {
            ICursor cursor = this.ContentResolver.Query(uri, null, null, null, null);
            cursor.MoveToFirst();
            string document_id = cursor.GetString(0);
            document_id = document_id.Split(':')[1];
            cursor.Close();

            cursor = ContentResolver.Query(
            Android.Provider.MediaStore.Images.Media.ExternalContentUri,
            null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new string[] { document_id }, null);
            cursor.MoveToFirst();
            string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();

            return path;
        }

        private async void doImport(Uri uri)
        {
            //handle = new Handler((m) =>
            //{
            //    if (m.What == 100)
            //    {

            //    }
            //});

            //var selectPath = uri.Path;
            var selectPath = GetPathToImage(uri);
            if (System.IO.File.Exists(selectPath))
            {
                var ext = System.IO.Path.GetExtension(selectPath).ToLower();
                if (ext != ".xls" || ext != "xlsx")
                {
                    dialog("文件格式不支持！");
                    handle.SendMessage(new Message { What = 100 });
                    return;
                }
            }
            else
            {
                return;
            }

            ProgressDialog pd = ProgressDialog.Show(this, "提示", "导入中，请稍等...");
            pd.SetProgressStyle(ProgressDialogStyle.Spinner);
            pd.Max = 1000;
            pd.Show();

            await Task.Factory.StartNew(() =>
            {
                var dir = System.IO.Path.GetDirectoryName(selectPath);
                var stream = System.IO.File.Open(selectPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read();
                    while (reader.Read())
                    {
                        var name = reader.GetString(0);
                        var code = reader.GetString(1);
                        var photo = reader.GetString(2);
                        Person p = new Person
                        {
                            Name = name,
                            Code = code,
                            Photo = photo
                        };
                        var sourceFile = System.IO.Path.Combine(dir, photo);
                        var densityFile = System.IO.Path.Combine(dbRoot, photo);
                        System.IO.File.Copy(sourceFile, densityFile, true);
                        Insert(p);
                    }
                }
                //dialog("导入成功！");
            });
            pd.Dismiss();
        }

        private void dialog(string str)
        {
            Toast.MakeText(this, str, ToastLength.Short).Show();
        }

        private async void Button3_Click(object sender, System.EventArgs e)
        {
            var connection = new SQLiteAsyncConnection(dbPath);
            {
                var count = await connection.ExecuteScalarAsync<int>("select count(*) from person");
                dialog("row count->" + count.ToString());
            }
        }

        private async void Button2_Click(object sender, System.EventArgs e)
        {
            await Insert(new Person { Name = "杨绍杰", Code = "123", Photo = "ysj.jpg" });
            Toast.MakeText(this, "结束", ToastLength.Short).Show();
        }

        private async Task Insert(Person person)
        {
            var connection = new SQLiteAsyncConnection(dbPath);
            {
                var count = await connection.InsertAsync(person);
            }
        }

        private string createDatabase(string path)
        {
            try
            {


                var connection = new SQLiteAsyncConnection(path);
                {
                    connection.CreateTableAsync<Person>();
                    return "Database created";
                }
                ////System.IO.File.Copy(path, "c:\\mls.db");
                //System.IO.File.Copy(@"C:\Users\ysj\Desktop\image\son.jpg", System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "tt.jpg"));
                //return string.Empty;
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        [DllImport("libc.so")]
        private static extern int getpid();


        [DllImport("JniTest", EntryPoint = "Java_slf_ly_testso_NDKTest_getString")]
        private static extern string getString();


    }
}

