using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    /*
     * ExBootParameterBlock (ExtendedBPB) struct as class
     * **NOTE** Individual values are Little-Endian except strings **NOTE**
     */
    public class ExBootParameterBlock
    {
        /*
         * CTOR - Deserializes FAT Boot Sector into Extended BootParameterBlock
         */
        public ExBootParameterBlock(byte[] data)
        {
            physicalDriveNum = data[0x40];
            reserved = data[0x41];
            exBootSig = data[0x42];
            volumeSN = (uint)(data[0x46] << 24 | data[0x45] << 16 | data[0x44] << 8 | data[0x43]);

            label = new byte[11];
            Array.Copy(data, 0x47, label, 0, label.Length);

            sysID = new byte[8];
            Array.Copy(data, 0x52, sysID, 0, sysID.Length);
        }

        #region Properties

        private byte physicalDriveNum;

        public byte PhysicalDriveNum
        {
            get { return physicalDriveNum; }
            set { physicalDriveNum = value; }
        }
        private byte reserved;

        public byte Reserved
        {
            get { return reserved; }
            set { reserved = value; }
        }
        private byte exBootSig;

        public byte ExBootSig
        {
            get { return exBootSig; }
            set { exBootSig = value; }
        }
        private uint volumeSN;

        public uint VolumeSN
        {
            get { return volumeSN; }
            set { volumeSN = value; }
        }
        private byte[] label;

        public byte[] Label
        {
            get { return label; }
            set { label = value; }
        }
        private byte[] sysID;

        public byte[] SysID
        {
            get { return sysID; }
            set { sysID = value; }
        }

        #endregion
    }
}
