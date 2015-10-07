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
     * PreciseFileStream - FileStream extension to provide Byte precise Position and Seek     * 
     */
    public class PreciseFileStream : FileStream
    {
        /*
         * CTOR override to initialize position var
         */
        public PreciseFileStream(SafeFileHandle handle, FileAccess access) : base(handle, access)
        {
            position = 0;
        }

        /*
         * Override base position with this one
         */
        private long position;

        public override long Position
        {
            get { return position; }
            set { position = value; }
        }

        /*
         * Read override to keep position accurate
         */
        public override int Read(byte[] array, int offset, int count)
        {
            position += count;
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
                    position += offset;
                    break;

                case SeekOrigin.Begin:
                    position = offset;
                    break;

                case SeekOrigin.End:
                    position = (base.Length - 1) - offset;
                    break;
            }
            long val = base.Seek(offset, origin);
            byte[] trash = new byte[offset % 0x100];
            if (trash.Length != 0)
            {
                base.Read(trash, 0, trash.Length);
            }
            return val + trash.Length;
        }
    }
}
