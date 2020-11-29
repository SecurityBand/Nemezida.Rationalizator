namespace Nemezida.Rationalizator.Web.Controllers
{
    using AutoMapper;
    using AutoMapper.EntityFrameworkCore;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using Nemezida.Rationalizator.Web.DataAccess;
    using Nemezida.Rationalizator.Web.DataAccess.Models;
    using Nemezida.Rationalizator.Web.Extensions;
    using Nemezida.Rationalizator.Web.ViewModels;

    using Nest;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class IdeasController : ControllerBase
    {
        private readonly SystemDbContext _context;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public IdeasController(SystemDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            //var connection = new ConnectionSettings(new Uri(configuration["ElasticServerUri"])).DefaultIndex("ideas");
            //_elasticClient = new ElasticClient(connection);
        }

        [HttpGet("Tags")]
        public async Task<ActionResult<CollectionDTO<string>>> ListTags(int start = 0, int count = 30, TagsOrderMode orderBy = TagsOrderMode.Raiting)
        {
            if (orderBy == TagsOrderMode.Name)
            {
                var result = await _context.IdeaTags
                    .AsNoTracking()
                    .Skip(start)
                    .Take(count)
                    .Select(x => x.Name)
                    .OrderBy(x => x)
                    .ToListAsync();

                return this.CollectionResult(result);
            }
            else
            {
                var result = await _context.IdeaTags
                    .AsNoTracking()
                    .Include(x => x.Ideas)
                    .Select(x => new { Tag = x.Name, Raiting = x.Ideas.Count() })
                    .OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync();

                return this.CollectionResult(result.Select(x => x.Tag), result.Count);
            }
        }

        [HttpGet("Tags/Search/{str}")]
        public async Task<ActionResult<CollectionDTO<string>>> FindTags(string str, int start = 0, int count = 30, TagsOrderMode orderBy = TagsOrderMode.Raiting)
        {
            if (orderBy == TagsOrderMode.Name)
            {
                var result = await _context.IdeaTags
                    .AsNoTracking()
                    .Where(x => x.Name.Contains(str))
                    .Skip(start)
                    .Take(count)
                    .Select(x => x.Name)
                    .OrderBy(x => x)
                    .ToListAsync();

                return this.CollectionResult(result);
            }
            else
            {
                var result = await _context.IdeaTags
                    .AsNoTracking()
                    .Where(x => x.Name.Contains(str))
                    .Include(x => x.Ideas)
                    .Select(x => new { Tag = x.Name, Raiting = x.Ideas.Count() })
                    .OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync();

                return this.CollectionResult(result.Select(x => x.Tag), result.Count);
            }
        }

        [HttpGet]
        public async Task<ActionResult<CollectionDTO<IdeaViewDTO>>> List(int start = 0, int count = 30, OrderMode orderBy = OrderMode.Date)
        {
            List<IdeaViewDTO> ideas;
            if (orderBy == OrderMode.Date)
            {
                ideas = await _context.Ideas
                    .AsNoTracking()
                    .Include(x => x.Files)
                    .Include(x => x.Tags)
                    .OrderBy(x => x.CreatedDate)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<IdeaEntity, IdeaViewDTO>(_mapper);
            }
            else
            {
                ideas = await _context.Ideas
                    .AsNoTracking()
                    .Include(x => x.Files)
                    .Include(x => x.Tags)
                    .OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<IdeaEntity, IdeaViewDTO>(_mapper);
            }

            return this.CollectionResult(ideas);
        }

        [HttpGet("Search/{str}")]
        public async Task<ActionResult<CollectionDTO<IdeaViewDTO>>> Search(string str, int start = 0, int count = 30, OrderMode orderBy = OrderMode.Date)
        {
            List<IdeaViewDTO> ideas;
            if (orderBy == OrderMode.Date)
            {
                ideas = await _context.Ideas
                    .AsNoTracking()
                    .Include(x => x.Files)
                    .Include(x => x.Tags)
                    .Where(x => x.Text.Contains(str))
                    .OrderBy(x => x.CreatedDate)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<IdeaEntity, IdeaViewDTO>(_mapper);
            }
            else
            {
                ideas = await _context.Ideas
                    .AsNoTracking()
                    .Include(x => x.Files)
                    .Include(x => x.Tags)
                    .Where(x => x.Text.Contains(str))
                    .OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<IdeaEntity, IdeaViewDTO>(_mapper);
            }

            return this.CollectionResult(ideas);
        }

        [HttpGet("ByTag/{tag}")]
        public async Task<ActionResult<CollectionDTO<IdeaViewDTO>>> SearchByTag(string tag, int start = 0, int count = 30, OrderMode orderBy = OrderMode.Date)
        {
            var tagEntity = await _context.IdeaTags.FirstOrDefaultAsync(x => x.Name == tag);
            if (tagEntity == null)
            {
                return this.NotFound();
            }

            List<IdeaViewDTO> ideas = null;
            if (orderBy == OrderMode.Date)
            {
                ideas = await _context.Ideas
                    .Include(x => x.Files)
                    .Include(x => x.Tags)
                    .Where(x => x.Tags.Any(y => y.Name == tag))
                    .OrderBy(x => x.CreatedDate)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<IdeaEntity, IdeaViewDTO>(_mapper);
            }
            else
            {
                ideas = await _context.Ideas
                    .AsNoTracking()
                    .Include(x => x.Files)
                    .Include(x => x.Tags)
                    .Where(x => x.Tags.Any(y => y.Name == tag))
                    .OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<IdeaEntity, IdeaViewDTO>(_mapper);
            }


            return this.CollectionResult(ideas);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<IdDTO>> Add(AddIdeaDTO vm)
        {
            if (vm.Files != null && !_context.PersistentStorageFileInfos.All(x => vm.Files.Contains(x.Id)))
            {
                return this.NotFound();
            }

            var entity = await this.ToEntityAsync(vm);

            _context.Ideas.Add(entity);
            await _context.SaveChangesAsync();

            //await _elasticClient.IndexDocumentAsync(vm);

            return this.IdResult(entity.Id);
        }

        [HttpPost("{ideaId}/Edit")]
        public async Task<ActionResult> Edit(long ideaId, EditIdeaDTO vm)
        {
            var idea = await _context.Ideas.FindAsync(ideaId);
            if (idea == null)
            {
                return this.NotFound();
            }

            idea = _mapper.Map(vm, idea);

            _context.Ideas.Update(idea);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("{ideaId}/Delete")]
        public async Task<ActionResult> Delete(long ideaId)
        {
            var idea = await _context.Ideas.FindAsync(ideaId);
            if (idea == null)
            {
                return this.NotFound();
            }

            _context.Ideas.Remove(idea);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("{ideaId}/AddTag")]
        public async Task<ActionResult> AddTag(long ideaId, string tag)
        {
            var idea = _context.Ideas.Include(x => x.Tags).FirstOrDefault(x => x.Id == ideaId);
            if (idea == null)
            {
                return this.NotFound();
            }

            if (!idea.Tags.Any(x => x.Name == tag))
            {
                var tagEntity = await _context.IdeaTags.FirstOrDefaultAsync(x => x.Name == tag);
                if (tagEntity == null)
                {
                    tagEntity = new IdeaTagEntity { Name = tag };
                    _context.IdeaTags.Add(tagEntity);
                }

                idea.Tags.Add(tagEntity);
            }

            _context.Ideas.Update(idea);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("{ideaId}/RemoveTag")]
        public async Task<ActionResult> RemoveTag(long ideaId, string tag)
        {
            var idea = await this._context.Ideas.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == ideaId);
            if (idea == null)
            {
                return this.NotFound();
            }

            var tagEntity = idea.Tags.FirstOrDefault(x => x.Name == tag);
            if (tagEntity == null)
            {
                return this.NotFound();
            }

            _context.IdeaTags.Remove(tagEntity);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("{ideaId}/Vote")]
        public async Task<ActionResult> Vote(long ideaId)
        {
            var idea = await this._context.Ideas.FirstOrDefaultAsync(x => x.Id == ideaId);
            if (idea == null)
            {
                return this.NotFound();
            }

            idea.Raiting++;

            _context.Ideas.Update(idea);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        private async Task<IdeaEntity> ToEntityAsync(AddIdeaDTO vm)
        {
            var existsTags = await _context.IdeaTags
                .Where(x => vm.Tags.Any(y => x.Name == y))
                .ToListAsync();

            var newTags = vm.Tags
                .Except(existsTags.Select(x => x.Name))
                .Select(x => new IdeaTagEntity { Name = x })
                .ToList();

            existsTags.AddRange(newTags);            

            return new IdeaEntity
            {
                Text = vm.Text,
                Tags = existsTags,
                Files = vm.Files?.Select(x =>  new PersistentStorageFileInfoEntity { Id = x }).ToList(),
                CreatedDate = DateTime.Now
            };
        }
    }
}
