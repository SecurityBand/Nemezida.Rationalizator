namespace Nemezida.Rationalizator.Web.ViewModels
{
    using Nemezida.Rationalizator.Web.DataAccess;

    using System;
    using System.Collections.Generic;

    public class RationalOfferDTO : IHaveId
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
        public StatusDTO Status { get; set; }

        /// <summary>
        /// Авторы
        /// </summary>
        public virtual IEnumerable<UserViewModel> Autors { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// История изменения статусов рационализаторского проедложения
        /// </summary>
        //public ICollection<StatusHistoryEntity> StatusHistory { get; set; }

        /// <summary>
        /// Выбранные пользователем значения дополнительных полей
        /// </summary>
        public IEnumerable<FieldValueDTO> FieldValues { get; set; }

        /// <summary>
        /// Файлы, прикрепленые к заявке
        /// </summary>
        public IEnumerable<string> Files { get; set; }
    }
}
