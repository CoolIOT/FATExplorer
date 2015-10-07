using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FATExplorer
{
    public partial class FATExplorer : Form
    {
        private const int BYTES_PER_SECTOR = 512;

        public FATExplorer()
        {
            InitializeComponent();

            List<HardDrive> disks = new List<HardDrive>();

            ManagementObjectSearcher search = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject wmi_HD in search.Get())
            {
                HardDrive hdd = new HardDrive();
                hdd.Model = wmi_HD["Model"].ToString();
                hdd.Type = wmi_HD["InterfaceType"].ToString();
                hdd.DeviceId = wmi_HD["DeviceId"].ToString();

                IntPtr handle = Exports.CreateFile(hdd.DeviceId,
                                                (uint)FileAccess.Read,
                                                (uint)FileShare.None,
                                                IntPtr.Zero,
                                                (uint)FileMode.Open,
                                                Exports.FILE_FLAG_NO_BUFFERING,
                                                IntPtr.Zero);

                if (handle.ToInt32() == -1)
                {
                    MessageBox.Show("Verify you have run the program or development environment with Administrator Privileges to access the hard disks.");
                    continue;
                }

                PreciseFileStream disk = new PreciseFileStream(handle, FileAccess.Read);
                byte[] data = new byte[512];
                disk.Read(data, 0, 512);

                hdd.MBR = new MasterBootRecord(data);

                foreach (PartitionTableEntry entry in hdd.MBR.PartitionTable.Partitions)
                {
                    if (!disks.Contains(hdd) && (entry.TypeCode == 0x0B || entry.TypeCode == 0x0C))
                    {
                        disks.Add(hdd);
                    }
                    if (entry.TypeCode == 0x0B || entry.TypeCode == 0x0C)
                    {
                        data = new byte[512];
                        disk.Seek((long)entry.LBA_Begin1 * BYTES_PER_SECTOR, SeekOrigin.Begin);
                        disk.Read(data, 0, data.Length);
                        hdd.Partitions.Add(new Partition(data, hdd, entry));
                    }
                }
                              
                disk.Close();
            }
            if (disks.Count > 0)
            {
                if (disks[0].Partitions.Count > 0)
                {
                    IntPtr handle = Exports.CreateFile(disks[0].DeviceId,
                                                (uint)FileAccess.Read,
                                                (uint)FileShare.None,
                                                IntPtr.Zero,
                                                (uint)FileMode.Open,
                                                Exports.FILE_FLAG_NO_BUFFERING,
                                                IntPtr.Zero);

                    PreciseFileStream disk = new PreciseFileStream(handle, FileAccess.Read);
                    disks[0].Partitions[0].ParseDirectoryEntries(disk);
                    disk.Close();
                    AddNode(disks[0].Partitions[0].RootDirectory);
                }
            }

        }

        public void AddNode(DirectoryEntry directoryNode)
        {
            TreeNode node = null;
            if (treeViewDirectory.Nodes.Count == 0)
            {
                node = new TreeNode(directoryNode.ShortFilename);
                treeViewDirectory.Nodes.Add(node);
                treeViewDirectory.SelectedNode = node;
            }
            else
            {
                node = new TreeNode(directoryNode.LongFilename == null ? directoryNode.ShortFilename : directoryNode.LongFilename);
                treeViewDirectory.SelectedNode.Nodes.Add(node);
            }

            if (directoryNode.IsDirectory || directoryNode.IsVolumeID)
            {
                TreeNode tempSelectedNode = treeViewDirectory.SelectedNode;
                treeViewDirectory.SelectedNode = node;
                foreach (DirectoryEntry entry in directoryNode.Children)
                {
                    AddNode(entry);
                }
                treeViewDirectory.SelectedNode = tempSelectedNode;
            }
        }

        public T ByteArrayToStructure<T>(byte[] bytes)
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return structure;
        }
    }
}
