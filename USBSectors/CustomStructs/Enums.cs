using System;

namespace USBSectors.CustomStructs
{
    public enum HKL : uint
    {
        HKL_PREV = 0,
        HKL_NEXT = 1
    }

    [Flags]
    public enum KLF : uint
    {
        KLF_ACTIVATE = 0x00000001,
        KLF_SUBSTITUTE_OK = 0x00000002,
        KLF_REORDER = 0x00000008,
        KLF_REPLACELANG = 0x00000010,
        KLF_NOTELLSHELL = 0x00000080,
        KLF_SETFORPROCESS = 0x00000100,
        KLF_SHIFTLOCK = 0x00010000,
        KLF_RESET = 0x40000000
    }

    public enum LanguageId : uint
    {
        LANGUAGE_ID_EN_GB = 0x0809,
        LANGUAGE_ID_EN_US = 0x0409,
        LANGUAGE_ID_RU_RUS = 0x0419,
        LANGUAGE_ID_UK_UA = 0x0422
    }


    [Flags]
    public enum FlashWSettings : uint
    {
        FLASHW_STOP = 0,
        FLASHW_CAPTION = 1,
        FLASHW_TRAY = 2,
        FLASHW_ALL = 3,
        FLASHW_TIMER = 4,
        FLASHW_TIMERNOFG = 12
    }


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


    [Flags]
    public enum DiGetClassFlags : uint
    {
        DIGCF_DEFAULT = 0x00000001,  // only valid with DIGCF_DEVICEINTERFACE
        DIGCF_PRESENT = 0x00000002,
        DIGCF_ALLCLASSES = 0x00000004,
        DIGCF_PROFILE = 0x00000008,
        DIGCF_DEVICEINTERFACE = 0x00000010,
    }


