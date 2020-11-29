namespace Nemezida.Rationalizator.Web.Mapping
{
    using AutoMapper;

    using Nemezida.Rationalizator.Web.DataAccess.Models;
    using Nemezida.Rationalizator.Web.ViewModels;

    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            this.CreateMap<CategoryEntity, CategoryViewDTO>();
        }
    }
}
