using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    public class PartitionTable
    {
        public PartitionTable(byte[] data)
        {
            partitions = new PartitionTableEntry[4];
            for (int i = 0; i < 4; i++)
            {
                byte[] entry = new byte[16];
                Array.Copy(data, i * 16, entry, 0, entry.Length);
                partitions[i] = new PartitionTableEntry(entry);
            }
        }

        private PartitionTableEntry[] partitions;

        internal PartitionTableEntry[] Partitions
        {
            get { return partitions; }
            set { partitions = value; }
        }
    }
}
