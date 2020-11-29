namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    using System;
    using System.IO;

    /// <summary>
    /// Информация о хранимых файлах
    /// </summary>
    public class PersistentStorageFileInfoEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Название загруженного файла
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Версия файла
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Дата загрузки
        /// </summary>
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// Тип файла
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Поток чтения файла
        /// </summary>
        public Stream ReadStream => File.OpenRead(this.Path);

        /// <summary>
        /// Флаг, указывающий, существует ли файл
        /// </summary>
        public bool Exists => this.Path != null && File.Exists(this.Path);
    }
}
