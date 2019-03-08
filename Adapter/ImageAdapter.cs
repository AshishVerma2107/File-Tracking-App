using Android.Content;
using Android.Views;
using Android.Widget;

namespace File_Tracking.Adapter
{
    public class ImageAdapter : BaseAdapter
    {
        Context context;
        public ImageAdapter(Context c)
        {
            context = c;
        }
        public override int Count
        {
            get
            {
                return thumbIds.Length;
            }
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
       
        // create a new ImageView for each item referenced by the Adapter   
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView imageView;
            if (convertView == null)
            { // if it's not recycled, initialize some attributes   
                imageView = new ImageView(context);
                
                imageView.LayoutParameters = new GridView.LayoutParams(100, 100);
                imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                imageView.SetPadding(10, 10, 10, 10);
            }
            else
            {
                imageView = (ImageView)convertView;
            }
           // imageView.SetImageResource(thumbIds[position]);
            return imageView;
        }
        // references to our images   
        int[] thumbIds = {
        Resource.Drawable.image4,
       

    };
        
    }
    
}