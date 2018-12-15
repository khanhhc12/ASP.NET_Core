using System;
using System.IO;

namespace HelloQuartz
{
    public static class FileWatcher
    {
        public static bool IsChanged = false;
        public static bool IsNotified = false;

        public static void CreateFileWatcher(string path)
        {
            try
            {
                if (IsNotified) return;
                IsChanged = true;
                IsNotified = true;

                // Create a new FileSystemWatcher and set its properties.
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = path;
                /* Watch for changes in LastAccess and LastWrite times, and 
                   the renaming of files or directories. */
                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                // Only watch text files.
                //watcher.Filter = "*.txt";
                watcher.Filter = "*.json";

                // Add event handlers.
                watcher.Changed += new FileSystemEventHandler(OnChanged);
                //watcher.Created += new FileSystemEventHandler(OnChanged);
                //watcher.Deleted += new FileSystemEventHandler(OnChanged);
                //watcher.Renamed += new RenamedEventHandler(OnRenamed);

                // Begin watching.
                watcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            IsChanged = true;
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
            IsChanged = true;
        }
    }
}