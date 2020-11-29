namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Рационализаторские предложения
    /// </summary>
    public class RationalOfferEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        public int Raiting { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public StatusEntity Status { get; set; }

        /// <summary>
        /// Идентификатор статуса
        /// </summary>
        public long StatusId { get; set; }

        /// <summary>
        /// Авторы
        /// </summary>
        public virtual ICollection<UserEntity> Autors { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<RationalOfferTagEntity> Tags { get; set; }

        /// <summary>
        /// История изменения статусов рационализаторского проедложения
        /// </summary>
        public virtual ICollection<StatusHistoryEntity> StatusHistory { get; set; }

        /// <summary>
        /// Выбранные пользователем значения дополнительных полей
        /// </summary>
        public virtual ICollection<FieldValueEntity> FieldValues { get; set; }

        /// <summary>
        /// Файлы, прикрепленые к заявке
        /// </summary>
        public virtual ICollection<PersistentStorageFileInfoEntity> Files { get; set; }

    }
}
