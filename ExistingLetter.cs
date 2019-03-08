using Android.OS;
using Android.Views;

using Android.Widget;

using File_Tracking.Database;

using System.Collections.Generic;
using File_Tracking.Model;
using Android.Support.Design.Widget;
using System;
using Android.Preferences;
using System.Dynamic;
using Newtonsoft.Json;
using System.Json;
using Android.Content;
using System.Threading.Tasks;
using File_Tracking.Adapter;
using System.Linq;
using Android.Graphics;
using Android.Support.V4.App;


namespace File_Tracking.Fragments
{
    public class ExistingLetter : Fragment
    {
        InternetConnection con;
        BlobFileUpload blobUpload;
      
        Geolocation geo;
        ListView LetterList;
        DBHelper dba;
        Service restService;

        ExistingLetterAdapter adapter;
        TextInputEditText search;
     
        ISharedPreferences prefs;
       
        List<ModelClass> result;

        List<FileTrackingModel> LocalDBResult;

        // List<MediaFileModel> imageresult;
        List<ImageListModel> imglistmodel;
        public static GridAdapter grid_adapter;
        public FragmentManager fragManager;

        public Context context;
        TextInputEditText qrDetails, senderName, addresseeName, subject, letterDate, medium, scanningDate, syncDate;
        ImageView imageLetter;
        Button Send;
        List<FileTrackingModel> letterLists = new List<FileTrackingModel>();
        string imgLetter = "";
        List<ModelClass> modelList = new List<ModelClass>();
        ImageZoomAdapter imgadap;
        int position;

        public ExistingLetter(FragmentManager fm)
        {

            this.fragManager = fm;
        }

        

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            con = new InternetConnection();
            dba = new DBHelper();

            geo = new Geolocation();
            restService = new Service();
            blobUpload = new BlobFileUpload(Activity);
           
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            View view = inflater.Inflate(Resource.Layout.ExistingLetterLayoutDemo, container, false);

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());
            dba = new DBHelper();





            search = view.FindViewById<TextInputEditText>(Resource.Id.searchQr);
            qrDetails = view.FindViewById<TextInputEditText>(Resource.Id.qrdetail);
            senderName = view.FindViewById<TextInputEditText>(Resource.Id.sender);
            addresseeName = view.FindViewById<TextInputEditText>(Resource.Id.addressee);
            subject = view.FindViewById<TextInputEditText>(Resource.Id.subject);
            letterDate = view.FindViewById<TextInputEditText>(Resource.Id.DateLetter);
            medium = view.FindViewById<TextInputEditText>(Resource.Id.medium);
            scanningDate = view.FindViewById<TextInputEditText>(Resource.Id.datescanning);
            syncDate = view.FindViewById<TextInputEditText>(Resource.Id.syncdate);
            imageLetter = view.FindViewById<ImageView>(Resource.Id.letterimage);
            Send = view.FindViewById<Button>(Resource.Id.send);

            qrDetails.Visibility = ViewStates.Gone;
            senderName.Visibility = ViewStates.Gone;
            addresseeName.Visibility = ViewStates.Gone;
            subject.Visibility = ViewStates.Gone;
            letterDate.Visibility = ViewStates.Gone;
            medium.Visibility = ViewStates.Gone;
            scanningDate.Visibility = ViewStates.Gone;
            syncDate.Visibility = ViewStates.Gone;
            imageLetter.Visibility = ViewStates.Gone;

            imageLetter.Click += delegate

