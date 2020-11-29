namespace Nemezida.Rationalizator.Web.ViewModels
{
    using System.Collections.Generic;

    public class CollectionDTO<TItem>
    {
        public CollectionDTO(IEnumerable<TItem> items, int count)
        {
            this.Items = items;
            this.Count = count;
        }

        public IEnumerable<TItem> Items { get; set; }

        public int Count { get; set; }
    }
}
