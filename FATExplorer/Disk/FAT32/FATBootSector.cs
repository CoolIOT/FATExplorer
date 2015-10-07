using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    /*
     * FATBootSector struct as class 
     * **NOTE** Individual values are Little-Endian except strings **NOTE**
     */
    public class FATBootSector
    {
        public FATBootSector(byte[] data)
        {
            jumpInstruction = (uint)(data[2] << 16 | data[1] << 8 | data[0]);

            OEM_ID = (uint)(data[3] << 56 | data[4] << 48 | data[5] << 40 | data[6] << 32 | data[7] << 24 | data[8] << 16 | data[9] << 8 | data[10]);

            bPB = new BootParameterBlock(data);

            exBPB = new ExBootParameterBlock(data);

            bootstrapCode = new byte[448];
            Array.Copy(data, 0x3E, bootstrapCode, 0, bootstrapCode.Length);

            endMarker = (ushort)(data[0x1FE] << 8 | data[0x1FF]);
        }

        #region Properties

        private uint jumpInstruction;

        public uint JumpInstruction
        {
            get { return jumpInstruction; }
            set { jumpInstruction = value; }
        }
        private ulong OEM_ID;

        public ulong OEM_ID1
        {
            get { return OEM_ID; }
            set { OEM_ID = value; }
        }
        private BootParameterBlock bPB;

        public BootParameterBlock BPB
        {
            get { return bPB; }
            set { bPB = value; }
        }
        private ExBootParameterBlock exBPB;

        public ExBootParameterBlock ExBPB
        {
            get { return exBPB; }
            set { exBPB = value; }
        }
        private byte[] bootstrapCode;

        public byte[] BootstrapCode
        {
            get { return bootstrapCode; }
            set { bootstrapCode = value; }
        }
        private ushort endMarker;

        public ushort EndMarker
        {
            get { return endMarker; }
            set { endMarker = value; }
        }

        #endregion
    }
}
