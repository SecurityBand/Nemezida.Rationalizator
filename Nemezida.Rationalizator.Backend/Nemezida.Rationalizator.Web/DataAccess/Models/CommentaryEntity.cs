namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Комментарии
    /// </summary>
    public class CommentaryEntity : IHaveId
    {
        public long Id { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Вложенные файлы
        /// </summary>
        public virtual ICollection<PersistentStorageFileInfoEntity> Files { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Автор комментария
        /// </summary>
        public virtual UserEntity Autor { get; set; }

        /// <summary>
        /// Идентификатор автора комментария
        /// </summary>
        public long AutorId { get; set; }

        /// <remarks>
        /// Комментарии не удаляются, а только помечаются удаленными
        /// </remarks>
        public bool Deleted { get; set; }

        /// <summary>
        /// Родительский комментарий
        /// </summary>
        public virtual CommentaryEntity Parent { get; set; }

        /// <summary>
        /// Идентификатор родительского комментария
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// Дочерние комментарии
        /// </summary>
        public virtual ICollection<CommentaryEntity> Childrens { get; set; }
    }
}
