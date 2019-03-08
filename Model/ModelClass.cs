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
    class ModelClass
    {
        public int Id { get; set; }
        public string QrDetail { get; set; }
        public string SenderName { get; set; }
        public string AddresseeName { get; set; }
        public string Subject { get; set; }
       
        public string LetterDate { get; set; }
        public string Medium { get; set; }
        
        public string ScanningDate { get; set; }
       
        public string SyncDate { get; set; }
        public string ImageLetter { get; set; }
        public string ImagePath { get; set; }
        public string licenceId { get; set; }
        public string Created_by { get; set; }
        public DateTime Created_On { get; set; }
        public string CreatedIP { get; set; }
        public string versionName { get; set; }
        public string datetime { get; set; }
        public string geolocation { get; set; }
    }
}