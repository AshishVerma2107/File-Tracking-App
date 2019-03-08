using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Views;
using Android.Widget;
using Context = Android.Content.Context;

using File_Tracking.Model;
using Android.Graphics;
using System.Net;

namespace File_Tracking.Adapter
{

    class SenderListAdapter : BaseAdapter<ReceiverDetails>
    {
        List<ReceiverDetails> receiver;
        Context context;
        string img ;
        string CheckStatus = "";
        ImageView Image;
        public SenderListAdapter(Context mContext, List<ReceiverDetails> receiverList)
        {
            this.receiver = receiverList;
            this.context = mContext;
        }

        public override ReceiverDetails this[int position]
        {
            get
            {
                return receiver[position];
            }
        }

        public override int Count
        {
            get
            {
                return receiver.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

           
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SenderListDemo, null);

               TextView Department = view.FindViewById<TextView>(Resource.Id.depart);
               TextView Name = view.FindViewById<TextView>(Resource.Id.names);
               CheckBox Check = view.FindViewById<CheckBox>(Resource.Id.check);
                TextView Address = view.FindViewById<TextView>(Resource.Id.add);

           


                Department.Text = receiver[position].DesignationName;
                Name.Text = receiver[position].NPName;
                Address.Text = receiver[position].Address;

            if (Check.Selected)
            {
                CheckStatus = "Yes";
            }
            else
            {
                CheckStatus = "No";
            }



            Image = view.FindViewById<ImageView>(Resource.Id.photo);

             img = receiver[position].PhotoPath;



            

            var imageBitmap = GetImageBitmapFromUrl(img);

            
            Image.SetImageBitmap(imageBitmap);




            return view;

        }
        //public byte[] GetStreamFromFile(string filePath)
        //{
        //    try
        //    {
        //        Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(filePath));
        //        byte[] byteArray = System.IO.File.ReadAllBytes(uri.Path);
        //        return byteArray;
        //    }
        //    catch (System.Exception e)
        //    {
        //        return null;
        //    }
        //}

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}
