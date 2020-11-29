namespace Nemezida.Rationalizator.Web.ViewModels
{
    using Nemezida.Rationalizator.Web.DataAccess;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserViewModel : IHaveId
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
