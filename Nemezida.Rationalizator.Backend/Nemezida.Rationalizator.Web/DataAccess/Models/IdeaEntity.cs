namespace Nemezida.Rationalizator.Web.DataAccess.Models
{
    using System;
    using System.Collections.Generic;

    public class IdeaEntity : IHaveId
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public int Raiting { get; set; }

        public virtual ICollection<IdeaTagEntity> Tags { get; set; }

        public virtual ICollection<PersistentStorageFileInfoEntity> Files { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
