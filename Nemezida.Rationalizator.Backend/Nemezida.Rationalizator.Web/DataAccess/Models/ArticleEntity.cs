namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Статьи документации
    /// </summary>
    public class ArticleEntity : IHaveId
    {
        public long Id { get; set; }

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
        public virtual ICollection<PersistentStorageFileInfoEntity> Files { get; set; }

        /// <summary>
        /// теги статьи
        /// </summary>
        public virtual ICollection<ArticleTagEntity> Tags { get; set; }

        /// <summary>
        /// Категория статьи
        /// </summary>
        public virtual CategoryEntity Category { get; set; }

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

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        /// <remarks>
        /// Сортировка статей первично выполняется по этому полю, а после уже по рейтингу/дате (в зависимости от параметров)
        /// </remarks>
        public int Order { get; set; }

    }
}
