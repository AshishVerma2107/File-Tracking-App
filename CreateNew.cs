using Android.OS;
using Android.Views;
using Android.App;
using Android.Widget;
using Android.Content;
using Android.Graphics;
using Android.Content.PM;
using Android.Provider;
using System;
using Android.Support.Design.Widget;
using File_Tracking.Database;
using File_Tracking.Adapter;

using Java.IO;
using Android.Util;
using File_Tracking.Model;
using Android.Content.Res;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace File_Tracking.Fragments
{
    public class CreateNew : Android.Support.V4.App.Fragment
    {
        InternetConnection con;
        DBHelper dba;
        BlobFileUpload blobUpload;
        Service service;
        Geolocation geo;
        EditText search;
        Button ScanQr, Submit;
        TextInputEditText Sender, Addressee, Subject, letterDate, Medium, ScanningDate1, SyncDate1, QrDetail;
        string OtherSender = "", OtherAddressee = "", OtherSubject = "", OtherDate = "", OtherMedium = "", OtherScanningDate = "", OtherSyncDate = "", OtherQrDetail = "", OtherImageLetter = "";
       
        GridView gridViewVer;
        ImageButton cameraVer;
        public static GridAdapter grid_adapter;
        public static File _file;
         string file_path;
        LinearLayout gridLayout;
        ProgressDialog progress;
       
        string imageType = "Printed";
        int counted = 10;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            con = new InternetConnection();
            dba = new DBHelper();

            geo = new Geolocation();
            service = new Service();
            blobUpload = new BlobFileUpload(Activity);
            progress = new ProgressDialog(Activity);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetCancelable(false);
            progress.SetMessage("Please wait...");

            

            // SaveCountAsync(counted);
            //  ReadCountAsync();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           base.OnCreateView(inflater, container, savedInstanceState);

            View v = inflater.Inflate(Resource.Layout.CreateNewLayout, container, false);

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());


            Utilities.imageList.Clear();

           // search = v.FindViewById<EditText>(Resource.Id.inputSearch);
            ScanQr = v.FindViewById<Button>(Resource.Id.scan);
          
            QrDetail = v.FindViewById<TextInputEditText>(Resource.Id.qrdetail);
            Sender = v.FindViewById<TextInputEditText>(Resource.Id.sender);
            Addressee = v.FindViewById<TextInputEditText>(Resource.Id.addressee);
            Subject = v.FindViewById<TextInputEditText>(Resource.Id.subject);
            letterDate = v.FindViewById<TextInputEditText>(Resource.Id.date);
            Medium = v.FindViewById<TextInputEditText>(Resource.Id.medium);
            ScanningDate1 = v.FindViewById<TextInputEditText>(Resource.Id.scanningdate);
            SyncDate1 = v.FindViewById<TextInputEditText>(Resource.Id.syncdate);
            Submit = v.FindViewById<Button>(Resource.Id.submit);


            gridLayout = v.FindViewById<LinearLayout>(Resource.Id.gridver);
            gridViewVer = v.FindViewById<GridView>(Resource.Id.gridview);
            grid_adapter = new GridAdapter(Activity, Utilities.imageList);
            gridViewVer.Adapter = grid_adapter;

            cameraVer = v.FindViewById<ImageButton>(Resource.Id.imageButton);

            cameraVer.Click += delegate
            {

                CameraPic();
            };

            letterDate.Click += (sender, e) =>
            {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(Activity, OnDateSet, today.Year, today.Month - 1, today.Day);

                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();
            };
            ScanningDate1.Click += (sender, e) =>
            {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(Activity, OnScanningDateSet, today.Year, today.Month - 1, today.Day);

                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();
            };
            SyncDate1.Click += (sender, e) =>
            {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(Activity, OnSyncDateSet, today.Year, today.Month - 1, today.Day);

                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();
            };

            //CameraLetter.Click += delegate
            //{
            //    CameraPic();
            //};

            ScanQr.Click += delegate
            {
                Intent intent = new Intent(Activity,typeof(BarCodeScannerActivity));

             
               StartActivity(intent);
               QrDetail.Text = BarCodeScannerActivity.barcodestring;

                OtherQrDetail = QrDetail.Text.ToString();
                QrDetail.Text = OtherQrDetail.ToString();

                

            };

           

            Submit.Click += delegate
             {
                 OtherSender = Sender.Text.ToString();
                 OtherAddressee = Addressee.Text.ToString();
                 OtherSubject = Subject.Text.ToString();
                 OtherDate = letterDate.Text.ToString();
                 OtherMedium = Medium.Text.ToString();
                 OtherScanningDate = ScanningDate1.Text.ToString();
                 OtherSyncDate = SyncDate1.Text.ToString();
                 OtherQrDetail = QrDetail.Text.ToString();
                 OtherImageLetter = Utilities.imageList[0].ImagePath;
                 
                 if (OtherQrDetail.Equals(""))
                 {
                     Toast.MakeText(Activity, "Please Click Scan QR Code for QR Value ", ToastLength.Short).Show();
                     return;
                 }

                 if (OtherSender.Equals(""))
                 {
                     Toast.MakeText(Activity, "Please Enter Sender Name", ToastLength.Short).Show();
                     return;
                 }
                 if (OtherAddressee.Equals(""))
                 {
                     Toast.MakeText(Activity, "Please Enter Addressee Name", ToastLength.Short).Show();
                     return;
                 }
                 if (OtherSubject.Equals(""))
                 {
                     Toast.MakeText(Activity, "Please Enter Subject", ToastLength.Short).Show();
                     return;
                 }
                 if (OtherDate.Equals(""))
                 {
                     Toast.MakeText(Activity, "Please Select Date", ToastLength.Short).Show();
                     return;
                 }
                 if (OtherMedium.Equals(""))
                 {
                     Toast.MakeText(Activity, "Please Enter Medium Name", ToastLength.Short).Show();
                     return;
                 }

                 if (OtherScanningDate.Equals(""))
                 {
                     Toast.MakeText(Activity, "Please Select Scanning Date", ToastLength.Short).Show();
                     return;
                 }
                 if (OtherSyncDate.Equals(""))
                 {
                     Toast.MakeText(Activity, "Please Select Sync Date", ToastLength.Short).Show();
                     return;
                 }
                 //if (OtherImageLetter.Equals(""))
                 //{
                 //    Toast.MakeText(Activity, "Please Click Letter Image", ToastLength.Short).Show();
                 //    return;
                 //}
                 if (Utilities.imageList.Count < 1)
                 {
                     Toast.MakeText(Activity, "Please Capture Atleast One Photograph.", ToastLength.Short).Show();
                     return;
                 }

                 getPrivateAlbumStorageDir(Activity, "AshishAlbum");


                 //var localContacts = Application.Context.GetSharedPreferences("MyContacts", FileCreationMode.Private);
                 //var contactEdit = localContacts.Edit();
                 //contactEdit.PutString("SenderName", OtherSender);
                 //contactEdit.PutString("AddresseeName", OtherAddressee);
                 //contactEdit.Commit();

                 if (con.connectivity())
                 {
                     SendToFileManagementServer();

                     //var demo = System.IO.Path.Combine(string.Format("file:///android_asset/pdfis/web/viewer.html?file={0}", string.Format("file:///android_asses/Content/{0}", Utilities.imageURL1)));


                     //FileOutputStream fileoutput = new FileOutputStream("AshishFolder");
                     //fileoutput.Write(OtherSender.Length);
                     //fileoutput.Write(OtherSubject.Length);
                     //fileoutput.Close();
                 }
                 else
                 {


                     //Submit.Enabled = true;
                     //PackageManager manager = Activity.PackageManager;
                     //PackageInfo info = manager.GetPackageInfo(Activity.PackageName, 0);
                     //info.VersionName.ToString();
                    // FileTrackingModel file = new FileTrackingModel();
                      //dba.insertLetterData(file);

                     Toast.MakeText(Activity, "Data saved in local storage.", ToastLength.Short).Show();
                     int i = dba.insertLetterData(OtherQrDetail, OtherSender, OtherAddressee, OtherSubject, OtherDate, OtherMedium, OtherScanningDate, OtherSyncDate, OtherImageLetter, "no");
                    // Toast.MakeText(Activity, "Data Saved in Database.", ToastLength.Long).Show();

                     //for (int j = 0; j < Utilities.imageList.Count; j++)
                     //{
                     //    dba.insertMediaData("LetterImage", i.ToString(), Utilities.imageList[j].ImagePath, Utilities.imageList[j].ImagePath.Substring(Utilities.imageList[j].ImagePath.LastIndexOf("/") + 1));
                     //}
                 }

                 //Submit.Enabled = true;
                 //Sender.Text = "";
                 //Addressee.Text = "";
                 //Subject.Text = "";
                 //letterDate.Text = "";
                 //Medium.Text = "";
                 //ScanningDate1.Text = "";
                 //SyncDate1.Text = "";
                 //QrDetail.Text = "";

                 //Utilities.imageList.Clear();

                 //Utilities.GlobalBusinessList.Clear();

                 //grid_adapter.NotifyDataSetChanged();






             };



            return v;
        }

        public File getPrivateAlbumStorageDir(Context context, String albumName)
        {
            // Get the directory for the app's private pictures directory.
            File file = new File(context.GetExternalFilesDir(
                    System.Environment.SystemDirectory), albumName);
            
            if (!file.Mkdirs())
            {
               // Log.e(LOG_TAG, "Directory not created");
            }
            return file;
        }

        //public async Task SaveCountAsync(int count)
        //{
        //    var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "count.txt");
        //    using (var writer = System.IO.File.CreateText(backingFile))
        //    {
        //        await writer.WriteLineAsync(count.ToString());

        //    }


        //}

        //public async Task<int> ReadCountAsync()
        //{
        //    var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "count.txt");

        //    if (backingFile == null || !System.IO.File.Exists(backingFile))
        //    {
        //        return 0;
        //    }

        //    var count = 0;
        //    using (var reader = new System.IO.StreamReader(backingFile, true))
        //    {
        //        string line;
        //        while ((line = await reader.ReadLineAsync()) != null)
        //        {
        //            if (int.TryParse(line, out var newcount))
        //            {
        //                count = newcount;
        //            }
        //        }
        //    }

        //    return count;
        //}



        public async Task SendToFileManagementServer()
        {

            progress.Show();
          //  byte[] pictByteArray = null;

            List<string> imageList = new List<string>();

            //for (int i = 0; i < Utilities.imageList.Count; i++)
            //{

            //    byte[] img = GetStreamFromFile(Utilities.imageList[i].ImagePath);
            //   var url = await blobUpload.UploadPhotoAsync(img, Utilities.imageList[i].ImagePath.Substring(Utilities.imageList[i].ImagePath.LastIndexOf("/") + 1), "TDFImages");

            //   if (url != null)
            //    {
            //        imageList.Add(url);
            //    }
            //}


            ModelClass model = new ModelClass();
            model.QrDetail = OtherQrDetail;
            model.SenderName = OtherSender;
            model.AddresseeName = OtherAddressee;
            model.Subject = OtherSubject;
            model.LetterDate = OtherDate;
            model.Medium = OtherMedium;
            model.ScanningDate = OtherScanningDate;
            model.SyncDate = OtherSyncDate;
            model.ImageLetter = Utilities.imageName1;
            model.datetime = DateTime.Now.ToString("yyyy-MM-dd");
            model.geolocation = geo.GetGeoLocation(Activity);
            model.ImagePath = Utilities.imageList[0].ImagePath;


           

            PackageManager manager = Activity.PackageManager;
            PackageInfo info = manager.GetPackageInfo(Activity.PackageName, 0);
            // model.versionName = info.VersionName.ToString();

            string dt = JsonConvert.SerializeObject(model);
            string response = "";
            try
            {
                response = await service.PostServiceMethod(Activity, "InsertFileTrackingDetails", dt);
                
            }
            catch (Exception ex)
            {

            }

            if (response.Contains("Success"))
            {
                Toast.MakeText(Activity, "Data Submitted To Server Successfully.", ToastLength.Long).Show();

                Sender.Text = "";
                Addressee.Text = "";
                Subject.Text = "";
                letterDate.Text = "";
                Medium.Text = "";
                ScanningDate1.Text = "";
                SyncDate1.Text = "";
                QrDetail.Text = "";

                Utilities.imageList.Clear();
                Utilities.GlobalBusinessList.Clear();
                grid_adapter.NotifyDataSetChanged();

                try
                {
                    int i = dba.insertLetterData(OtherQrDetail, OtherSender, OtherAddressee, OtherSubject, OtherDate, OtherMedium, OtherScanningDate, OtherSyncDate, OtherImageLetter, "no");


                    //for (int j = 0; j < imageList.Count; j++)
                    //{
                    //    dba.insertMediaData("LetterImage", i.ToString(), imageList[j], imageList[j].Substring(imageList[j].LastIndexOf("/") + 1));
                    //}
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                Toast.MakeText(Activity, "Something went wrong. Please try after sometime.", ToastLength.Long).Show();
                try
                {
                    int i = dba.insertLetterData(OtherQrDetail, OtherSender, OtherAddressee, OtherSubject, OtherDate, OtherMedium, OtherScanningDate, OtherSyncDate, OtherImageLetter, "yes");

                    //for (int j = 0; j < Utilities.imageList.Count; j++)
                    //{
                    //    dba.insertMediaData("LetterImage", i.ToString(), Utilities.imageList[j].ImagePath, Utilities.imageList[j].ImagePath.Substring(Utilities.imageList[j].ImagePath.LastIndexOf("/") + 1));
                    //}
                }
                catch (Exception ex)
                {

                }

            }
            progress.Dismiss();
        }

        public byte[] GetStreamFromFile(string filePath)
        {
            try
            {
                Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(filePath));
                byte[] byteArray = System.IO.File.ReadAllBytes(uri.Path);
                return byteArray;
            }
            catch (System.Exception e)
            {
                return null;
            }
        }



        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == 100 && resultCode == (int)Android.App.Result.Ok)
            {
              //  int height = Resources.DisplayMetrics.HeightPixels;
               // int width = Resources.DisplayMetrics.WidthPixels;

                Utilities.imageList.Add(new ImageListModel
                {


                    ImagePath = _file.AbsolutePath,
                    ImageName = Utilities.imageName1,
                    Status = "0",
                });
                grid_adapter.NotifyDataSetChanged();
            }
        }
        public void CameraPic()
        {
           // getPrivateAlbumStorageDir(Activity, "AshishAlbum");

            TakeAPicture();
           // OnConfigurationChanged(Android.Content.Res.Orientation.Portrait);
        }

        public void TakeAPicture()
        {





            try
            {
                File _dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "FileTracking");

                string file_path = _dir.ToString();
                if (!_dir.Exists())
                {
                    _dir.Mkdirs();
                }
                try
                {

                    string FileName = Utilities.fileName();
                    Utilities.imageName1 = FileName;
                    Intent intent = new Intent(MediaStore.ActionImageCapture);
                    _file = new File(_dir, string.Format(FileName, Guid.NewGuid()));
                    Utilities.imageURL1 = _file.AbsolutePath;
                    intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    StartActivityForResult(intent, 100);


                }
                catch (Exception e)
                {
                    Log.Error("Error In Capture", e.Message);
                }


            }
            catch (Exception e) { }

        }

        public override void OnResume()
        {
            base.OnResume();
            QrDetail.Text = BarCodeScannerActivity.barcodestring;

            OtherQrDetail = QrDetail.Text.ToString();
            QrDetail.Text = OtherQrDetail.ToString();
           
        }

        private void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {


            // invoicedate.Text = e.Date.ToLongDateString();
            letterDate.Text = e.Date.ToString("dd-MMM-yyyy");
            string date = e.Date.ToString("yyyy-MM-dd");

        }
        private void OnScanningDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {


            // invoicedate.Text = e.Date.ToLongDateString();
           
           
            ScanningDate1.Text = e.Date.ToString("dd-MMM-yyyy");
            string date1 = e.Date.ToString("yyyy-MM-dd");

        }
        private void OnSyncDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {



            SyncDate1.Text = e.Date.ToString("dd-MMM-yyyy");
            string date2 = e.Date.ToString("yyyy-MM-dd");

        }

      

        
    }
}




//<EditText
//        android:id="@+id/inputSearch"
//        android:layout_width="fill_parent"
//        android:layout_height="wrap_content"
//        android:hint="Search"
//        android:inputType="textVisiblePassword"
//        android:background="@android:color/white"
//        android:paddingBottom="10dp"
//        android:paddingLeft="5dp"
//        android:paddingRight="5dp"
//        android:paddingTop="10dp"
//		android:drawableRight="@drawable/search"
//        android:textColor="#000000" />