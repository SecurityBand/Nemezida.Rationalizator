namespace Nemezida.Rationalizator.Web.Extensions
{
    using Nemezida.Rationalizator.Web.DataAccess.Models;

    public static class EntityExtensions
    {
        public static string ToUrl(this PersistentStorageFileInfoEntity fileInfo)
        {
            return "api/Files/" + fileInfo.ToName();
        }

        public static string ToName(this PersistentStorageFileInfoEntity fileInfo)
        {
            return "id" + fileInfo.Id;
        }

        public static long ToId(this string fileName)
        {
            return long.Parse(fileName.Substring(2));
        }
    }
}
