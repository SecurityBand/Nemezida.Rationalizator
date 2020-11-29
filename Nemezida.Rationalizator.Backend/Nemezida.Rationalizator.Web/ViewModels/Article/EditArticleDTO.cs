namespace Nemezida.Rationalizator.Web.ViewModels.Article
{
    using System.Collections.Generic;

    public class EditArticleDTO
    {
        /// <summary>
        /// Заголовок статьи
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст превью статьи (отображается в списке статей)
        /// </summary>
        public string PreviewText { get; set; }

        /// <summary>
        /// Текст статьи
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Прикрепленые файлы
        /// </summary>
        public virtual IEnumerable<long> Files { get; set; }
    }
}
