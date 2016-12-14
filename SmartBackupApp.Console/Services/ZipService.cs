using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBackupApp.App.Services
{
    public class ZipService
    {
        private static readonly Logger _logger  = LogManager.GetCurrentClassLogger();
        private static FileService _fileService = new FileService();
        public void ZipFolder(string path, bool DeleteFolderOnCompletation)
        {
            var dir             = new DirectoryInfo(path);
            var zipFilepath     = dir.FullName.Remove(dir.FullName.Length - 1, 1) + ".zip";

            _logger.Debug($"Zipping result {path} >> {zipFilepath}");
            ZipFile.CreateFromDirectory(path, zipFilepath,  CompressionLevel.Fastest, true);
            if(DeleteFolderOnCompletation) { _fileService.DeleteDirectory(path); }
        }
    }
}
