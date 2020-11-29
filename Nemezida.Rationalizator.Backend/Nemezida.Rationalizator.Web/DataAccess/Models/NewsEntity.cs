namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    using System;

    /// <summary>
    /// Новости на главной
    /// </summary>
    public class NewsEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Заголовок новости
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст новости
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата публикации
        /// </summary>
        public DateTime PublicationDate { get; set; }
    }
}
