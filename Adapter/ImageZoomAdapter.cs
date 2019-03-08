using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Bumptech.Glide;
using File_Tracking.Model;

namespace File_Tracking.Adapter
{
    class ImageZoomAdapter : BaseAdapter<ModelClass>
    {

        Context context;
        List<ModelClass> users;

       int pos;

        FragmentManager fragManager;

        public ImageZoomAdapter(Context context, List<ModelClass> users1, int pos, FragmentManager fm)
        {
            users = new List<ModelClass>();
            this.context = context;
            users = users1;
            this.pos = pos;
            this.fragManager = fm;
        }

        public override ModelClass this[int position]
        {
            get
            {
                return users[position];
            }
        }


        public override int Count
        {
            get
            {
                return users.Count;
            }
        }

       

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ZoomLayout, parent, false);

                var photo = view.FindViewById<ImageView>(Resource.Id.zoomimage);

                photo.Click += delegate
                {
                    FragmentTransaction ft = fragManager.BeginTransaction();
                    DialogFragment newFragment = ImageDialog.newInstance();
                    Bundle bundle = new Bundle();
                    bundle.PutString("Photo", users[pos].ImagePath);
                    newFragment.Arguments = bundle;
                    newFragment.Show(ft, "dialog");


                    //Intent intent = new Intent(context,typeof(ImageDialog));
                    //intent.PutExtra("Photo", users[pos].FilePath);
                    //context.StartActivity(intent);
                };

                view.Tag = new ImageReportAdapterViewHolder() { Images = photo };
            }

            var holder = (ImageReportAdapterViewHolder)view.Tag;

            Glide.With(context).Load(users[pos].ImagePath).Into(holder.Images);
            // holder.Images.tex


            return view;
        }

        //fill in your items
        //holder.Title.Text = "new text here";




    }

    class ImageReportAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
        public ImageView Images { get; set; }
    }
}