    public enum DEVICE_TYPE : uint
    {
        FILE_DEVICE_BEEP = 0x01,
        FILE_DEVICE_CD_ROM = 0x02,
        FILE_DEVICE_CD_ROM_FILE_SYSTEM = 0x03,
        FILE_DEVICE_CONTROLLER = 0x04,
        FILE_DEVICE_DATALINK = 0x05,
        FILE_DEVICE_DFS = 0x06,
        FILE_DEVICE_DISK = 0x07, // IOCTL_DISK_BASE
        FILE_DEVICE_DISK_FILE_SYSTEM = 0x08,
        FILE_DEVICE_FILE_SYSTEM = 0x09,
        FILE_DEVICE_INPORT_PORT = 0x0a,
        FILE_DEVICE_KEYBOARD = 0x0b,
        FILE_DEVICE_MAILSLOT = 0x0c,
        FILE_DEVICE_MIDI_IN = 0x0d,
        FILE_DEVICE_MIDI_OUT = 0x0e,
        FILE_DEVICE_MOUSE = 0x0f,
        FILE_DEVICE_MULTI_UNC_PROVIDER = 0x10,
        FILE_DEVICE_NAMED_PIPE = 0x11,
        FILE_DEVICE_NETWORK = 0x12,
        FILE_DEVICE_NETWORK_BROWSER = 0x13,
        FILE_DEVICE_NETWORK_FILE_SYSTEM = 0x14,
        FILE_DEVICE_NULL = 0x15,
        FILE_DEVICE_PARALLEL_PORT = 0x16,
        FILE_DEVICE_PHYSICAL_NETCARD = 0x17,
        FILE_DEVICE_PRINTER = 0x18,
        FILE_DEVICE_SCANNER = 0x19,
        FILE_DEVICE_SERIAL_MOUSE_PORT = 0x1a,
        FILE_DEVICE_SERIAL_PORT = 0x1b,
        FILE_DEVICE_SCREEN = 0x1c,
        FILE_DEVICE_SOUND = 0x1d,
        FILE_DEVICE_STREAMS = 0x1e,
        FILE_DEVICE_TAPE = 0x1f,
        FILE_DEVICE_TAPE_FILE_SYSTEM = 0x20,
        FILE_DEVICE_TRANSPORT = 0x21,
        FILE_DEVICE_UNKNOWN = 0x22,
        FILE_DEVICE_VIDEO = 0x23,
        FILE_DEVICE_VIRTUAL_DISK = 0x24,
        FILE_DEVICE_WAVE_IN = 0x25,
        FILE_DEVICE_WAVE_OUT = 0x26,
        FILE_DEVICE_8042_PORT = 0x27,
        FILE_DEVICE_NETWORK_REDIRECTOR = 0x28,
        FILE_DEVICE_BATTERY = 0x29,
        FILE_DEVICE_BUS_EXTENDER = 0x2a,
        FILE_DEVICE_MODEM = 0x2b,
        FILE_DEVICE_VDM = 0x2c,
        FILE_DEVICE_MASS_STORAGE = 0x2d, // IOCTL_STORAGE_BASE
        FILE_DEVICE_SMB = 0x2e,
        FILE_DEVICE_KS = 0x2f,
        FILE_DEVICE_CHANGER = 0x30, // IOCTL_CHANGER_BASE
        FILE_DEVICE_SMARTCARD = 0x31,
        FILE_DEVICE_ACPI = 0x32,
        FILE_DEVICE_DVD = 0x33,
        FILE_DEVICE_FULLSCREEN_VIDEO = 0x34,
        FILE_DEVICE_DFS_FILE_SYSTEM = 0x35,
        FILE_DEVICE_DFS_VOLUME = 0x36,
        FILE_DEVICE_SERENUM = 0x37,
        FILE_DEVICE_TERMSRV = 0x38,
        FILE_DEVICE_KSEC = 0x39,
        FILE_DEVICE_FIPS = 0x3A,
        FILE_DEVICE_INFINIBAND = 0x3B,
        FILE_DEVICE_VMBUS = 0x3E,
        FILE_DEVICE_CRYPT_PROVIDER = 0x3F,
        FILE_DEVICE_WPD = 0x40,
        FILE_DEVICE_BLUETOOTH = 0x41,
        FILE_DEVICE_MT_COMPOSITE = 0x42,
        FILE_DEVICE_MT_TRANSPORT = 0x43,
        FILE_DEVICE_BIOMETRIC = 0x44,
        FILE_DEVICE_PMI = 0x45,
        FILE_DEVICE_EHSTOR = 0x46,
        FILE_DEVICE_DEVAPI = 0x47,
        FILE_DEVICE_GPIO = 0x48,
        FILE_DEVICE_USBEX = 0x49,
        FILE_DEVICE_CONSOLE = 0x50,
        FILE_DEVICE_NFP = 0x51,
        FILE_DEVICE_SYSENV = 0x52,
        FILE_DEVICE_VIRTUAL_BLOCK = 0x53,
        FILE_DEVICE_POINT_OF_SERVICE = 0x54,
        FILE_DEVICE_STORAGE_REPLICATION = 0x55,
        FILE_DEVICE_TRUST_ENV = 0x56 // IOCTL_VOLUME_BASE
    }

    public enum STORAGE_BUS_TYPE
    {
        BusTypeUnknown = 0x00,
        BusTypeScsi = 0x1,
        BusTypeAtapi = 0x2,
        BusTypeAta = 0x3,
        BusType1394 = 0x4,
        BusTypeSsa = 0x5,
        BusTypeFibre = 0x6,
        BusTypeUsb = 0x7,
        BusTypeRAID = 0x8,
        BusTypeiScsi = 0x9,
        BusTypeSas = 0xA,
        BusTypeSata = 0xB,
        BusTypeSd = 0xC,
        BusTypeMmc = 0xD,
        BusTypeVirtual = 0xE,
        BusTypeFileBackedVirtual = 0xF,
        BusTypeMax = 0x10,
        BusTypeMaxReserved = 0x7F
    }


    public enum FATxType
    {
        Fat12_16,
        Fat32,
    }


    public enum EMoveMethod : uint
    {
        Begin = 0,
        Current = 1,
        End = 2
    }


    #region File

