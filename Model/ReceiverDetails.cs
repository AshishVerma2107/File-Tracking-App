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

namespace File_Tracking.Model
{
    class ReceiverDetails
    {

        public string NPID { get; set; }
        public string NPName { get; set; }

        public string DesignationName { get; set; }

        public string PhotoPath { get; set; }
        public string Address { get; set; }

    }
}