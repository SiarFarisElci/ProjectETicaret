using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Models.ShoppingTools
{
    public class Cart
    {

        Dictionary<int, CartItem> _sepetim;

        public Cart()
        {
            _sepetim = new Dictionary<int, CartItem>();
        }

        public List<CartItem> Sepetim
        {
            get
            {
                return _sepetim.Values.ToList();
            }

        }


        public void SepeteEkle(CartItem cartItem)
        {
            if (_sepetim.ContainsKey(cartItem.ID))
            {
                _sepetim[cartItem.ID].Amount++;
                return;
            }

            _sepetim.Add(cartItem.ID , cartItem);
        }

        public void SepetenCikar(int id)
        {
            if (_sepetim[id].Amount > 1)
            {
                _sepetim[id].Amount--;
                return;
            }

            _sepetim.Remove(id);
        }

        public decimal TotalPrice { get
            {
                return _sepetim.Sum(x => x.Value.SubTotal);
            }
                }

    }
}