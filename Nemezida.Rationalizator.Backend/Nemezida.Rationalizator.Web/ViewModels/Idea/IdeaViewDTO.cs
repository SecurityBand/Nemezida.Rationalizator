namespace Nemezida.Rationalizator.Web.ViewModels
{
    using Nemezida.Rationalizator.Web.DataAccess;

    using System;
    using System.Collections.Generic;

    public class IdeaViewDTO : IHaveId
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public int Raiting { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public virtual IEnumerable<string> Files { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
