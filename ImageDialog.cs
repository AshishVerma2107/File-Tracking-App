using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Bumptech.Glide;

namespace File_Tracking
{
    class ImageDialog : DialogFragment
    {
        private ImageView imgView;
        string Images;
        static public ImageDialog newInstance()
        {
            ImageDialog f = new ImageDialog();
            return f;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetStyle(DialogFragmentStyle.NoTitle, Android.Resource.Style.ThemeBlackNoTitleBarFullScreen);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View v = inflater.Inflate(Resource.Layout.ZoomLayout, container, false);

            imgView = v.FindViewById<ImageView>(Resource.Id.zoomimage);


           // v.Tag = new ImageReportAdapterViewHolder() { Images = imgView };

            Images = Arguments.GetString("Photo");

           // var holder = (ImageReportAdapterViewHolder)v.Tag;

            Glide.With(Activity).Load(Images).Into(imgView);
      
            return v;
        }
    }

    //class ImageReportAdapterViewHolder : Java.Lang.Object
    //{
    //    //Your adapter views to re-use
    //    //public TextView Title { get; set; }
    //    public ImageView Images { get; set; }
    //}
}