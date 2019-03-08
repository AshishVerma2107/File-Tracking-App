using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace File_Tracking.Fragments
{
    public class ZoomFragment : DialogFragment
    {
        public FragmentManager fragManager;
       

        public ZoomFragment(FragmentManager fm)
        {
            this.fragManager = fm;
        }

        

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.ZoomLayout, container, false);

            return view;
        }
    }
}