using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    /*
     * PreciseFileStream - FileStream extension to provide Byte precise PrecisePosition and Seek     * 
     */
    public class PreciseFileStream : FileStream
    {
        /*
         * CTOR override to initialize precisePrecisePosition var
         */
        public PreciseFileStream(SafeFileHandle handle, FileAccess access) : base(handle, access)
        {
            precisePosition = 0;
        }

        /*
         * Override base precisePrecisePosition with this one
         */
        private long precisePosition;

        public long PrecisePosition
        {
            get { return precisePosition; }
            set { precisePosition = value; }
        }

        /*
         * Read override to keep precisePrecisePosition accurate
         */
        public override int Read(byte[] array, int offset, int count)
        {
            precisePosition += count;
            return base.Read(array, offset, count);
        }

        /*
         * Seek override hack to seek desired amount - Doesn't work for SeekOrigin.End
         */
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Current:
                    precisePosition += offset;
                    break;

                case SeekOrigin.Begin:
                    precisePosition = offset;
                    break;

                case SeekOrigin.End:
                    precisePosition = (base.Length - 1) - offset;
                    break;
            }
            long val = base.Seek(offset, origin);
            byte[] trash = new byte[offset % 0x400];
            if (trash.Length != 0)
            {
                base.Read(trash, 0, trash.Length);
            }
            return val + trash.Length;
        }
    }
}
