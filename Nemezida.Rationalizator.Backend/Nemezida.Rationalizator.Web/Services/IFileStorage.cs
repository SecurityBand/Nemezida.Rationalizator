namespace Nemezida.Rationalizator.Web.Services
{
    using Nemezida.Rationalizator.Web.DataAccess.Models;

    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Сервис хранилища файлов
    /// </summary>
    public interface IFileStorage
    {
        /// <summary>
        /// Добавляет новый файл в хранилище
        /// </summary>
        /// <param name="contentStream">Поток данных файла</param>
        /// <param name="contentType">Тип файла</param>
        /// <param name="name">Название файла</param>
        /// <param name="showInDocs">Отображать ли файл в библиотеке документов</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Описатель сохраненого файла</returns>
        Task<PersistentStorageFileInfoEntity> AddAsync(Stream contentStream, string contentType, string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает запрос для поиска файла в хранилище
        /// </summary>
        /// <returns>Запрос для поиска файла в хранилище</returns>
        IQueryable<PersistentStorageFileInfoEntity> Find();

        /// <summary>
        /// Получает файл по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор файла</param>
        /// <returns>Описатель найденного файла или null, если файл не найден</returns>
        PersistentStorageFileInfoEntity GetById(long id);

        /// <summary>
        /// Получает файл указанной версии
        /// </summary>
        /// <param name="name">Название файла</param>
        /// <param name="version">Версия файла</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Описатель найденного файла или null, если файл не найден</returns>
        Task<PersistentStorageFileInfoEntity> GetAsync(string name, int version, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает файл последней версии
        /// </summary>
        /// <param name="name">Название файла</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Описатель найденного файла или null, если файл не найден</returns>
        Task<PersistentStorageFileInfoEntity> GetLastVersionAsync(string name, CancellationToken cancellationToken = default);
    }
}
