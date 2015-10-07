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
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace FATExplorer
{
    public partial class FATExplorer : Form
    {
        //This is supposed to be constant across all FAT32 drives.
        private const int BYTES_PER_SECTOR = 512;
        List<HardDrive> disks;

        public FATExplorer()
        {
            InitializeComponent();

            enumerateFATDevices();

            buildDropDownList();

        }

        public void buildDropDownList()
        {
            foreach (HardDrive hdd in disks)
            {
                foreach (Partition partition in hdd.Partitions)
                {
                    if (partition.Entry.TypeCode == 0x0B || partition.Entry.TypeCode == 0x0C)
                    {
                        if (partition.RootDirectory == null)
                        {
                            parseDirectoryTree(partition);
                        }
                        string volumeId = partition.RootDirectory.LongFilename == null ? partition.RootDirectory.ShortFilename : partition.RootDirectory.LongFilename;
                        string volumeName = String.Format("{0} Disk: {1} Type: {2}", volumeId, hdd.DeviceId, hdd.Type);
                        comboBoxPartitions.Items.Add(new ComboBoxItem(volumeName, partition));
                    }
                }
            }
        }

        public void enumerateFATDevices()
        {
            disks = new List<HardDrive>();

            ManagementObjectSearcher search = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            //Iterate over each search result - list of HD's from WMI
            foreach (ManagementObject wmi_HD in search.Get())
            {
                HardDrive hdd = new HardDrive();
                hdd.Model = wmi_HD["Model"].ToString();
                hdd.Type = wmi_HD["InterfaceType"] == null ? "" : wmi_HD["InterfaceType"].ToString();
                hdd.DeviceId = wmi_HD["DeviceId"].ToString();

                //Kernel32 CreateFile 
                SafeFileHandle handle = Exports.CreateFile(hdd.DeviceId,
                                                (uint)FileAccess.Read,
                                                (uint)FileShare.None,
                                                IntPtr.Zero,
                                                (uint)FileMode.Open,
                                                Exports.FILE_FLAG_NO_BUFFERING,
                                                IntPtr.Zero);

                //Occurs when in use or insufficient privileges
                if (handle.IsInvalid)
                {
                    int error = Marshal.GetLastWin32Error();
                    MessageBox.Show(this, "Please verify you have Administrator Privileges and disks are not in use.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    continue;
                }

                //HDD is valid, add to our list
                disks.Add(hdd);

                //Create extended FileStream from disk handle
                PreciseFileStream disk = new PreciseFileStream(handle, FileAccess.Read);

                //Sector buffer
                byte[] data = new byte[512];

                //Fill Buffer
                disk.Read(data, 0, 512);

                //Deserialize data into MasterBootRecord
                hdd.MBR = new MasterBootRecord(data);

                //Iterate over partitions in MBR
                foreach (PartitionTableEntry entry in hdd.MBR.PartitionTable.Partitions)
                {
                    //Clear data buffer
                    data = new byte[512];

                    //Seek to Partition start (FAT32 boot sector, location given in Partition table entry in Sectors)
                    disk.Seek((long)entry.LBA_Begin1 * BYTES_PER_SECTOR, SeekOrigin.Begin);

                    //Read FAT32 BootSector - Volume Info
                    disk.Read(data, 0, data.Length);

                    //Deserialize data into Partition object
                    hdd.Partitions.Add(new Partition(data, hdd, entry));                    
                }

                //Got to remember to close the disk
                disk.Close();
            }
        }

        /*
         * AddNode - Recurses over some root DirectoryEntry node and add's text to the treeViewDirectory
         * Returns - TreeNode created at current level of recursion
         */
        public TreeNode AddNode(DirectoryEntry directoryNode, TreeNode rootNode)
        {
            TreeNode node = null;
            if (rootNode == null)
            {
                node = new TreeNode(directoryNode.LongFilename == null ? directoryNode.ShortFilename : directoryNode.LongFilename);
            }
            else
            {
                node = new TreeNode(directoryNode.LongFilename == null ? directoryNode.ShortFilename : directoryNode.LongFilename);
                rootNode.Nodes.Add(node);
            }

            if (directoryNode.IsDirectory || directoryNode.IsVolumeID)
            {
                foreach (DirectoryEntry entry in directoryNode.Children)
                {
                    AddNode(entry, node);
                }
            }
            return node;
        }

        /*
         * Event handler when different partition is selected - forces tree refresh
         */
        private void comboBoxPartitions_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayPartition(((sender as ComboBox).SelectedItem as ComboBoxItem).Value);
        }

        /*
         * Event handler for refresh click
         */
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            displayPartition((comboBoxPartitions.SelectedItem as ComboBoxItem).Value);
        }

        /*
         * parseDirectoryTree - Start directory tree parsing from physical disk
         * Return - Enumerates partition's rootDirectory structure
         */
        private void parseDirectoryTree(Partition partition)
        {        
            //Create handle
            SafeFileHandle handle = Exports.CreateFile(partition.Hdd.DeviceId,
                            (uint)FileAccess.Read,
                            (uint)FileShare.None,
                            IntPtr.Zero,
                            (uint)FileMode.Open,
                            Exports.FILE_FLAG_NO_BUFFERING,
                            IntPtr.Zero);

            //Wrap handle in extended FileStream
            PreciseFileStream disk = new PreciseFileStream(handle, FileAccess.Read);

            //Hand off FileStream to workhorse
            partition.ParseDirectoryEntries(disk);

            //Don't forget to close
            disk.Close();
        }

        /*
         * Rebuild's the treeViewPartitions node structure based on the selected partition's rootDirectory structure 
         */
        private void displayPartition(object o)
        {
            Partition partition = null;
            if (o is Partition)
            {
                partition = o as Partition;
            }
            else
            {
                return;
            } 

            treeViewDirectory.Nodes.Clear();
            treeViewDirectory.Nodes.Add(AddNode(partition.RootDirectory, null));

        }

    }
}
