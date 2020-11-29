namespace Nemezida.Rationalizator.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Nemezida.Rationalizator.Web.Extensions;
    using Nemezida.Rationalizator.Web.Services;

    [Route("api/Files")]
    [ApiController]
    public class FileStorageController : ControllerBase
    {
        private readonly IFileStorage _fileStorage;

        public FileStorageController(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        // GET: api/Files/logo.jpg
        [HttpGet("id{id}")]
        public ActionResult Get(long id)
        {
            var fileInfo = _fileStorage.GetById(id);
            if (fileInfo == null)
            {
                return this.NotFound();
            }

            return this.File(fileInfo.ReadStream, fileInfo.ContentType);
        }

        // POST: api/FileStorage
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Upload")]
        public async Task<ActionResult> Upload(IFormFile file)
        {
            var url = await this.UploadInternalAsync(file);
            return this.Ok(url);
        }

        // POST: api/FileStorage
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("UploadAll")]
        public async Task<ActionResult<IEnumerable<string>>> Upload(IFormFileCollection files)
        {
            var results = new List<string>();
            foreach (var file in files)
            {
                var url = await this.UploadInternalAsync(file);
                results.Add(url);
            }

            return this.Ok(results);
        }

        private async Task<string> UploadInternalAsync(IFormFile file)
        {
            var fileInfo = await _fileStorage.AddAsync(
                file.OpenReadStream(),
                file.ContentType,
                file.FileName);

            return fileInfo.ToUrl();
        }
    }
}
