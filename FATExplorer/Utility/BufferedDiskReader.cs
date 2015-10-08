﻿using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer.Utility
{
    public class BufferedDiskReader
    {
        private const uint bytesPerSector = 512;

        private SafeFileHandle hFile;

        private byte[] buffer;

        private long currentPos;

        private long absolutePos;

        private int bufferSector;

        private uint bytesRead;

        public BufferedDiskReader(string filename)
        {
            hFile = Exports.CreateFile(filename,
                                        (uint)FileAccess.Read,
                                        (uint)FileShare.None,
                                        IntPtr.Zero,
                                        (uint)FileMode.Open,
                                        Exports.FILE_FLAG_NO_BUFFERING,
                                        IntPtr.Zero);
            buffer = new byte[bytesPerSector];
            Exports.ReadFile(hFile, buffer, (uint)buffer.Length, ref bytesRead, IntPtr.Zero);
            bufferSector = 0;

            currentPos = 0;
        }

        public bool IsInvalid
        {
            get
            {
                return hFile.IsInvalid;
            }
        }

        public ulong Position
        {
            get
            {
                return (ulong)absolutePos;
            }
        }

        public int Read(byte[] data, uint index, int length)
        {
            //If read is in current buffer sector
            if (bufferSector == ((absolutePos + (length-1)) / bytesPerSector))
            {
                Array.Copy(buffer, currentPos, data, 0, length);
                currentPos += length;
                absolutePos += length;
                return length;
            }

            //If read crosses into next buffer sector
            if (bufferSector < ((absolutePos + (length-1)) / bytesPerSector))
            {
                Array.Copy(buffer, currentPos, data, 0, buffer.Length - currentPos);
                Exports.ReadFile(hFile, buffer, bytesPerSector, ref bytesRead, IntPtr.Zero);
                Array.Copy(buffer, 0, data, buffer.Length - currentPos, length - (buffer.Length - currentPos));
                currentPos = length % bytesPerSector;
                absolutePos += length;
                bufferSector++;
                return length;
            }
            return 0;
        }

        public ulong SeekAbsolute(ulong pos)
        {
            ulong res = 0;
            if ((uint)(pos / bytesPerSector) != bufferSector)
            {
                uint closestSector = (uint)((pos / bytesPerSector) * bytesPerSector);
                int positionLow = (int)closestSector;
                int positionHigh = (int)((closestSector & 0xFFFFFFFF00000000) >> 32);
                res = Exports.SetFilePointer(hFile, positionLow, ref positionHigh, Exports.EMoveMethod.Begin);
                uint confirmRes = Exports.SetFilePointer(hFile, 0, ref positionHigh, Exports.EMoveMethod.Current);
                if (res != confirmRes)
                {
                    int value = Marshal.GetLastWin32Error();
                    throw new Exception("RUH ROH");
                }
                bufferSector = (int)(closestSector / bytesPerSector);
                Exports.ReadFile(hFile, buffer, (uint)buffer.Length, ref bytesRead, IntPtr.Zero);
            }
            currentPos = (long)(pos % bytesPerSector);
            absolutePos = (long)pos;
            return res;
        }

        public void Close()
        {
            hFile.Close();
        }

        public long SectorInBytes
        {
            get
            {
                int x = 0;
                return Exports.SetFilePointer(hFile, 0, ref x, Exports.EMoveMethod.Current);
            }
        }
    }
}