using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    /*
     * DirectoryEntryType - Enum of possible directory table entry types
     */
    public enum DirectoryEntryType
    {
        Normal,
        LongFileName,
        Unused,
        EndOfDirectory
    }
}
