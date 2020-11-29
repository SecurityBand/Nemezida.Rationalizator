namespace Nemezida.Rationalizator.Web.Impl
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using Nemezida.Rationalizator.Web.DataAccess;
    using Nemezida.Rationalizator.Web.DataAccess.Models;
    using Nemezida.Rationalizator.Web.Services;

    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Сервис локального хранилища файлов
    /// </summary>
    public class LocalFileStorage : IFileStorage
    {
        private readonly SystemDbContext _dbContext;

        public LocalFileStorage(SystemDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            this.RootDir = configuration["FileStorageRoot"];
        }

        public string RootDir { get; set; }

        public async Task<PersistentStorageFileInfoEntity> AddAsync(Stream contentStream, string contentType, string name, CancellationToken cancellationToken = default)
        {
            contentStream.Position = 0;

            var lastVersionFileInfo = await this.GetLastVersionAsync(name, cancellationToken);
            var newVersion = lastVersionFileInfo == null
                ? 0
                : lastVersionFileInfo.Version + 1;

            var model = new PersistentStorageFileInfoEntity
            {
                ContentType = contentType,
                Name = name,
                Version = newVersion,
                UploadDate = DateTime.Now,
                Path = LocalFileStorage.GenerateFilePath(this.RootDir)
            };

            Directory.CreateDirectory(this.RootDir);

            var fileStream = new FileStream(model.Path, FileMode.Create);
            fileStream.Position = 0;

            await contentStream.CopyToAsync(fileStream);
            await contentStream.DisposeAsync();
            await fileStream.FlushAsync();
            await fileStream.DisposeAsync();

            _dbContext.PersistentStorageFileInfos.Add(model);
            await _dbContext.SaveChangesAsync();

            return model;
        }

        public IQueryable<PersistentStorageFileInfoEntity> Find()
        {
            return _dbContext.PersistentStorageFileInfos;
        }

        public Task<PersistentStorageFileInfoEntity> GetAsync(string name, int version, CancellationToken cancellationToken = default)
        {
            return this.Find()
                .FirstOrDefaultAsync(x => x.Name == name && x.Version == version, cancellationToken);
        }

        public Task<PersistentStorageFileInfoEntity> GetLastVersionAsync(string name, CancellationToken cancellationToken = default)
        {
            return this.Find()
                .Where(x => x.Name == name)
                .OrderByDescending(x => x.Version)
                .Take(1)
                .FirstOrDefaultAsync(cancellationToken);
        }

        private static string GenerateFilePath(string root)
        {
            while (true)
            {
                var path = Path.Combine(root, Path.GetRandomFileName());
                if (File.Exists(path))
                {
                    continue;
                }

                return path;
            }
        }

        public PersistentStorageFileInfoEntity GetById(long id)
        {
            return this._dbContext.PersistentStorageFileInfos.Find(id);
        }
    }
}
