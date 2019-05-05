using System;

namespace USBSectors.Utils
{
    internal static class Extensions
    {
        public static bool TryConvertToEnum<T>(this ValueType valueType, out T enumValue) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), valueType))
            {
                enumValue = (T)valueType;
                return true;
            }
            else
            {
                enumValue = default;
                return false;
            }
        }
    }
}
