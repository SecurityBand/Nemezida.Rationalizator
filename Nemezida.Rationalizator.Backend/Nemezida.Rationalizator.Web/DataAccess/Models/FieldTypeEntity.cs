namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    /// <summary>
    /// Типы полей
    /// </summary>
    public class FieldTypeEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Название типа
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание типа
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Json схема типа
        /// </summary>
        public string JsonSchema { get; set; }

        /// <summary>
        /// Позволяет ли это поле добавлять новые варианты значений
        /// </summary>
        public bool IsEnum { get; set; }

        /// <summary>
        /// Позволяет ли это поле выбирать несколько вариантов значений
        /// </summary>
        public bool AloowMultiselect { get; set; }
    }
}
