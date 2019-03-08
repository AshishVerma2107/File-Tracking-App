using SQLite;

namespace File_Tracking.Model
{
    public class MediaFileModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Type { get; set; }
        public string TypeId { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
    }
}