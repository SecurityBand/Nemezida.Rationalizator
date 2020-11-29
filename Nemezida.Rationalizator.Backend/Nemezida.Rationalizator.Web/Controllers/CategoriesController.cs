namespace Nemezida.Rationalizator.Web.Controllers
{
    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Nemezida.Rationalizator.Web.DataAccess;
    using Nemezida.Rationalizator.Web.DataAccess.Models;
    using Nemezida.Rationalizator.Web.Extensions;
    using Nemezida.Rationalizator.Web.ViewModels;

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Контроллер категорий статей
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly SystemDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(SystemDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CollectionDTO<CategoryViewDTO>>> ListRoot()
        {
            var categories = await _context.Categories
                .Where(x => x.ParentId == null)
                .ToListAsync<CategoryEntity, CategoryViewDTO>(_mapper);

            return this.CollectionResult(categories);
        }

        [HttpGet("{parentId}")]
        public async Task<ActionResult<CollectionDTO<CategoryViewDTO>>> ListChildrens(long parentId)
        {
            var categories = await _context.Categories
                .Where(x => x.ParentId == null)
                .ToListAsync<CategoryEntity, CategoryViewDTO>(_mapper);

            return this.CollectionResult(categories);
        }

        [HttpPost("{parentId}/Add")]
        public async Task<ActionResult> Add(long parentId, NewCategoryDTO vm)
        {
            var parent = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == parentId);

            if (parent == null)
            {
                return this.NotFound();
            }

            var entity = new CategoryEntity
            { 
                Name = vm.Name, 
                Description = vm.Description, 
                Parent = parent 
            };

            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return this.Ok();
        }
    }
}
