namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    /// <summary>
    /// Пользователи
    /// </summary>
    public class UserEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Файл фотографии пользователя
        /// </summary>
        public PersistentStorageFileInfoEntity Photo { get; set; }

        /// <summary>
        /// Идентификатор фотографии
        /// </summary>
        public long PhotoId { get; set; }

        /// <summary>
        /// Звание пользователя
        /// </summary>
        public UserRankEntity Rank { get; set; }

        /// <summary>
        /// Идентификатор звания
        /// </summary>
        public long RankId { get; set; }

        /// <summary>
        /// Является ли пользователь заблокированным
        /// </summary>
        public bool IsBlocked { get; set; }
    }
}
