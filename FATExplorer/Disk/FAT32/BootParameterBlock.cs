using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    /*
     * BootParameterBlock struct as class - FAT specific structure
     * **NOTE** Individual values are Little-Endian but not strings **NOTE**
     */
    public class BootParameterBlock
    {
        /*
         * CTOR - Deserializes FAT Boot Sector into BPB values
         */
        public BootParameterBlock(byte[] data)
        {
            bytesPerSector = (ushort)(data[0x0C] << 8 | data[0x0B]);
            sectorsPerCluster = data[0x0D];
            reservedSectors = (ushort)(data[0x0F] << 8 | data[0x0E]);
            numOfFATs = data[0x10];
            rootEntries = (ushort)(data[0x12] << 8 | data[0x11]);
            smallSectors = (ushort)(data[0x14] << 8 | data[0x13]);
            mediaDescriptor = data[0x15];
            sectorsPerFAT = (ushort)(data[0x17] << 8 | data[0x16]);
            sectorsPerTrack = (ushort)(data[0x19] << 8 | data[0x18]);
            numOfHeads = (ushort)(data[0x1B] << 8 | data[0x1A]);
            hiddenSectors = (uint)(data[0x1F] << 24 | data[0x1E] << 16 | data[0x1D] << 8 | data[0x1C]);
            largeSectors = (uint)(data[0x23] << 24 | data[0x22] << 16 | data[0x21] << 8 | data[0x20]);
            sectorsPerFAT32 = (uint)(data[0x27] << 24 | data[0x26] << 16 | data[0x25] << 8 | data[0x24]);
            fileSystemVersion = (ushort)(data[0x29] << 8 | data[0x28]);
            rootClusterNumber = (uint)(data[0x2D] << 24 | data[0x2C] << 16 | data[0x2B] << 8 | data[0x2A]);
            fileSysInfoSecNum = (ushort)(data[0x2F] << 8 | data[0x2E]);
            backupBootSec = (ushort)(data[0x33] << 8 | data[0x32]);

        }

        #region Properties

        private ushort bytesPerSector;

        public ushort BytesPerSector
        {
            get { return bytesPerSector; }
            set { bytesPerSector = value; }
        }
        private byte sectorsPerCluster;

        public byte SectorsPerCluster
        {
            get { return sectorsPerCluster; }
            set { sectorsPerCluster = value; }
        }
        private ushort reservedSectors;

        public ushort ReservedSectors
        {
            get { return reservedSectors; }
            set { reservedSectors = value; }
        }
        private byte numOfFATs;

        public byte NumOfFATs
        {
            get { return numOfFATs; }
            set { numOfFATs = value; }
        }
        private ushort rootEntries;

        public ushort RootEntries
        {
            get { return rootEntries; }
            set { rootEntries = value; }
        }
        private ushort smallSectors;

        public ushort SmallSectors
        {
            get { return smallSectors; }
            set { smallSectors = value; }
        }
        private byte mediaDescriptor;

        public byte MediaDescriptor
        {
            get { return mediaDescriptor; }
            set { mediaDescriptor = value; }
        }
        private ushort sectorsPerFAT;

        public ushort SectorsPerFAT
        {
            get { return sectorsPerFAT; }
            set { sectorsPerFAT = value; }
        }
        private ushort sectorsPerTrack;

        public ushort SectorsPerTrack
        {
            get { return sectorsPerTrack; }
            set { sectorsPerTrack = value; }
        }
        private ushort numOfHeads;

        public ushort NumOfHeads
        {
            get { return numOfHeads; }
            set { numOfHeads = value; }
        }
        private uint hiddenSectors;

        public uint HiddenSectors
        {
            get { return hiddenSectors; }
            set { hiddenSectors = value; }
        }
        private uint largeSectors;

        public uint LargeSectors
        {
            get { return largeSectors; }
            set { largeSectors = value; }
        }
        private uint sectorsPerFAT32;

        public uint SectorsPerFAT32
        {
            get { return sectorsPerFAT32; }
            set { sectorsPerFAT32 = value; }
        }
        private ushort reserved;

        public ushort Reserved
        {
            get { return reserved; }
            set { reserved = value; }
        }
        private ushort fileSystemVersion;

        public ushort FileSystemVersion
        {
            get { return fileSystemVersion; }
            set { fileSystemVersion = value; }
        }
        private uint rootClusterNumber;

        public uint RootClusterNumber
        {
            get { return rootClusterNumber; }
            set { rootClusterNumber = value; }
        }
        private ushort fileSysInfoSecNum;

        public ushort FileSysInfoSecNum
        {
            get { return fileSysInfoSecNum; }
            set { fileSysInfoSecNum = value; }
        }
        private ushort backupBootSec;

        public ushort BackupBootSec
        {
            get { return backupBootSec; }
            set { backupBootSec = value; }
        }
        private byte[] reserved2;

        public byte[] Reserved2
        {
            get { return reserved2; }
            set { reserved2 = value; }
        }

        #endregion
    }
}
