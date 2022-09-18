using Project.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Optionals
{
    public class OrderDetailMap : BaseMap<OrderDetail>
    {
        public OrderDetailMap()
        {
            Ignore(x=> x.ID);

            HasKey(x=> new
            {
                x.OrderID,
                x.ProductID
            });
        }
    }
}
