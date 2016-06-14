using System.Security.AccessControl;

namespace EffectiveRightsCheck.Core
{
    public static class FileSystemRightsEx
    {
        public static bool HasRights(this FileSystemRights rights, FileSystemRights testRights)
        {
            return (rights & testRights) == testRights;
        }
    }
}