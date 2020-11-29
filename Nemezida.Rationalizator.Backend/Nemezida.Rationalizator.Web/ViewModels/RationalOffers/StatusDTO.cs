namespace Nemezida.Rationalizator.Web.ViewModels
{
    using Nemezida.Rationalizator.Web.DataAccess;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class StatusDTO : IHaveId
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
