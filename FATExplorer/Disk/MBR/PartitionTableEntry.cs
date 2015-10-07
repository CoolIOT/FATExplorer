using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    /*
     * PartitionTableEntry struct as class
     */
    public class PartitionTableEntry
    {
        /*
         * CTOR - Deserialize partition table entry bytes
         */
        public PartitionTableEntry(byte[] data)
        {
            //Byte 0 is boot flag
            bootFlag = data[0];

            //Bytes 1- 3 are CHS begin value - not used (we're using LBA)
            CHS_Begin = (uint)(data[3] << 16 | data[2] << 8 | data[1]);

            //Byte 4 is type code (filesystem in use) 
            typeCode = data[4];

            //Bytes 5 - 7 are CHS end value
            CHS_End = (uint)(data[7] << 16 | data[6] << 8 | data[5]);

            //Bytes 8-11 are LBA begin - we use this to find start of FAT
            LBA_Begin = (uint)(data[11] << 24 | data[10] << 16 | data[9] << 8 | data[8]);

            //Bytes 12 - 15 are total number of sectors, redundant - found in FAT boot sector
            Num_Sectors = (uint)(data[15] << 24 | data[14] << 16 | data[13] << 8 | data[12]);
        }

        #region Properties

        private byte bootFlag;

        public byte BootFlag
        {
            get { return bootFlag; }
        }
        private uint CHS_Begin;

        public uint CHS_Begin1
        {
            get { return CHS_Begin; }
        }
        private byte typeCode;

        public byte TypeCode
        {
            get { return typeCode; }
        }
        private uint CHS_End;

        public uint CHS_End1
        {
            get { return CHS_End; }
        }
        private uint LBA_Begin;

        public uint LBA_Begin1
        {
            get { return LBA_Begin; }
        }
        private uint Num_Sectors;

        public uint Num_Sectors1
        {
            get { return Num_Sectors; }
        }

        #endregion
    }
}
