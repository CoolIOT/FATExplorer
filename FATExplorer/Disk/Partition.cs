using FATExplorer.Utility;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    public class Partition
    {
        private uint[] FAT = null;

        /* 
         * CTOR - Creates FATBootSector and Calculates Partition Cluster Start LBA
         * 
         */
        public Partition(byte[] bootSectorBytes, HardDrive hdd, PartitionTableEntry entry)
        {
            if (entry.TypeCode == 0x0B || entry.TypeCode == 0x0C)
            {
                bootSector = new FATBootSector(bootSectorBytes);
                //Partition cluster starts immediately following both FAT's
                clusterBeginLBA = entry.LBA_Begin1 + bootSector.BPB.ReservedSectors + (bootSector.BPB.NumOfFATs * bootSector.BPB.SectorsPerFAT32);
            }
            this.hdd = hdd;
            this.entry = entry;
        }

        /*
         * ParseDirectoryEntries - Seeks disk to Directory Table start then begins recursive creation         * 
         */
        public void ParseDirectoryEntries(BufferedDiskReader disk)
        {            
            disk.SeekAbsolute((ulong)this.clusterBeginLBA * BootSector.BPB.BytesPerSector);

            int error = Marshal.GetLastWin32Error();

            byte[] data = new byte[32];
            disk.Read(data, 0, data.Length);

            rootDirectory = new DirectoryEntry(data, disk, this);
            if (!rootDirectory.IsVolumeID)
            {
                DirectoryEntry entry = rootDirectory;
                rootDirectory = new DirectoryEntry();
                rootDirectory.Children.Add(entry);
                disk.Read(data, 0, data.Length);
                entry = new DirectoryEntry(data, disk, this);
                while (entry.Type != DirectoryEntryType.EndOfDirectory)
                {
                    if (entry.Type != DirectoryEntryType.Unused)
                    {
                        rootDirectory.Children.Add(entry);
                    }
                    disk.Read(data, 0, data.Length);
                    entry = new DirectoryEntry(data, disk, this);
                }
            }
        }

        private byte[] blankVolumeId = {
            
        };

        /*
         * ReadFAT - Read FAT table 1 into memory
         */
        public void ReadFAT(BufferedDiskReader disk)
        {
            byte[] fatBytes = new byte[(bootSector.BPB.SectorsPerFAT32 * bootSector.BPB.BytesPerSector)];
            FAT = new uint[fatBytes.Length / 4];

            disk.SeekAbsolute((ulong)(bootSector.BPB.BytesPerSector * clusterBeginLBA));
            disk.Read(fatBytes, 0, fatBytes.Length);


            for (int i = 0; i < FAT.Length; i++)
            {
                FAT[i] = (uint)(fatBytes[i * 4 + 0] << 24 | fatBytes[i * 4 + 1] << 16 | fatBytes[i * 4 + 2] << 8 | fatBytes[i * 4 + 3]);
            }
        }

        private long ReadNextFAT(uint index)
        {
            if (FAT == null || index >= FAT.Length || index < 0)
            {
                return -1;
            }
            return FAT[index];
        }

        public byte[] OpenFile(BufferedDiskReader disk, DirectoryEntry file)
        {
            if (file.IsDirectory)
            {
                return null;
            }
            byte[] data = ReadCluster(disk, file.FirstClusterLBA);
            return data;

        }

        private byte[] ReadCluster(BufferedDiskReader disk, uint clusterLBA)
        {
            byte[] clusterBytes = new byte[(ulong)bootSector.BPB.BytesPerSector * (ulong)bootSector.BPB.SectorsPerCluster];

            ulong clusterStartByte = (ulong)bootSector.BPB.BytesPerSector * (ulong)clusterLBA;

            disk.SeekAbsolute(clusterStartByte);

            for (int i = 0; i < bootSector.BPB.SectorsPerCluster; i++)
            {
                byte[] data = new byte[bootSector.BPB.BytesPerSector];
                disk.Read(data, 0, data.Length);
                Array.Copy(data, 0, clusterBytes, i * bootSector.BPB.BytesPerSector, bootSector.BPB.BytesPerSector);
            }
            return clusterBytes;
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
