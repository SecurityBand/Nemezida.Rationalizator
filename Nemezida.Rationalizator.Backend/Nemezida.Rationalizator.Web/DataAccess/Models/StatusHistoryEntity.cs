namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    using System;

    /// <summary>
    /// История изменения статусов
    /// </summary>
    public class StatusHistoryEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Рационализаторское предложение, которому принадлежит эта запись истории
        /// </summary>
        public RationalOfferEntity RationalOffer { get; set; }

        /// <summary>
        /// Идентификатор рационализаторского предложения, которому принадлежит эта запись истории
        /// </summary>
        public long RationalOfferId { get; set; }

        /// <summary>
        /// Статус в который было переведено рационализаторское приложение приложение при создании этой записи
        /// </summary>
        public StatusEntity NewStatus { get; set; }

        /// <summary>
        /// Идентификатор статуса в который было переведено рационализаторское приложение приложение при создании этой записи
        /// </summary>
        public long NewStatusId { get; set; }

        /// <summary>
        /// Дата добаления этой записи
        /// </summary>
        public DateTime DateAdded { get; set; }

        // Ссылка на комментарий
    }
}
