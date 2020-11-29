namespace Nemezida.Rationalizator.Web.Controllers
{
    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using Nemezida.Rationalizator.Web.DataAccess;
    using Nemezida.Rationalizator.Web.DataAccess.Models;
    using Nemezida.Rationalizator.Web.Extensions;
    using Nemezida.Rationalizator.Web.ViewModels;
    using Nemezida.Rationalizator.Web.ViewModels.RationalOffers;

    using Nest;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class RationalOfferController : ControllerBase
    {
        private readonly SystemDbContext _context;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public RationalOfferController(SystemDbContext context, IConfiguration configuration, IMapper mapper)
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
                var result = await _context.RationalOfferTags
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
                var result = await _context.RationalOfferTags
                    .AsNoTracking()
                    .Include(x => x.RationalOffers)
                    .Select(x => new { Tag = x.Name, Raiting = x.RationalOffers.Count() })
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
                var result = await _context.RationalOfferTags
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
                var result = await _context.RationalOfferTags
                    .AsNoTracking()
                    .Where(x => x.Name.Contains(str))
                    .Include(x => x.RationalOffers)
                    .Select(x => new { Tag = x.Name, Raiting = x.RationalOffers.Count() })
                    .OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync();

                return this.CollectionResult(result.Select(x => x.Tag), result.Count);
            }
        }

        [HttpGet]
        public async Task<ActionResult<CollectionDTO<RationalOfferListDTO>>> List(int start = 0, int count = 30, OrderMode orderBy = OrderMode.Raiting)
        {
            List<RationalOfferListDTO> ideas;
            if (orderBy == OrderMode.Date)
            {
                ideas = await _context.RationalOffers
                    .AsNoTracking()
                    .Include(x => x.Files)
                    .OrderBy(x => x.CreationDate)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<RationalOfferEntity, RationalOfferListDTO>(_mapper);
            }
            else
            {
                ideas = await _context.RationalOffers
                    .AsNoTracking()
                    .Include(x => x.Files)
                    .OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<RationalOfferEntity, RationalOfferListDTO>(_mapper);
            }

            return this.CollectionResult(ideas);
        }

        [HttpGet("Search/{str}")]
        public async Task<ActionResult<CollectionDTO<RationalOfferListDTO>>> Search(string str, int start = 0, int count = 30, OrderMode orderBy = OrderMode.Date)
        {
            List<RationalOfferListDTO> ideas;
            if (orderBy == OrderMode.Date)
            {
                ideas = await _context.RationalOffers
                    .AsNoTracking()
                    .Include(x => x.Files)
                    .Include(x => x.Tags)
                    .Where(x => x.Text.Contains(str))
                    .OrderBy(x => x.CreationDate)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<RationalOfferEntity, RationalOfferListDTO>(_mapper);
            }
            else
            {
                ideas = await _context.RationalOffers
                    .AsNoTracking()
                    .Include(x => x.Files)
                    .Include(x => x.Tags)
                    .Where(x => x.Text.Contains(str))
                    .OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<RationalOfferEntity, RationalOfferListDTO>(_mapper);
            }

            return this.CollectionResult(ideas);
        }

        [HttpGet("ByTag/{tag}")]
        public async Task<ActionResult<CollectionDTO<RationalOfferListDTO>>> SearchByTag(string tag, int start = 0, int count = 30, OrderMode orderBy = OrderMode.Date)
        {
            var tagEntity = await _context.IdeaTags.FirstOrDefaultAsync(x => x.Name == tag);
            if (tagEntity == null)
            {
                return this.NotFound();
            }

            List<RationalOfferListDTO> ideas = null;
            if (orderBy == OrderMode.Date)
            {
                ideas = await _context.RationalOffers
                    .Include(x => x.Files)
                    .Include(x => x.Tags)
                    .Where(x => x.Tags.Any(y => y.Name == tag))
                    .OrderBy(x => x.CreationDate)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<RationalOfferEntity, RationalOfferListDTO>(_mapper);
            }
            else
            {
                ideas = await _context.RationalOffers
                    .AsNoTracking()
                    .Include(x => x.Files)
                    .Include(x => x.Tags)
                    .Where(x => x.Tags.Any(y => y.Name == tag))
                    .OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<RationalOfferEntity, RationalOfferListDTO>(_mapper);
            }


            return this.CollectionResult(ideas);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<IdDTO>> Add(AddRationalOfferDTO vm)
        {
            if (vm.Files != null && !_context.PersistentStorageFileInfos.All(x => vm.Files.Contains(x.Id)))
            {
                return this.NotFound();
            }

            var entity = await this.ToEntityAsync(vm);

            _context.RationalOffers.Add(entity);
            await _context.SaveChangesAsync();

            //await _elasticClient.IndexDocumentAsync(vm);

            return this.IdResult(entity.Id);
        }

        [HttpPost("{rationalOfferId}/Edit")]
        public async Task<ActionResult> Edit(long rationalOfferId, AddRationalOfferDTO vm)
        {
            var rationalOffer = await _context.RationalOffers.FindAsync(rationalOfferId);
            if (rationalOffer == null)
            {
                return this.NotFound();
            }

            rationalOffer = _mapper.Map(vm, rationalOffer);

            _context.RationalOffers.Update(rationalOffer);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("{rationalOfferId}/Delete")]
        public async Task<ActionResult> Delete(long rationalOfferId)
        {
            var rationalOffer = await _context.RationalOffers.FindAsync(rationalOfferId);
            if (rationalOffer == null)
            {
                return this.NotFound();
            }

            _context.RationalOffers.Remove(rationalOffer);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("{rationalOfferId}/AddTag")]
        public async Task<ActionResult> AddTag(long rationalOfferId, string tag)
        {
            var rationalOffer = _context.RationalOffers.Include(x => x.Tags).FirstOrDefault(x => x.Id == rationalOfferId);
            if (rationalOffer == null)
            {
                return this.NotFound();
            }

            if (!rationalOffer.Tags.Any(x => x.Name == tag))
            {
                var tagEntity = await _context.RationalOfferTags.FirstOrDefaultAsync(x => x.Name == tag);
                if (tagEntity == null)
                {
                    tagEntity = new RationalOfferTagEntity { Name = tag };
                    _context.RationalOfferTags.Add(tagEntity);
                }

                rationalOffer.Tags.Add(tagEntity);
            }

            _context.RationalOffers.Update(rationalOffer);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("{rationalOfferId}/RemoveTag")]
        public async Task<ActionResult> RemoveTag(long rationalOfferId, string tag)
        {
            var rationalOffer = await this._context.RationalOffers.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == rationalOfferId);
            if (rationalOffer == null)
            {
                return this.NotFound();
            }

            var tagEntity = rationalOffer.Tags.FirstOrDefault(x => x.Name == tag);
            if (tagEntity == null)
            {
                return this.NotFound();
            }

            _context.RationalOfferTags.Remove(tagEntity);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("{ideaId}/Vote")]
        public async Task<ActionResult> Vote(long ideaId)
        {
            var idea = await this._context.RationalOffers.FirstOrDefaultAsync(x => x.Id == ideaId);
            if (idea == null)
            {
                return this.NotFound();
            }

            idea.Raiting++;

            _context.RationalOffers.Update(idea);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        private async Task<RationalOfferEntity> ToEntityAsync(AddRationalOfferDTO vm)
        {
            var existsTags = await _context.RationalOfferTags
                .Where(x => vm.Tags.Any(y => x.Name == y))
                .ToListAsync();

            var newTags = vm.Tags
                .Except(existsTags.Select(x => x.Name))
                .Select(x => new RationalOfferTagEntity { Name = x })
                .ToList();

            existsTags.AddRange(newTags);

            return new RationalOfferEntity
            {
                Title = vm.Title,
                Text = vm.Text,
                Tags = existsTags,
                Files = vm.Files?.Select(x => new PersistentStorageFileInfoEntity { Id = x }).ToList(),
                CreationDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                // TODO: FIeld values
            };
        }
    }
}
