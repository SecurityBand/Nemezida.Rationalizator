namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    /// <summary>
    /// Статусы рационализаторских предложений
    /// </summary>
    public class StatusEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Текст статуса
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Цвет текста статуса
        /// </summary>
        public string FontColor { get; set; }

        /// <summary>
        /// Фон статуса
        /// </summary>
        public string BackgroundColor { get; set; }
    }
}