    [Flags]
    public enum EFileAccess : uint
    {
        None = 0,
        //
        // Standart Section
        //
        AccessSystemSecurity = 0x1000000,   // AccessSystemAcl access type
        MaximumAllowed = 0x2000000,     // MaximumAllowed access type

        Delete = 0x10000,
        ReadControl = 0x20000,
        WriteDAC = 0x40000,
        WriteOwner = 0x80000,
        Synchronize = 0x100000,

        StandardRightsRequired = 0xF0000,
        StandardRightsRead = ReadControl,
        StandardRightsWrite = ReadControl,
        StandardRightsExecute = ReadControl,
        StandardRightsAll = 0x1F0000,
        SpecificRightsAll = 0xFFFF,

        FILE_READ_DATA = 0x0001,        // file & pipe
        FILE_LIST_DIRECTORY = 0x0001,       // directory
        FILE_WRITE_DATA = 0x0002,       // file & pipe
        FILE_ADD_FILE = 0x0002,         // directory
        FILE_APPEND_DATA = 0x0004,      // file
        FILE_ADD_SUBDIRECTORY = 0x0004,     // directory
        FILE_CREATE_PIPE_INSTANCE = 0x0004, // named pipe
        FILE_READ_EA = 0x0008,          // file & directory
        FILE_WRITE_EA = 0x0010,         // file & directory
        FILE_EXECUTE = 0x0020,          // file
        FILE_TRAVERSE = 0x0020,         // directory
        FILE_DELETE_CHILD = 0x0040,     // directory
        FILE_READ_ATTRIBUTES = 0x0080,      // all
        FILE_WRITE_ATTRIBUTES = 0x0100,     // all

        //
        // Generic Section
        //

        GenericRead = 0x80000000,
        GenericWrite = 0x40000000,
        GenericExecute = 0x20000000,
        GenericAll = 0x10000000,

        SPECIFIC_RIGHTS_ALL = 0x00FFFF,
        FILE_ALL_ACCESS =
        StandardRightsRequired |
        Synchronize |
        0x1FF,

        FILE_GENERIC_READ = StandardRightsRead | FILE_READ_DATA | FILE_READ_ATTRIBUTES | FILE_READ_EA | Synchronize,

        FILE_GENERIC_WRITE = StandardRightsWrite | FILE_WRITE_DATA | FILE_WRITE_ATTRIBUTES | FILE_WRITE_EA | FILE_APPEND_DATA | Synchronize,

        FILE_GENERIC_EXECUTE = StandardRightsExecute | FILE_READ_ATTRIBUTES | FILE_EXECUTE | Synchronize
    }

