using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    /*
     * HardDrive - Basic harddrive information with references to Partitions and MBR
     * 
     */
    public class HardDrive
    {

        #region Properties

        private string model = null;
        private string type = null;
        private string serialNo = null;
        private string deviceId = null;
        private MasterBootRecord _MBR;
        private List<Partition> partitions;

        public HardDrive()
        {
            partitions = new List<Partition>();
        }

        public List<Partition> Partitions
        {
            get { return partitions; }
            set { partitions = value; }
        }

        public MasterBootRecord MBR
        {
            get { return _MBR; }
            set { _MBR = value; }
        }

        public string DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }
        public string Model
        {
            get { return model; }
            set { model = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }

        #endregion

    }
}
