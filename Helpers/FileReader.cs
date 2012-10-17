using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Helpers
{
    /// <summary>
    /// file olvasat vegzo segedosztaly
    /// </summary>
    public class FileReader
    {
        private string _FileName = String.Empty;

        private int _CodePage = 0;

        private System.IO.StreamReader _sr = null;

        private System.IO.FileStream _fs = null;

        /// <summary>
        /// konstruktor file eleresi utjaval
        /// </summary>
        /// <param name="fileName"></param>
        public FileReader(string fileName) : this(fileName, System.Text.Encoding.Default.CodePage) { }

        /// <summary>
        /// konstruktor file eleresi utjaval, karakter kodlap szamaval  
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="codePage"></param>
        public FileReader(string fileName, int codePage)
        {
            _FileName = fileName;

            _CodePage = codePage;
        }

        /// <summary>
        /// file eleres kiolvasasa
        /// </summary>
        public string FileName
        {
            get { return _FileName; }
        }

        /// <summary>
        /// file tartalmanak beolvasasa string-be
        /// </summary>
        /// <returns></returns>
        public string ReadToEnd()
        {
            try
            {
                if (String.IsNullOrEmpty(_FileName)) { return String.Empty; }

                if (!System.IO.File.Exists(_FileName)) { return String.Empty; }

                _fs = new System.IO.FileStream(_FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                _sr = new System.IO.StreamReader(_fs, System.Text.Encoding.GetEncoding(_CodePage));

                _sr.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

                return _sr.ReadToEnd();
            }
            catch { return String.Empty; }
            finally 
            {
                if (_fs != null) { _fs.Close(); }
                if (_sr != null) { _sr.Close(); }
            }
        }

        /// <summary>
        /// file beolvasasa byte tombbe
        /// </summary>
        /// <returns></returns>
        public byte[] ReadBytes()
        {
            try
            {
                byte[] arr = new System.Collections.Generic.List<byte>().ToArray();

                if (String.IsNullOrEmpty(_FileName)) { return arr; }

                if (!System.IO.File.Exists(_FileName)) { return arr; }

                _fs = new System.IO.FileStream(_FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                arr = new byte[_fs.Length];
                
                _fs.Read(arr, 0, (int)_fs.Length);

                return arr;
            }
            catch { return new System.Collections.Generic.List<byte>().ToArray(); }
            finally 
            {
                if (_fs != null) { _fs.Close(); }
                if (_sr != null) { _sr.Close(); }
            }
        }

    }
}