            {
                FragmentTransaction ft = fragManager.BeginTransaction();
                DialogFragment newFragment = ImageDialog.newInstance();
                Bundle bundle = new Bundle();
                // bundle.PutString("Photo", imglistmodel[0].ImagePath);
                bundle.PutString("Photo", imgLetter);
                newFragment.Arguments = bundle;
                newFragment.Show(ft, "dialog");


                // ImageAdapter imageadp = new ImageZoomAdapter(Activity, ModelClass.ImagePath, position, FragmentManager);



                //FragmentTransaction transcation = FragmentManager.BeginTransaction();
                //DialogFragment imgdia = new ImageDialog(FragmentManager, modelList);
                //imgdia.Show(transcation, "Dialog Fragment");



                //FragmentTransaction ft = fragManager.BeginTransaction();

                // DialogFragment newFragment = ImageDialog.newInstance();

                //  DialogFragment newFragment = new ImageZoomAdapter(context, modelList );
               // Bundle bundle = new Bundle();
                // bundle.PutString("Photo", imgLetter);

                //bundle.PutString("Photo", result[0].ImagePath);

                //newFragment.Arguments = bundle;
                //newFragment.Show(ft, "dialog");



                //BitmapFactory.Options bmOptions = new BitmapFactory.Options();
                //bmOptions.InJustDecodeBounds = false;

                //bmOptions.InPurgeable = true;
                //Bitmap bitmap = BitmapFactory.DecodeFile(result[0].ImagePath, bmOptions);

                //imageLetter.SetImageBitmap(bitmap);

                //FragmentTransaction transcation1 = FragmentManager.BeginTransaction();
                //DialogFragment zoom = new ZoomFragment(FragmentManager);
                // zoom.Show(transcation1, "Dialog Fragment");

            };

           // Send.Click += delegate

            Send.Click += (object sender, EventArgs args) =>
            {

                Send.Enabled = false;

                FragmentTransaction transcation = FragmentManager.BeginTransaction();
                DialogFragment receiverName = new ReceiverListFragment(FragmentManager);
                receiverName.Show(transcation, "Dialog Fragment");



                // FragmentManager.BeginTransaction()
                //  .Replace(Resource.Id.container, receiverName)
                // .Commit();

                //Intent intent = new Intent(Activity, typeof(MessageOnMobileActivity));


                //StartActivity(intent);
                Send.Enabled = true;

            };
            

            search.TextChanged += delegate
            {


                if (search.Text.ToString().Length >= 12)
                {
                    string searchText = search.Text.ToString();
                    
                        LetterSearch(searchText);
                 

                }
                else
                {
                    
                }

                };



