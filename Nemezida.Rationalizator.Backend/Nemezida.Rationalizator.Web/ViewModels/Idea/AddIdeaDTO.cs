namespace Nemezida.Rationalizator.Web.ViewModels
{
    using System.Collections.Generic;

    public class AddIdeaDTO
    {
        public string Text { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public virtual IEnumerable<long> Files { get; set; }
    }
}
