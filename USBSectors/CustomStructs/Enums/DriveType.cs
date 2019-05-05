namespace USBSectors.CustomStructs.Enums
{
    public enum DriveType : uint
    {
        /// <summary>The drive type cannot be determined.</summary>
        Unknown = 0,    //DRIVE_UNKNOWN

        /// <summary>The root path is invalid, for example, no volume is mounted at the path.</summary>
        Error = 1,        //DRIVE_NO_ROOT_DIR

        /// <summary>The drive is a type that has removable media, for example, a floppy drive or removable hard disk.</summary>
        Removable = 2,    //DRIVE_REMOVABLE

        /// <summary>The drive is a type that cannot be removed, for example, a fixed hard drive.</summary>
        Fixed = 3,        //DRIVE_FIXED

        /// <summary>The drive is a remote (network) drive.</summary>
        Remote = 4,        //DRIVE_REMOTE

        /// <summary>The drive is a CD-ROM drive.</summary>
        CDROM = 5,        //DRIVE_CDROM

        /// <summary>The drive is a RAM disk.</summary>
        RAMDisk = 6        //DRIVE_RAMDISK
    }
}
