using System;
using System.IO;

namespace PortalService.Contract.ViewModel
{
    [Serializable]
    public class AnnounceInfoFileUpload
    {
        /// <summary>
        /// 公告資料Guid
        /// </summary>
        public Guid AnnounceUuid { get; set; }
        /// <summary>
        /// 檔案byte[]
        /// </summary>
        public byte[] FileBytes { get; set; }
        /// <summary>
        /// 上傳位置的根目錄
        /// </summary>
        public string UploadFolderRoot { get; set; }
        /// <summary>
        /// 副檔名
        /// </summary>
        public string FileExtension { get; set; }
    }
}
