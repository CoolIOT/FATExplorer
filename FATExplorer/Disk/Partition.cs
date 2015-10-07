using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    public class Partition
    {
        /* 
         * CTOR - Creates FATBootSector and Calculates Partition Cluster Start LBA
         * 
         */
        public Partition(byte[] bootSectorBytes, HardDrive hdd, PartitionTableEntry entry)
        {
            bootSector = new FATBootSector(bootSectorBytes);
            this.hdd = hdd;
            this.entry = entry;

            //Partition cluster starts immediately following both FAT's
            clusterBeginLBA = entry.LBA_Begin1 + bootSector.BPB.ReservedSectors + (bootSector.BPB.NumOfFATs * bootSector.BPB.SectorsPerFAT32);
        }

        /*
         * ParseDirectoryEntries - Seeks disk to Directory Table start then begins recursive creation         * 
         */
        public void ParseDirectoryEntries(PreciseFileStream disk)
        {            
            disk.Seek((long)this.clusterBeginLBA * BootSector.BPB.BytesPerSector, SeekOrigin.Current);

            byte[] data = new byte[32];
            disk.Read(data, 0, data.Length);
            rootDirectory = new DirectoryEntry(data, disk, this);
        }

        /*
         * ReadFAT - Read FAT table 1 into memory
         */
        public void ReadFAT(PreciseFileStream disk)
        {
            byte[] fatBytes = new byte[(bootSector.BPB.SectorsPerFAT32 * bootSector.BPB.BytesPerSector)];
            uint[] FAT = new uint[fatBytes.Length / 4];

            disk.Seek((long)(bootSector.BPB.BytesPerSector * clusterBeginLBA), SeekOrigin.Begin);
            disk.Read(fatBytes, 0, fatBytes.Length);

            for (int i = 0; i < FAT.Length; i++)
            {
                FAT[i] = (uint)(fatBytes[i * 4 + 0] << 24 | fatBytes[i * 4 + 1] << 16 | fatBytes[i * 4 + 2] << 8 | fatBytes[i * 4 + 3]);
            }
        }

        #region Properties

        private DirectoryEntry rootDirectory;

        public DirectoryEntry RootDirectory
        {
            get { return rootDirectory; }
            set { rootDirectory = value; }
        }

        private FATBootSector bootSector;

        public FATBootSector BootSector
        {
            get { return bootSector; }
            set { bootSector = value; }
        }

        private HardDrive hdd;

        public HardDrive Hdd
        {
            get { return hdd; }
            set { hdd = value; }
        }

        private PartitionTableEntry entry;

        public PartitionTableEntry Entry
        {
            get { return entry; }
            set { entry = value; }
        }

        private ulong clusterBeginLBA;

        public ulong ClusterBeginLBA
        {
            get { return clusterBeginLBA; }
            set { clusterBeginLBA = value; }
        }

        #endregion

    }
}
