using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace reflex_training
{
    class FileLogger
    {
        private ReaderWriterLockSlim lock_ = new ReaderWriterLockSlim();
        public void WriteData(string path, string format, params object[] args)
        {
            lock_.EnterWriteLock();
            try
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(format, args);
                }
            }
            finally
            {
                lock_.ExitWriteLock();
            }
        }

    }
}
