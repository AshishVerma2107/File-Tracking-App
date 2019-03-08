using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using File_Tracking.Database;
using File_Tracking.Model;

namespace File_Tracking.Adapter
{
   public class ExistingLetterAdapter : BaseAdapter<FileTrackingModel>
    {
        List<FileTrackingModel> filetrackingModel;
        Context context;

        DBHelper dba;
       
        public ExistingLetterAdapter(Context mContext, List<FileTrackingModel> fileModel)
        {
            this.filetrackingModel = fileModel;
            this.context = mContext;
        }
    
    public override FileTrackingModel this[int position]
    {
        get
        {
            return filetrackingModel[position];
        }
    }

    public override int Count
    {
        get
        {
            return filetrackingModel.Count;
        }
    }

    public override long GetItemId(int position)
    {
        return position;
    }

    public override View GetView(int position, View convertView, ViewGroup parent)
    {
        var view = convertView;

        //if (view == null)
        //{

            view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ExistingLayoutAdapter, null);
            TextView qrDetails = view.FindViewById<TextView>(Resource.Id.qrdetail);
            TextView senderName = view.FindViewById<TextView>(Resource.Id.sender);
            TextView addresseeName = view.FindViewById<TextView>(Resource.Id.addressee);
            TextView subject = view.FindViewById<TextView>(Resource.Id.subject);
            TextView letterDate = view.FindViewById<TextView>(Resource.Id.DateLetter);
            TextView medium = view.FindViewById<TextView>(Resource.Id.medium);
            TextView scanningDate = view.FindViewById<TextView>(Resource.Id.scanningDate);
            TextView syncDate = view.FindViewById<TextView>(Resource.Id.syncDate);



            //}

            qrDetails.Text = "QR Deatils : "+filetrackingModel[position].QrDetail;
            senderName.Text = "Sender name : " +filetrackingModel[position].SenderName;
            addresseeName.Text = "AddresseeName : "+ filetrackingModel[position].AddresseeName;
            subject.Text = "Subject : " + filetrackingModel[position].Subject;
            try
            {
                letterDate.Text = "LetterDate : " + filetrackingModel[position].LetterDate.ToString();
            }
            catch (Exception ex)
            {
                letterDate.Text = "";
            }
            medium.Text = "Medium : "+filetrackingModel[position].Medium;
            scanningDate.Text = "ScanningDate : "+filetrackingModel[position].ScanningDate.ToString();
            syncDate.Text = "SyncDate : "+ filetrackingModel[position].SyncDate.ToString();

            return view;

    }

}
}