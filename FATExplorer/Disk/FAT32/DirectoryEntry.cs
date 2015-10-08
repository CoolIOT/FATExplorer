using FATExplorer.Utility;
using Microsoft.Win32.SafeHandles;
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
        /*
         * CTOR - Creates current entry data's node then attaches any children, recurses into children directories
         * **NOTE** Individual values are Little-Endian on disk, except strings **NOTE**
         */
        public DirectoryEntry(byte[] data, BufferedDiskReader disk, Partition partition)
        {
            //Attribute byte
            attributeByte = data[0x0B];

            //0xE5 is unused - 0x2E is . or .. (need to figure out how to handle)
            if (data[0] == 0xE5 || data[0] == 0x2E)
            {
                type = DirectoryEntryType.Unused;
                return;
            }

            //0x00 is end of directory entry
            if (data[0] == 0x00)
            {
                type = DirectoryEntryType.EndOfDirectory;
                return;
            }

            //Four least significant bits set means long file name
            if ((attributeByte & 0x0F) == 0x0F)
            {
                //Set type
                type = DirectoryEntryType.LongFileName;

                //Type parse long file name
                longFilename = parseLongFilename(data, disk);

                //Read next entry after long file name
                disk.Read(data, 0, data.Length);

                //Get new attribute byte
                attributeByte = data[0x0B];

                //Normal entry always follows long filename entry
                type = DirectoryEntryType.Normal;
            } else {
                type = DirectoryEntryType.Normal;
            }

            //Bit 4 is directory flag
            isDirectory = (attributeByte & 16) != 0;

            //Bit 3 alone is VolumeId flag
            isVolumeID = (attributeByte ^ 8) == 0;

            //Get short name
            byte[] shortNameBytes = new byte[11];
            Array.Copy(data, 0, shortNameBytes, 0, shortNameBytes.Length);

            //Convert to string
            shortFilename = Encoding.ASCII.GetString(shortNameBytes);

            //First cluster of entry - Little-Endian
            uint firstCluster = (uint)(data[0x15] << 24 | data[0x14] << 16 | data[0x1B] << 8 | data[0x1A]);

            //Calculate LBA for first cluster
            firstClusterLBA = (uint)(partition.ClusterBeginLBA + (firstCluster - 2) * partition.BootSector.BPB.SectorsPerCluster);

            //If directory or volume we need to attach children
            if (isDirectory || isVolumeID)
            {
                children = new List<DirectoryEntry>();

                //Save old position for recursion
                ulong oldPos = disk.Position;

                //If this is directory seek to directory entry location
                if (isDirectory)
                {
                    disk.SeekAbsolute(firstClusterLBA * partition.BootSector.BPB.BytesPerSector);
                }
                //Read next entry
                disk.Read(data, 0, data.Length);

                //Recurse on next entry bytes
                DirectoryEntry entry = new DirectoryEntry(data, disk, partition);

                //Iterate until end of directory entry is found
                while (entry.Type != DirectoryEntryType.EndOfDirectory)
                {
                    //Attach child unless unused
                    if (entry.Type != DirectoryEntryType.Unused)
                    {
                        children.Add(entry);
                    }
                    //Read new bytes
                    disk.Read(data, 0, data.Length);

                    //Recurse on next entry bytes
                    entry = new DirectoryEntry(data, disk, partition);
                }
                //Seek to start Pos since we're going back up a level
                disk.SeekAbsolute(oldPos);
            }
        }

        /*
         * parseLongFilename - Parses long file name of one or more entries with FileStream and first entry bytes
         */
        private string parseLongFilename(byte[] data, BufferedDiskReader disk)
        {
            string result = "";
            //Parse first entry bytes
            LongFilenameEntry entry = new LongFilenameEntry(data);

            //Add to result
            result += entry.FilenamePart;

            //Sequence number is first entry's sequence number XOR 0x40
            int sequenceNumber = entry.SequenceNumber ^ 0x40;

            //Iterate over following entries
            for (int i = 0; i < sequenceNumber - 1; i++)
            {
                //Read next entry
                disk.Read(data, 0, data.Length);

                //Parse entry bytes
                entry = new LongFilenameEntry(data);

                //Add to result - they are in reverse order
                result = entry.FilenamePart + result;
            }

             return result;
        }

        #region Properties

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

        #endregion

    }
}
