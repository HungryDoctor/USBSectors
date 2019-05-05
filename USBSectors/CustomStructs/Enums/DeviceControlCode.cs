namespace USBSectors.CustomStructs.Enums
{
    internal enum DeviceControlCode : uint
    {
        #region IOCTL

        STORAGE_QUERY_PROPERTY = 0x2d1400,
        STORAGE_GET_DEVICE_NUMBER = 0x2d1080,

        #endregion


        #region FSCTL

        LOCK_VOLUME = 0x00090018,
        DISMOUNT_VOLUME = 0x00090020,
        UNLOCK_VOLUME = 0x0009001C

        #endregion
    }
}
