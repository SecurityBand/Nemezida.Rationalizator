namespace Nemezida.Rationalizator.Web.ViewModels
{
    using System.Collections.Generic;

    public class AddRationalOfferDTO
    {
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Выбранные пользователем значения дополнительных полей
        /// </summary>
        public IEnumerable<AddFieldValueDTO> FieldValues { get; set; }

        /// <summary>
        /// Файлы, прикрепленые к заявке
        /// </summary>
        public IEnumerable<long> Files { get; set; }


        public IEnumerable<string> Tags { get; set; }
    }
}