                return view;
        }

       

        private void LetterSearch(String search_string)
        {
           // string qrnumber = "", sendername = "", addresseename = "", sub = "", letterdate = "", medi = "", scandate = "", syncdate = "";

            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            string licenceid = prefs.GetString("LicenceId", "");
            string liceId = prefs.GetString("LicenceId", "");
            string desigId = prefs.GetString("DesignationId", "");

            //dynamic value = new ExpandoObject();
           // value.searchkey = search_string;
           // string json = JsonConvert.SerializeObject(value);

            if (con.connectivity())
            {
                FetchToFileManagementServer(search_string).Wait();
            }

            


                //try
                //{


                //      LocalDBResult = dba.getQrSurveyDetail(search_string);
                //    //  JsonValue item = await database.GetServiceMethod(Activity, "GetGSTINData", json).ConfigureAwait(false);
                //    // result = JsonConvert.DeserializeObject<List<FileTrackingModel>>(item);

                //    Activity.RunOnUiThread(() =>
                //    {
                //        if (result != null)
                //        {
                //            for (int i = 0; i < result.Count; i++)
                //            {

                //                qrnumber = result[i].QrDetail;
                //                sendername = result[i].SenderName;
                //                addresseename = result[i].AddresseeName;
                //                sub = result[i].Subject;
                //                letterdate = result[i].LetterDate;
                //                medi = result[i].Medium;
                //                scandate = result[i].ScanningDate;
                //                syncdate = result[i].SyncDate;
                //                imgLetter = result[i].ImageLetter;


                //            }
                //            if (result != null)
                //            {
                //                try
                //                {
                //                    qrDetails.Visibility = ViewStates.Visible;
                //                    senderName.Visibility = ViewStates.Visible;
                //                    addresseeName.Visibility = ViewStates.Visible;
                //                    subject.Visibility = ViewStates.Visible;
                //                    letterDate.Visibility = ViewStates.Visible;
                //                    medium.Visibility = ViewStates.Visible;
                //                    scanningDate.Visibility = ViewStates.Visible;
                //                    syncDate.Visibility = ViewStates.Visible;
                //                    imageLetter.Visibility = ViewStates.Visible;

                //                    qrDetails.Text = qrnumber;
                //                    senderName.Text = sendername;
                //                    addresseeName.Text = addresseename;
                //                    subject.Text = sub;
                //                    medium.Text = medi;
                //                    letterDate.Text = letterdate;
                //                    scanningDate.Text = scandate;
                //                    syncDate.Text = syncdate;

                //                    BitmapFactory.Options bmOptions = new BitmapFactory.Options();
                //                    bmOptions.InJustDecodeBounds = false;

                //                    bmOptions.InPurgeable = true;
                //                    Bitmap bitmap = BitmapFactory.DecodeFile(imgLetter, bmOptions);

                //                    imageLetter.SetImageBitmap(bitmap);


                //                }
                //                catch (Exception e)
                //                {

                //                }
                //            }
                //        }
                //    });

                //}

                //catch (Exception e)
                //{

                //}

            

        }

        private async Task FetchToFileManagementServer(string search_string)
        {
            string qrnumber = "", sendername = "", addresseename = "", sub = "", letterdate = "", medi = "", scandate = "", syncdate = "";
            try
            {
                dynamic value = new ExpandoObject();
                value.QrDetail = search_string;
                string json = JsonConvert.SerializeObject(value);
                JsonValue item = await restService.GetServiceMethod(Activity, "GetFileTrackingDetails", json).ConfigureAwait(false);
                // result = JsonConvert.DeserializeObject<List<FileTrackingModel>>(item);
                result = JsonConvert.DeserializeObject<List<ModelClass>>(item);

                

                    Activity.RunOnUiThread(() =>
                {
                    if (result != null)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            qrnumber = result[i].QrDetail;
                            sendername = result[i].SenderName;
                            addresseename = result[i].AddresseeName;
                            sub = result[i].Subject;
                            letterdate = result[i].LetterDate;
                            medi = result[i].Medium;
                            scandate = result[i].ScanningDate;
                            syncdate = result[i].SyncDate;
                            imgLetter = result[i].ImagePath;


                                                qrDetails.Visibility = ViewStates.Visible;
                                                senderName.Visibility = ViewStates.Visible;
                                                addresseeName.Visibility = ViewStates.Visible;
                                                subject.Visibility = ViewStates.Visible;
                                                letterDate.Visibility = ViewStates.Visible;
                                                medium.Visibility = ViewStates.Visible;
                                                scanningDate.Visibility = ViewStates.Visible;
                                                syncDate.Visibility = ViewStates.Visible;
                                                imageLetter.Visibility = ViewStates.Visible;

                            qrDetails.Text = qrnumber;
                            senderName.Text = sendername;
                            addresseeName.Text = addresseename;
                            subject.Text = sub;
                            medium.Text = medi;
                            letterDate.Text = letterdate;
                            scanningDate.Text = scandate;
                            syncDate.Text = syncdate;

                            BitmapFactory.Options bmOptions = new BitmapFactory.Options();
                            bmOptions.InJustDecodeBounds = false;

                            bmOptions.InPurgeable = true;
                            Bitmap bitmap = BitmapFactory.DecodeFile(imgLetter, bmOptions);

                            imageLetter.SetImageBitmap(bitmap);
                        }
                    }
                });
            }
            catch (Exception e)
            {

            }
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
    }


}








    //<File_Tracking.ScaleImageView
    //        android:id="@+id/letterimage"
    //        android:layout_width="400dp"
    //        android:layout_height="200dp">
    //         </File_Tracking.ScaleImageView>