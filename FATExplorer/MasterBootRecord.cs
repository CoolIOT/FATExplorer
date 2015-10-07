using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    public class MasterBootRecord
    {
        public MasterBootRecord(byte[] data)
        {
            bootSector = new byte[446];
            Array.Copy(data, 0, bootSector, 0, bootSector.Length);
            byte[] partitionTableBytes = new byte[64];
            Array.Copy(data, 446, partitionTableBytes, 0, partitionTableBytes.Length);
            partitionTable = new PartitionTable(partitionTableBytes);
        }

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
    }
}
