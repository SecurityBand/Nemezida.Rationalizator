namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    /// <summary>
    /// Звания пользователей
    /// </summary>
    public class UserRankEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Название звания
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание звания
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Количество принятых заявок, необходмых для получения этого звания
        /// </summary>
        public int ApproveCount { get; set; }
    }
}
