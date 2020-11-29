namespace Nemezida.Rationalizator.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Nemezida.Rationalizator.Web.DataAccess;
    using Nemezida.Rationalizator.Web.DataAccess.Models;
    using Nemezida.Rationalizator.Web.Extensions;
    using Nemezida.Rationalizator.Web.ViewModels;
    using Nemezida.Rationalizator.Web.ViewModels.Article;

    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly SystemDbContext _context;
        private readonly IMapper _mapper;

        public ArticlesController(SystemDbContext context, IMapper mapper)
        {
            _context = context;
        }

        [HttpGet("Tags")]
        public async Task<ActionResult<CollectionDTO<string>>> ListTags(int start = 0, int count = 30, TagsOrderMode orderBy = TagsOrderMode.Raiting)
        {
            if (orderBy == TagsOrderMode.Name)
            {
                var result = await _context.ArticleTags
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
                var result = await _context.ArticleTags
                    .AsNoTracking()
                    .Include(x => x.Articles)
                    .Select(x => new { Tag = x.Name, Raiting = x.Articles.Count() })
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
                var result = await _context.ArticleTags
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
                var result = await _context.ArticleTags
                    .AsNoTracking()
                    .Where(x => x.Name.Contains(str))
                    .Include(x => x.Articles)
                    .Select(x => new { Tag = x.Name, Raiting = x.Articles.Count() })
                    .OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync();

                return this.CollectionResult(result.Select(x => x.Tag), result.Count);
            }
        }

        [HttpGet]
        public async Task<ActionResult<CollectionDTO<ArticleListDTO>>> List(long? categoryId = null, int start = 0, int count = 30, OrderMode orderBy = OrderMode.Date)
        {
            var query = _context.Articles
                .AsNoTracking()
                .Include(x => x.Files) as IQueryable<ArticleEntity>;

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            List<ArticleListDTO> articles;
            if (orderBy == OrderMode.Date)
            {
                articles = await query.OrderBy(x => x.CreatedDate)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<ArticleEntity, ArticleListDTO>(_mapper);
            }
            else
            {
                articles = await query.OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<ArticleEntity, ArticleListDTO>(_mapper);
            }

            return this.CollectionResult(articles);
        }

        [HttpGet("Search/{str}")]
        public async Task<ActionResult<CollectionDTO<ArticleListDTO>>> Search(string str, long? categoryId = null, int start = 0, int count = 30, OrderMode orderBy = OrderMode.Date)
        {
            var query = _context.Articles
                .AsNoTracking()
                .Include(x => x.Files) as IQueryable<ArticleEntity>;

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            List<ArticleListDTO> articles;
            if (orderBy == OrderMode.Date)
            {
                articles = await query.Where(x => x.Text.Contains(str))
                    .OrderBy(x => x.CreatedDate)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<ArticleEntity, ArticleListDTO>(_mapper);
            }
            else
            {
                articles = await query.Where(x => x.Text.Contains(str))
                    .OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<ArticleEntity, ArticleListDTO>(_mapper);
            }

            return this.CollectionResult(articles);
        }

        [HttpGet("ByTag/{tag}")]
        public async Task<ActionResult<CollectionDTO<ArticleListDTO>>> SearchByTag(string tag, long? categoryId = null, int start = 0, int count = 30, OrderMode orderBy = OrderMode.Date)
        {
            var tagEntity = await _context.ArticleTags.FirstOrDefaultAsync(x => x.Name == tag);
            if (tagEntity == null)
            {
                return this.NotFound();
            }

            var query = _context.Articles
                .Include(x => x.Files)
                .Include(x => x.Tags)
                .Where(x => x.Tags.Any(y => y.Name == tag));

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            List<ArticleListDTO> articles = null;
            if (orderBy == OrderMode.Date)
            {
                articles = await query.OrderBy(x => x.CreatedDate)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<ArticleEntity, ArticleListDTO>(_mapper);
            }
            else
            {
                articles = await query.OrderBy(x => x.Raiting)
                    .Skip(start)
                    .Take(count)
                    .ToListAsync<ArticleEntity, ArticleListDTO>(_mapper);
            }


            return this.CollectionResult(articles);
        }

        [HttpGet("{articleId}")]
        public async Task<ActionResult<ArticleViewDTO>> Get(long articleId)
        {
            var article = await _context.Articles.FindAsync(articleId);

            if (article == null)
            {
                return NotFound();
            }

            return this.Ok(new ArticleViewDTO
            {
                Id = article.Id,
                Title = article.Title,
                Text = article.Text,
                Files = article.Files.Select(fileInfo => fileInfo.ToUrl()),
                Tags = article.Tags.Select(x => x.Name),
                CategoryId = article.CategoryId,
                Raiting = article.Raiting,
                CreatedDate = article.CreatedDate,
                ModifiedDate = article.ModifiedDate
            });
        }

        [HttpPost("Add")]
        public async Task<ActionResult<IdDTO>> Add(AddArticleDTO vm)
        {
            if (vm.Files != null && !_context.PersistentStorageFileInfos.All(x => vm.Files.Contains(x.Id)))
            {
                return this.NotFound();
            }

            var entity = await this.ToEntityAsync(vm);

            _context.Articles.Add(entity);
            await _context.SaveChangesAsync();

            //await _elasticClient.IndexDocumentAsync(vm);

            return this.IdResult(entity.Id);
        }

        [HttpPost("{articleId}/Edit")]
        public async Task<ActionResult> Edit(long articleId, EditArticleDTO vm)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null)
            {
                return this.NotFound();
            }

            article = _mapper.Map(vm, article);

            _context.Articles.Update(article);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("{articleId}/Delete")]
        public async Task<ActionResult> Delete(long articleId)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null)
            {
                return this.NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("{articleId}/AddTag")]
        public async Task<ActionResult> AddTag(long articleId, string tag)
        {
            var article = _context.Articles.Include(x => x.Tags).FirstOrDefault(x => x.Id == articleId);
            if (article == null)
            {
                return this.NotFound();
            }

            if (!article.Tags.Any(x => x.Name == tag))
            {
                var tagEntity = await _context.ArticleTags.FirstOrDefaultAsync(x => x.Name == tag);
                if (tagEntity == null)
                {
                    tagEntity = new ArticleTagEntity { Name = tag };
                    _context.ArticleTags.Add(tagEntity);
                }

                article.Tags.Add(tagEntity);
            }

            _context.Articles.Update(article);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("{articleId}/RemoveTag")]
        public async Task<ActionResult> RemoveTag(long articleId, string tag)
        {
            var article = await this._context.Articles.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == articleId);
            if (article == null)
            {
                return this.NotFound();
            }

            var tagEntity = article.Tags.FirstOrDefault(x => x.Name == tag);
            if (tagEntity == null)
            {
                return this.NotFound();
            }

            _context.ArticleTags.Remove(tagEntity);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("{articleId}/Vote")]
        public async Task<ActionResult> Vote(long articleId)
        {
            var article = await this._context.Articles.FirstOrDefaultAsync(x => x.Id == articleId);
            if (article == null)
            {
                return this.NotFound();
            }

            article.Raiting++;

            _context.Articles.Update(article);
            await _context.SaveChangesAsync();

            return this.Ok();
        }

        private async Task<ArticleEntity> ToEntityAsync(AddArticleDTO vm)
        {
            var existsTags = await _context.ArticleTags
                .Where(x => vm.Tags.Any(y => x.Name == y))
                .ToListAsync();

            var newTags = vm.Tags
                .Except(existsTags.Select(x => x.Name))
                .Select(x => new ArticleTagEntity { Name = x })
                .ToList();

            existsTags.AddRange(newTags);

            return new ArticleEntity
            {
                Title = vm.Title,
                PreviewText = vm.PreviewText,
                Text = vm.Text,
                Tags = existsTags,
                Files = vm.Files?.Select(x => new PersistentStorageFileInfoEntity { Id = x }).ToList(),
                CategoryId = vm.CategoryId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
