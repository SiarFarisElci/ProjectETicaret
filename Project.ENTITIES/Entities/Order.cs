using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Entities
{
    public  class Order : BaseEntity
    {
        public string ShippedAddress { get; set; }
        public decimal TotalPrice { get; set; }

        //Sipariş işlemlerinin icerisindeki bilgileri daha rahat yakalamak adına actıgımız property'lerdern bir tanesi de TotalPrice'dir... Yalnız burada cok dikkatli olmamız gerekir gerçekten size hız kazandıracak bir durum varsa bu bilgileri ek olarak buraya almamız gerekir...Aynı zamanda bu durum abartılmamalıdır... Yani ilgili yapının tüm verileri asla bu sınıfa komple koyulmamalıdır...Sadece spesifik parcalar alınmalıdır

        public string UserName { get; set; }
        public string Email { get; set; }

        public int? AppUserID { get; set; }

        //Relational Properties

        public virtual AppUser  AppUser { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }

    }
}
