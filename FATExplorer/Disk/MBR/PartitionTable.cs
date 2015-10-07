using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    /*
     * PartitionTable struct as class
     */
    public class PartitionTable
    {
        /*
         * CTOR - Deserializes partition table bytes and creates partition table entries
         */
        public PartitionTable(byte[] data)
        {
            //Four partitions per partition table (64 bytes)
            partitions = new PartitionTableEntry[4];

            //Iterate over each entry
            for (int i = 0; i < 4; i++)
            {
                byte[] entry = new byte[16];
                Array.Copy(data, i * 16, entry, 0, entry.Length);

                //Deserialize
                partitions[i] = new PartitionTableEntry(entry);
            }
        }

        #region Properties

        private PartitionTableEntry[] partitions;

        internal PartitionTableEntry[] Partitions
        {
            get { return partitions; }
            set { partitions = value; }
        }

        #endregion
    }
}
