namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Теги статей
    /// </summary>
    public class ArticleTagEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Наименование тега
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Все идеи, связанные с этим тегом
        /// </summary>
        public virtual ICollection<ArticleEntity> Articles { get; set; }
    }
}
