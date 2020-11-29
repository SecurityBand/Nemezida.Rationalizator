namespace Nemezida.Rationalizator.Web.ViewModels
{
    using Nemezida.Rationalizator.Web.DataAccess;

    using System;
    using System.Collections.Generic;

    public class ArticleViewDTO : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Заголовок статьи
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст статьи
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Прикрепленые файлы
        /// </summary>
        public virtual IEnumerable<string> Files { get; set; }

        /// <summary>
        /// теги статьи
        /// </summary>
        public virtual IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// Идентификатор категории статьи
        /// </summary>
        public long? CategoryId { get; set; }

        /// <summary>
        /// Рейтинг статьи
        /// </summary>
        public int Raiting { get; set; }

        /// <summary>
        /// Дата создания статьи
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Дата изменения статьи
        /// </summary>
        public DateTime ModifiedDate { get; set; }

    }
}
