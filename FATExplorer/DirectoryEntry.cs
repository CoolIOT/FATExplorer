using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    public class DirectoryEntry
    {
        public DirectoryEntry(byte[] data, PreciseFileStream disk, Partition partition)
        {
            attributeByte = data[0x0B];

            if (data[0] == 0xE5 || data[0] == 0x2E)
            {
                type = DirectoryEntryType.Unused;
                return;
            }

            if (data[0] == 0x00)
            {
                type = DirectoryEntryType.EndOfDirectory;
                return;
            }

            if ((attributeByte & 0x0F) == 0x0F)
            {
                type = DirectoryEntryType.LongFileName;
                longFilename = parseLongFilename(data, disk);
                attributeByte = data[0x0B];
                type = DirectoryEntryType.Normal;
            } else {
                type = DirectoryEntryType.Normal;
            }

            isDirectory = (attributeByte & 16) != 0;
            isVolumeID = (attributeByte ^ 8) == 0;

            byte[] shortNameBytes = new byte[11];
            Array.Copy(data, 0, shortNameBytes, 0, shortNameBytes.Length);
            shortFilename = Encoding.ASCII.GetString(shortNameBytes);

            uint firstCluster = (uint)(data[0x15] << 24 | data[0x14] << 16 | data[0x1B] << 8 | data[0x1A]);

            firstClusterLBA = (uint)(partition.ClusterBeginLBA + (firstCluster - 2) * partition.BootSector.BPB.SectorsPerCluster);


            if (isDirectory || isVolumeID)
            {
                children = new List<DirectoryEntry>();
                long oldPos = disk.Position;
                if (isDirectory)
                {
                    disk.Seek(firstClusterLBA * partition.BootSector.BPB.BytesPerSector, SeekOrigin.Begin);
                }
                disk.Read(data, 0, data.Length);

                DirectoryEntry entry = new DirectoryEntry(data, disk, partition);
                while (entry.Type != DirectoryEntryType.EndOfDirectory)
                {
                    if (entry.Type != DirectoryEntryType.Unused)
                    {
                        children.Add(entry);
                    }

                    disk.Read(data, 0, data.Length);
                    entry = new DirectoryEntry(data, disk, partition);
                }
                disk.Seek(oldPos, SeekOrigin.Begin);
            }
        }

        private string parseLongFilename(byte[] data, FileStream disk)
        {
            string result = "";
            LongFilenameEntry entry = new LongFilenameEntry(data);
            result += entry.FilenamePart;
            int sequenceNumber = entry.SequenceNumber ^ 0x40;

            for (int i = 0; i < sequenceNumber - 1; i++)
            {
                disk.Read(data, 0, data.Length);
                entry = new LongFilenameEntry(data);
                result = entry.FilenamePart + result;
            }
            disk.Read(data, 0, data.Length);

             return result;
        }

        private List<DirectoryEntry> children;

        public List<DirectoryEntry> Children
        {
            get { return children; }
            set { children = value; }
        }

        private bool isVolumeID;

        public bool IsVolumeID
        {
            get { return isVolumeID; }
            set { isVolumeID = value; }
        }

        private bool isDirectory;

        public bool IsDirectory
        {
            get { return isDirectory; }
            set { isDirectory = value; }
        }

        private DirectoryEntryType type;

        public DirectoryEntryType Type
        {
            get { return type; }
            set { type = value; }
        }

        private string shortFilename;

        public string ShortFilename
        {
            get { return shortFilename; }
            set { shortFilename = value; }
        }

        private string longFilename;

        public string LongFilename
        {
          get { return longFilename; }
          set { longFilename = value; }
        }

        private byte attributeByte;

        private uint firstClusterLBA;

        public uint FirstClusterLBA
        {
            get { return firstClusterLBA; }
            set { firstClusterLBA = value; }
        }

        private uint fileSize;

        public uint FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

    }
}
