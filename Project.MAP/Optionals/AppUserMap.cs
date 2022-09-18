using Project.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Optionals
{
    public class AppUserMap : BaseMap<AppUser>
    {
        public AppUserMap()
        {
            HasOptional(x => x.Profile).WithRequired(x=> x.AppUser);
        }
    }
}
