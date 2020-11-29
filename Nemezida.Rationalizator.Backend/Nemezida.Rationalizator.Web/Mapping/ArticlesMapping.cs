namespace Nemezida.Rationalizator.Web.Mapping
{
    using AutoMapper;

    using Nemezida.Rationalizator.Web.DataAccess.Models;
    using Nemezida.Rationalizator.Web.ViewModels;
    using Nemezida.Rationalizator.Web.ViewModels.Article;

    using System.Linq;

    public class ArticlesMapping : Profile
    {
        public ArticlesMapping()
        {
            this.CreateMap<ArticleEntity, ArticleViewDTO>()
                .ForMember(x => x.Tags, x => x.MapFrom(f => f.Tags.Select(tag => tag.Name)))
                .ForMember(x => x.Files, x => x.MapFrom(f => f.Files.Select(x => $"id{x.Id}")));

            this.CreateMap<AddArticleDTO, ArticleEntity>();
        }
    }
}
