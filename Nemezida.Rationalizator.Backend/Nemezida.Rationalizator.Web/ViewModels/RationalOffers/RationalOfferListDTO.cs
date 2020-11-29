namespace Nemezida.Rationalizator.Web.ViewModels.RationalOffers
{
    using Nemezida.Rationalizator.Web.DataAccess;

    using System;
    using System.Collections.Generic;

    public class RationalOfferListDTO : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Превью текста
        /// </summary>
        public string PreviewText { get; set; }

        /// <summary>
        /// Текст идеи
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        public int Raiting { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public StatusDTO Status { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual IEnumerable<string> Tags { get; set; }
    }
}
