namespace USBSectors.CustomStructs
{
    public class BootSectorInfo
    {
        public string FileSystemName_Parsed;
        public string FileSystemName_Query;
        public ulong VolumeSerialNumber;

        public uint BytesPerSector;
        public uint SectorsPerCluster;
        public uint ReservedSectors;
    }
}
