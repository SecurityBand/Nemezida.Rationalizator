namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Категории рацпредложений/статей
    /// </summary>
    public class CategoryEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Наименование категории
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание категории
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Родительская категория или null для корневой
        /// </summary>
        public virtual CategoryEntity Parent { get; set; }

        /// <summary>
        /// Идентификатор родительской категории или null для корневой
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// Дочерние категории
        /// </summary>
        public virtual ICollection<CategoryEntity> Childrens { get; set; }

    }
}
