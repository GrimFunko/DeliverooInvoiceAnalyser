using System;
using System.IO;

namespace InvoiceAnalyserLibrary
{
    public class FileHandler
    {
        private string _fullDirectoryPath;
        public string FullDirectoryPath
        {
            get { return _fullDirectoryPath; }
            set { _fullDirectoryPath = value; }
        }

        public FileHandler(string _directoryPath)
        {
            FullDirectoryPath = _directoryPath;
        }

        public int GetPDFFileCount()
        {
            return Directory.GetFiles(FullDirectoryPath, "*.pdf").Length;
        }

    }
}
