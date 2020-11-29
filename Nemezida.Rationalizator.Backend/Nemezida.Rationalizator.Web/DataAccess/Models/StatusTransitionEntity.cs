namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Переходы статусов
    /// </summary>
    public class StatusTransitionEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Статус, ИЗ которого возможен переход
        /// </summary>
        public virtual StatusEntity CurrentStatus { get; set; }

        /// <summary>
        /// Идентификатор статус, ИЗ которого возможен переход
        /// </summary>
        public long CurrentStatusId { get; set; }

        /// <summary>
        /// Статус, В который возможен переход
        /// </summary>
        public virtual StatusEntity NextStatus { get; set; }

        /// <summary>
        /// Идентификатор статуса, В который возможен переход
        /// </summary>
        public long NextStatusId { get; set; }

        /// <summary>
        /// Необходим ли комментарий для выполнения перехода?
        /// </summary>
        public bool CommentRequired { get; set; }

    }
}
