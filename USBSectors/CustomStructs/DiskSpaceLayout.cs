namespace USBSectors.CustomStructs
{
    public struct DiskSpaceLayout
    {
        public uint lpSectorsPerCluster;
        public uint lpBytesPerSector;
        public uint lpNumberOfFreeClusters;
        public uint lpTotalNumberOfClusters;
    }
}
