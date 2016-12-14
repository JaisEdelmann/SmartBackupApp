using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBackupApp.App.Services
{
    class FileService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: "+ sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
                _logger.Trace($"Copying {Path.Combine(sourceDirName,file.Name)} >>> {temppath} ");
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public void CleanDirectory(string targetDir, int versions)
        {
            _logger.Debug($"Delete all directory older then '{versions}' versions");
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(targetDir);

            foreach (DirectoryInfo subDir in dir.GetDirectories().OrderByDescending(x => x.CreationTime).Take(versions))
            {
                _logger.Debug($"Preserving directories: {subDir.FullName}");
            }
            foreach (DirectoryInfo subDir in dir.GetDirectories().OrderByDescending(x => x.CreationTime).Skip(versions))
            {
                _logger.Warn($"Deleting directory: {subDir.FullName}");
                DeleteDirectory(subDir.FullName);
            }

            foreach (var file in dir.GetFiles().OrderByDescending(x => x.CreationTime).Take(versions +1))
            {
                _logger.Debug($"Preserving files: {file.FullName}");
            }

            foreach (var file in dir.GetFiles().OrderByDescending(x => x.CreationTime).Skip(versions -1))
            {
                _logger.Warn($"Deleting file: {file.FullName}");
                DeleteFile(file.FullName);

            }

        }

        public void DeleteFile(string path)
        {
            _logger.Trace($"Deleting file {path}");
            File.Delete(path);
        }

        public void DeleteDirectory(string dir)
        {
            _logger.Trace($"Deleting directory {dir}");
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(dir);
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
            directory.Delete();
        }
    }
}
