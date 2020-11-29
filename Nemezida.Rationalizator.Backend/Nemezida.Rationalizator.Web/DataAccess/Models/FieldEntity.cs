namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    /// <summary>
    /// Поля рационализаторской заявки
    /// </summary>
    public class FieldEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Название поля
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Подсказка к полю
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// Тип поля
        /// </summary>
        public FieldTypeEntity FieldType { get; set; }

        /// <summary>
        /// Идентификатор типа поля
        /// </summary>
        public long FieldTypeId { get; set; }

        /// <summary>
        /// Является ли поле полем только для чтения
        /// </summary>
        public bool IsReadOnly { get; set; }

    }
}
