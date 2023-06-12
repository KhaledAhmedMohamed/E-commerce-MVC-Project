using Amazon_Replica.Models;

namespace Amazon_Replica.Services
{
    public interface ICartRepo
    {
        public string GetCartId(Cart cart);
        public void AddToCart(int id);
        public List<CartItem> GetCartItems();
        public void ClearCart();
        public void ReduceQuantity(string id);
        public void RemoveFromCart(string id);
        public void IncreaseQuantity(string id);
        public decimal GetCartTotal();
        public CartItem GetCartItem(string id);

    }
}
