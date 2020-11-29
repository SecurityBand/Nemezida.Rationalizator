namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Теги статей
    /// </summary>
    public class RationalOfferTagEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Наименование тега
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Все рацпредложения, связанные с этим тегом
        /// </summary>
        public virtual ICollection<RationalOfferEntity> RationalOffers { get; set; }
    }
}