    [Flags]
    public enum EFileShare : uint
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0x00000000,
        /// <summary>
        /// Enables subsequent open operations on an object to request read access. 
        /// Otherwise, other processes cannot open the object if they request read access. 
        /// If this flag is not specified, but the object has been opened for read access, the function fails.
        /// </summary>
        Read = 0x00000001,
        /// <summary>
        /// Enables subsequent open operations on an object to request write access. 
        /// Otherwise, other processes cannot open the object if they request write access. 
        /// If this flag is not specified, but the object has been opened for write access, the function fails.
        /// </summary>
        Write = 0x00000002,
        /// <summary>
        /// Enables subsequent open operations on an object to request delete access. 
        /// Otherwise, other processes cannot open the object if they request delete access.
        /// If this flag is not specified, but the object has been opened for delete access, the function fails.
        /// </summary>
        Delete = 0x00000004
    }

    public enum ECreationDisposition : uint
    {
        /// <summary>
        /// Creates a new file. The function fails if a specified file exists.
        /// </summary>
        New = 1,
        /// <summary>
        /// Creates a new file, always. 
        /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes, 
        /// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
        /// </summary>
        CreateAlways = 2,
        /// <summary>
        /// Opens a file. The function fails if the file does not exist. 
        /// </summary>
        OpenExisting = 3,
        /// <summary>
        /// Opens a file, always. 
        /// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
        /// </summary>
        OpenAlways = 4,
        /// <summary>
        /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
        /// The calling process must open the file with the GENERIC_WRITE access right. 
        /// </summary>
        TruncateExisting = 5
    }

    [Flags]
    public enum EFileAttributes : uint
    {
        None = 0,
        Readonly = 0x00000001,
        Hidden = 0x00000002,
        System = 0x00000004,
        Directory = 0x00000010,
        Archive = 0x00000020,
        Device = 0x00000040,
        Normal = 0x00000080,
        Temporary = 0x00000100,
        SparseFile = 0x00000200,
        ReparsePoint = 0x00000400,
        Compressed = 0x00000800,
        Offline = 0x00001000,
        NotContentIndexed = 0x00002000,
        Encrypted = 0x00004000,
        Write_Through = 0x80000000,
        Overlapped = 0x40000000,
        NoBuffering = 0x20000000,
        RandomAccess = 0x10000000,
        SequentialScan = 0x08000000,
        DeleteOnClose = 0x04000000,
        BackupSemantics = 0x02000000,
        PosixSemantics = 0x01000000,
        OpenReparsePoint = 0x00200000,
        OpenNoRecall = 0x00100000,
        FirstPipeInstance = 0x00080000
    }

    #endregion



    [Flags]
    public enum EFileSystemFeature : uint
    {
        /// <summary>
        /// The file system preserves the case of file names when it places a name on disk.
        /// </summary>
        CasePreservedNames = 2,

        /// <summary>
        /// The file system supports case-sensitive file names.
        /// </summary>
        CaseSensitiveSearch = 1,

        /// <summary>
        /// The specified volume is a direct access (DAX) volume. This flag was introduced in Windows 10, version 1607.
        /// </summary>
        DaxVolume = 0x20000000,

        /// <summary>
        /// The file system supports file-based compression.
        /// </summary>
        FileCompression = 0x10,

        /// <summary>
        /// The file system supports named streams.
        /// </summary>
        NamedStreams = 0x40000,

        /// <summary>
        /// The file system preserves and enforces access control lists (ACL).
        /// </summary>
        PersistentACLS = 8,

        /// <summary>
        /// The specified volume is read-only.
        /// </summary>
        ReadOnlyVolume = 0x80000,

        /// <summary>
        /// The volume supports a single sequential write.
        /// </summary>
        SequentialWriteOnce = 0x100000,

        /// <summary>
        /// The file system supports the Encrypted File System (EFS).
        /// </summary>
        SupportsEncryption = 0x20000,

        /// <summary>
        /// The specified volume supports extended attributes. An extended attribute is a piece of
        /// application-specific metadata that an application can associate with a file and is not part
        /// of the file's data.
        /// </summary>
        SupportsExtendedAttributes = 0x00800000,

        /// <summary>
        /// The specified volume supports hard links. For more information, see Hard Links and Junctions.
        /// </summary>
        SupportsHardLinks = 0x00400000,

        /// <summary>
        /// The file system supports object identifiers.
        /// </summary>
        SupportsObjectIDs = 0x10000,

        /// <summary>
        /// The file system supports open by FileID. For more information, see FILE_ID_BOTH_DIR_INFO.
        /// </summary>
        SupportsOpenByFileId = 0x01000000,

        /// <summary>
        /// The file system supports re-parse points.
        /// </summary>
        SupportsReparsePoints = 0x80,

        /// <summary>
        /// The file system supports sparse files.
        /// </summary>
        SupportsSparseFiles = 0x40,

        /// <summary>
        /// The volume supports transactions.
        /// </summary>
        SupportsTransactions = 0x200000,

        /// <summary>
        /// The specified volume supports update sequence number (USN) journals. For more information,
        /// see Change Journal Records.
        /// </summary>
        SupportsUsnJournal = 0x02000000,

        /// <summary>
        /// The file system supports Unicode in file names as they appear on disk.
        /// </summary>
        UnicodeOnDisk = 4,

        /// <summary>
        /// The specified volume is a compressed volume, for example, a DoubleSpace volume.
        /// </summary>
        VolumeIsCompressed = 0x8000,

        /// <summary>
        /// The file system supports disk quotas.
        /// </summary>
        VolumeQuotas = 0x20
    }
}
