using SQLite;

namespace File_Tracking.Model
{
    public class FileTrackingModel
    {
        [PrimaryKey, AutoIncrement]
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

        [Ignore]
        public string UploadStatus { get; set; }
    }
}