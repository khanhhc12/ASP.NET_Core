using System;
using System.IO;

namespace HelloQuartz
{
    public class FileWatcher
    {
        SimpleQuartz simpleQuartz = new SimpleQuartz();

        public void CreateFileWatcher(string path)
        {
            try
            {
                simpleQuartz.Run().Wait();

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
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            simpleQuartz.Run().Wait();
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
            simpleQuartz.Run().Wait();
        }
    }
}