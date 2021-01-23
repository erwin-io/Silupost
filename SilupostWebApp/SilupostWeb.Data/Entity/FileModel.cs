using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Data.Entity
{
    public class FileModel
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public int FileSize { get; set; }
        public byte[] FileContent { get; set; }
        public SystemRecordManagerModel SystemRecordManager { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
    }
}
