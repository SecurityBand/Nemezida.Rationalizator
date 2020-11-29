namespace Nemezida.Rationalizator.Web.ViewModels
{
    using Nemezida.Rationalizator.Web.DataAccess;

    using System.Collections.Generic;

    /// <summary>
    /// Модель предпросмотра статьи
    /// </summary>
    public class ArticleListDTO : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Заголовок статьи
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст превью статьи (отображается в списке статей)
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Пути к прикрепленым файлам
        /// </summary>
        public virtual IEnumerable<string> Files { get; set; }

        /// <summary>
        /// Идентификатор категории статьи
        /// </summary>
        public long CategoryId { get; set; }

    }
}
