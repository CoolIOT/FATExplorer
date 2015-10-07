using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    public class PreciseFileStream : FileStream
    {
        public PreciseFileStream(IntPtr handle, FileAccess access) : base(handle, access)
        {
            position = 0;
        }

        private long position;

        public override long Position
        {
            get { return position; }
            set { position = value; }
        }

        public override int Read(byte[] array, int offset, int count)
        {
            position += count;
            return base.Read(array, offset, count);
        }

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
