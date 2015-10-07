using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    /*
     * LongFilenameEntry - Represent a variable length directory entry of Longfilename type
     */
    public class LongFilenameEntry
    {
        /*
         * CTOR - Parses one or more long file name diretory entries into a string
         */
        public LongFilenameEntry(byte[] data)
        {
            //Get beginning sequence number
            sequenceNumber = data[0];

            //Bytes 1 - 10 are first part of name
            filenamePart1 = new byte[10];
            Array.Copy(data, 0x01, filenamePart1, 0, filenamePart1.Length);

            //Remove \0's every other character
            filenamePart1 = fixNamePart(filenamePart1);

            //File attributes
            attributes = data[0x0B];

            //Reserved - not used
            reserved = data[0x0C];

            //Checksum - should be used, but not
            checksum = data[0x0D];

            //Filename part 2 
            filenamePart2 = new byte[12];
            Array.Copy(data, 0x0E, filenamePart2, 0, filenamePart2.Length);

            //Remove \0's every other character
            filenamePart2 = fixNamePart(filenamePart2);

            //Chunk reserved - not used
            reserved2 = (ushort)(data[0x1A] | data[0x1B]);

            //Filename part 3
            filenamePart3 = new byte[4];
            Array.Copy(data, 0x1C, filenamePart3, 0, filenamePart3.Length);

            //Remove \0's every other character
            filenamePart3 = fixNamePart(filenamePart3);


            //Add it all up
            filenamePart = Encoding.ASCII.GetString(filenamePart1) + Encoding.ASCII.GetString(filenamePart2) + Encoding.ASCII.GetString(filenamePart3);
        }

        /*
         * fixNamePart
         * Returns - byte array passed in minus any \0's and Non-ASCII chars
         */
        private byte[] fixNamePart(byte[] nameBytes) {
            //Build new byte array
            byte[] fixedNameBytes = new byte[nameBytes.Length];
            int fixedNameBytesPos = 0;

            //Iterate over given bytes
            for (int i = 0; i < nameBytes.Length; i++) {
                //If not \0 and is ASCII character add to new byte array
                if (nameBytes[i] != 0x00 && nameBytes[i] > 0 && nameBytes[i] < 127)
                {
                    fixedNameBytes[fixedNameBytesPos++] = nameBytes[i];
                }
            }
            //Replace to remove any trailing \0's 
            string temp = Encoding.ASCII.GetString(fixedNameBytes).Replace("\0", string.Empty);

            //Return bytes
            return Encoding.ASCII.GetBytes(temp);
        }

        #region Properties

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

        #endregion
    }
}
