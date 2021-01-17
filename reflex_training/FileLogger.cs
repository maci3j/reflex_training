using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace reflex_training
{
    /// <summary>
    /// FileLogger object definition.
    /// </summary>
    class FileLogger
    {
        private ReaderWriterLockSlim lock_ = new ReaderWriterLockSlim();
        
        /// <summary>
        /// This method enables writes to the file in queue-way, without locking up.
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <param name="format"></param>
        /// <param name="args"></param>
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
