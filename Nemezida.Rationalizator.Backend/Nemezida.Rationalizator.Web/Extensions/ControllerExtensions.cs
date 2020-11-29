namespace Nemezida.Rationalizator.Web.Extensions
{
    using Microsoft.AspNetCore.Mvc;

    using Nemezida.Rationalizator.Web.ViewModels;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ControllerExtensions
    {
        public static ActionResult<CollectionDTO<TItems>> CollectionResult<TItems>(this ControllerBase controller, IEnumerable<TItems> items, int count)
        {
            return controller.Ok(new CollectionDTO<TItems>(items, count));
        }

        public static ActionResult<CollectionDTO<TItems>> CollectionResult<TItems>(this ControllerBase controller, ICollection<TItems> items)
        {
            return controller.Ok(new CollectionDTO<TItems>(items, items.Count));
        }

        public static ActionResult<IdDTO> IdResult(this ControllerBase controller, long id)
        {
            return controller.Ok(new IdDTO { Id = id });
        }
    }
}
