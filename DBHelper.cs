using File_Tracking.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace File_Tracking.Database
{
    public class DBHelper
    {
        SQLiteConnection db;
        public DBHelper()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FileTracking.db3");
            db = new SQLiteConnection(dbPath);
            try
            {
               
                db.CreateTable<FileTrackingModel>();

                db.CreateTable<MediaFileModel>();
                db.CreateTable<ReceiverDetails>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Table error: " + e.Message);
            }

        }

        // public int insertLetterData(FileTrackingModel file)

        public int insertLetterData(string Qrdetails, string senderName, string addresseeName, string subject, string date, string medium, string scanDate, string syncDate, string letterImage, string uploadStatus)

     
        {
             
            int insertId = 0;

            try
            {
                FileTrackingModel tbl = new FileTrackingModel();
                tbl.QrDetail = Qrdetails;
                tbl.SenderName = senderName;
                tbl.AddresseeName = addresseeName;
                tbl.Subject = subject;
                tbl.LetterDate = date;
                tbl.Medium = medium;
                tbl.ScanningDate = scanDate;
                tbl.SyncDate = syncDate;
                tbl.ImageLetter = letterImage;
                tbl.UploadStatus = uploadStatus;

                int i = db.Insert(tbl);
               // var data1 = db.Query<FileTrackingModel>("SELECT * from [FileTrackingModel] ");
                //if (data1.Count > 0)
                //{
                //    foreach (var val in data1)
                //    {
                //        insertId = val.Id;
                //    }
                //}
                return i;
            }
            catch (Exception ex)
            { return 0; }
        }

        public List<FileTrackingModel> getQrSurveyDetail(string qrnumber)
        {
            try
            {
                 List<FileTrackingModel> data1 = db.Query<FileTrackingModel>("SELECT * from [FileTrackingModel] where [QrDetail]='" + qrnumber + "'");

                //List<FileTrackingModel> data1 = db.Query<FileTrackingModel>("Select FileTrackingModel.QrDetail,FileTrackingModel.SenderName,MediaFileModel.ImagePath, MediaFileModel.ImageName from FileTrackingModel Join MediaFileModel On FileTrackingModel.Id=MediaFileModel.Id where [Qrdetail]='" + qrnumber + "'");

                return data1;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<FileTrackingModel> getGSTINAllSurvey()
        {
            try
            {
                List<FileTrackingModel> data1 = db.Query<FileTrackingModel>("SELECT * from [FileTrackingModel]");
                return data1;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int insertMediaData(string type, string typeId, string imagePath, string imageName)
        {
            int i = 0;
            try
            {
                
                MediaFileModel tbl = new MediaFileModel();
                tbl.Type = type;
                tbl.TypeId = typeId;
                tbl.ImagePath = imagePath;
                tbl.ImageName = imageName;
                i = db.Insert(tbl);
                return i;
            }
            catch (Exception ex)
            { return 0; }
        }

        public List<MediaFileModel> getImageDetail(string imagePath)
        {
            try
            {
                List<MediaFileModel> data2 = db.Query<MediaFileModel>("SELECT * from [MediaFileModel] where [ImagePath]='" + imagePath + "'");
                return data2;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public List<MediaFileModel> getMediaImages(int insertedId, string type)
        {

            try
            {
                List<MediaFileModel> data1 = db.Query<MediaFileModel>("SELECT * from [MediaFileModel] where [TypeId] = " + insertedId + " and [Type] = '" + type + "'");
                return data1;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //public List<ReceiverDetails> getReceiverAllSurvey()
        //{


        //    try
        //    {
        //        List<ReceiverDetails> data4 = db.Query<ReceiverDetails>("SELECT * from [ReceiverDetails]");
        //        return data4;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        //public long updateGSTINStatus(int id)
        //{
        //    try
        //    {

        //        var data = db.Table<FileTrackingModel>();
        //        int idvalue = Convert.ToInt32(id);

        //        var data1 = (from values in data
        //                     where values.Id == idvalue
        //                     select values).FirstOrDefault();
        //        data1.UploadStatus = "yes";

        //        long i = db.Update(data1);
        //        return i;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}
    }
}



       

       

       

       

       

   