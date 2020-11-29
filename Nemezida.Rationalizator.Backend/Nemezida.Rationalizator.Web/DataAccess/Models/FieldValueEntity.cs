namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    /// <summary>
    /// Значения полей
    /// </summary>
    public class FieldValueEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Поле, для которого предназначено это значение
        /// </summary>
        public FieldEntity Field { get; set; }

        /// <summary>
        /// Идентификатор поля, для которого предназначено это значение
        /// </summary>
        public long FieldId { get; set; }

        /// <summary>
        /// Значение поля
        /// </summary>
        public string Value { get; set; }

    }
}
