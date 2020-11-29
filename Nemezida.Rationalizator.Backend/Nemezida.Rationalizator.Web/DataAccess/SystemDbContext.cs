
namespace Nemezida.Rationalizator.Web.DataAccess
{    
    using Microsoft.EntityFrameworkCore;
    using Nemezida.Rationalizator.Web.DataAccess.Models;

    public class SystemDbContext : DbContext
    {
        public SystemDbContext (DbContextOptions<SystemDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Статьи документации
        /// </summary>
        public DbSet<ArticleEntity> Articles { get; set; }

        /// <summary>
        /// Теги статей
        /// </summary>
        public DbSet<ArticleTagEntity> ArticleTags { get; set; }

        /// <summary>
        /// Категории рацпредложений/статей
        /// </summary>
        public DbSet<CategoryEntity> Categories { get; set; }

        /// <summary>
        /// Комментарии
        /// </summary>
        public DbSet<CommentaryEntity> Commentaries { get; set; }

        /// <summary>
        /// Поля рационализаторской заявки
        /// </summary>
        public DbSet<FieldEntity> Fields { get; set; }

        /// <summary>
        /// Типы полей
        /// </summary>
        public DbSet<FieldTypeEntity> FieldTypes { get; set; }

        /// <summary>
        /// Значения полей
        /// </summary>
        public DbSet<FieldValueEntity> FieldValues { get; set; }

        public DbSet<IdeaEntity> Ideas { get; set; }

        /// <summary>
        /// Теги статей
        /// </summary>
        public DbSet<IdeaTagEntity> IdeaTags { get; set; }

        /// <summary>
        /// Новости на главной
        /// </summary>
        public DbSet<NewsEntity> News { get; set; }

        /// <summary>
        /// Информация о хранимых файлах
        /// </summary>
        public DbSet<PersistentStorageFileInfoEntity> PersistentStorageFileInfos { get; set; }

        /// <summary>
        /// Рационализаторские предложения
        /// </summary>
        public DbSet<RationalOfferEntity> RationalOffers { get; set; }

        /// <summary>
        /// Теги статей
        /// </summary>
        public DbSet<RationalOfferTagEntity> RationalOfferTags { get; set; }

        /// <summary>
        /// Статусы рационализаторских предложений
        /// </summary>
        public DbSet<StatusEntity> Statuses { get; set; }

        /// <summary>
        /// История изменения статусов
        /// </summary>
        public DbSet<StatusHistoryEntity> StatusHistories { get; set; }

        /// <summary>
        /// Переходы статусов
        /// </summary>
        public DbSet<StatusTransitionEntity> StatusTransitions { get; set; }

        /// <summary>
        /// Пользователи
        /// </summary>
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// Звания пользователей
        /// </summary>
        public DbSet<UserRankEntity> UserRanks { get; set; }

    }
}