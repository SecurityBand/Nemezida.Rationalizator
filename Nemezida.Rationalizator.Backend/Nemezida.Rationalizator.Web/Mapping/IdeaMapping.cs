namespace Nemezida.Rationalizator.Web.Mapping
{
    using AutoMapper;

    using Nemezida.Rationalizator.Web.DataAccess;
    using Nemezida.Rationalizator.Web.DataAccess.Models;
    using Nemezida.Rationalizator.Web.ViewModels;

    using System.Linq;

    public class IdeaMapping : Profile
    {
        public IdeaMapping()
        {
            this.CreateMap<IdeaEntity, IdeaViewDTO>()
                .ForMember(x => x.Tags, x => x.MapFrom(f => f.Tags.Select(tag => tag.Name)))
                .ForMember(x => x.Files, x=> x.MapFrom(f => f.Files.Select(x => $"id{x.Id}")));

            this.CreateMap<AddIdeaDTO, IdeaEntity>();
        }
    }
}
