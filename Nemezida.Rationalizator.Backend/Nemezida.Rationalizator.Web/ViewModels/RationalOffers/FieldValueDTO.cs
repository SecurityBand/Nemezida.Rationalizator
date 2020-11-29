namespace Nemezida.Rationalizator.Web.ViewModels
{
    public class FieldValueDTO
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
        /// Является ли поле полем только для чтения
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Значение поля
        /// </summary>
        public string Value { get; set; }
    }
}
