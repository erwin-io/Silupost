using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SilupostMobileApp.Models
{
    public class FileModel
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public long FileSize { get; set; }
        public byte[] FileContent { get; set; }
        public ImageSource ImageSource { get; set; }
        public string SourceURL { get; set; }
        public SystemRecordManagerModel SystemRecordManager { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
    }
}
