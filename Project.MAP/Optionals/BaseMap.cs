using Project.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Optionals
{
    public class BaseMap<T> : EntityTypeConfiguration<T>  where T : BaseEntity , new()
    {
        public BaseMap()
        {

        }
    }
}
