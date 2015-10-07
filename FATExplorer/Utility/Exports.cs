using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    public static class Exports
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.StdCall,
        SetLastError = true)]
        public static extern SafeFileHandle CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr SecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(
            SafeFileHandle hFile, 
            [Out] byte[] lpBuffer,
            uint nNumberOfBytesToRead, 
            ref uint lpNumberOfBytesRead, 
            IntPtr lpOverlapped);


        public static uint Read(
            SafeFileHandle hFile, 
            [Out] byte[] lpBuffer, 
            uint nNumberOfBytesToRead,
            IntPtr lpOverlapped)
        {
            uint bytesRead = 0;
            byte[] test = new byte[nNumberOfBytesToRead];
            bool ret = ReadFile(hFile, test, nNumberOfBytesToRead, ref bytesRead, lpOverlapped);
            int error = Marshal.GetLastWin32Error();

            return bytesRead;
        }

        public enum EMoveMethod : uint
        {
            Begin = 0,
            Current = 1,
            End = 2
        }

        [DllImport("kernel32.dll", EntryPoint = "SetFilePointer")]
        public static extern uint SetFilePointer(
              [In] Microsoft.Win32.SafeHandles.SafeFileHandle hFile,
              [In] int lDistanceToMove,
              [In, Out] ref int lpDistanceToMoveHigh,
              [In] EMoveMethod dwMoveMethod);


        public static uint Seek(
              [In] Microsoft.Win32.SafeHandles.SafeFileHandle hFile,
              [In] ulong location,
              [In] EMoveMethod dwMoveMethod)
        {
            int location_low = (int)location;
            int location_high = (int)(location & 0xFFFFFFFF00000000) >> 32;
            return Exports.SetFilePointer(hFile, location_low, ref location_high, Exports.EMoveMethod.Begin);
        }

        public const int FILE_FLAG_NO_BUFFERING = 0x20000000;

    }
}
