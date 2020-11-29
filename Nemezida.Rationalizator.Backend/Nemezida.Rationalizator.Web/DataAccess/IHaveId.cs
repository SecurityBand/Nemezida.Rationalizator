namespace Nemezida.Rationalizator.Web.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    interface IHaveId
    {
        long Id { get; set; }
    }
}
