using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    /*
     * MasterBootRecord struct as class
     */
    public class MasterBootRecord
    {
        /*
         * CTOR - Deserializes MBR data from byte array
         */
        public MasterBootRecord(byte[] data)
        {
            //First 446 bytes are the boot sector code
            bootSector = new byte[446];
            Array.Copy(data, 0, bootSector, 0, bootSector.Length);

            //Next 64 bytes are the partition table bytes
            byte[] partitionTableBytes = new byte[64];
            Array.Copy(data, 446, partitionTableBytes, 0, partitionTableBytes.Length);

            //Deserialize partition table bytes
            partitionTable = new PartitionTable(partitionTableBytes);
        }

        #region Properties

        private byte[] bootSector;

        public byte[] BootSector
        {
            get { return bootSector; }
            set { bootSector = value; }
        }

        private PartitionTable partitionTable;

        public PartitionTable PartitionTable
        {
            get { return partitionTable; }
            set { partitionTable = value; }
        }

        #endregion
    }
}
