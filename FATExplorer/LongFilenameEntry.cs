using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    public class LongFilenameEntry
    {
        public LongFilenameEntry(byte[] data)
        {
            sequenceNumber = data[0];

            filenamePart1 = new byte[10];
            Array.Copy(data, 0x01, filenamePart1, 0, filenamePart1.Length);
            filenamePart1 = fixNamePart(filenamePart1);

            attributes = data[0x0B];
            reserved = data[0x0C];
            checksum = data[0x0D];

            filenamePart2 = new byte[12];
            Array.Copy(data, 0x0E, filenamePart2, 0, filenamePart2.Length);
            filenamePart2 = fixNamePart(filenamePart2);

            reserved2 = (ushort)(data[0x1A] | data[0x1B]);

            filenamePart3 = new byte[4];
            Array.Copy(data, 0x1C, filenamePart3, 0, filenamePart3.Length);
            filenamePart3 = fixNamePart(filenamePart3);

            filenamePart = Encoding.ASCII.GetString(filenamePart1) + Encoding.ASCII.GetString(filenamePart2) + Encoding.ASCII.GetString(filenamePart3);
        }

        private byte[] fixNamePart(byte[] nameBytes) {
            byte[] fixedNameBytes = new byte[nameBytes.Length];
            int fixedNameBytesPos = 0;
            for (int i = 0; i < nameBytes.Length; i++) {
                if (nameBytes[i] != 0x00 && nameBytes[i] > 0 && nameBytes[i] < 127)
                {
                    fixedNameBytes[fixedNameBytesPos++] = nameBytes[i];
                }
            }
            string temp = Encoding.ASCII.GetString(fixedNameBytes).Replace("\0", string.Empty);
            return Encoding.ASCII.GetBytes(temp);
        }

        private string filenamePart;

        public string FilenamePart
        {
            get { return filenamePart; }
            set { filenamePart = value; }
        }

        private byte sequenceNumber;

        public byte SequenceNumber
        {
            get { return sequenceNumber; }
            set { sequenceNumber = value; }
        }
        private byte[] filenamePart1;

        public byte[] FilenamePart1
        {
            get { return filenamePart1; }
            set { filenamePart1 = value; }
        }
        private byte attributes;

        public byte Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }
        private byte reserved;

        public byte Reserved
        {
            get { return reserved; }
            set { reserved = value; }
        }
        private byte checksum;

        public byte Checksum
        {
            get { return checksum; }
            set { checksum = value; }
        }
        private byte[] filenamePart2;

        public byte[] FilenamePart2
        {
            get { return filenamePart2; }
            set { filenamePart2 = value; }
        }
        private ushort reserved2;

        public ushort Reserved2
        {
            get { return reserved2; }
            set { reserved2 = value; }
        }
        private byte[] filenamePart3;

        public byte[] FilenamePart3
        {
            get { return filenamePart3; }
            set { filenamePart3 = value; }
        }
    }
}
