using Amazon_Replica.Data;
using Amazon_Replica.Models;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Amazon_Replica.Areas.Identity.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Amazon_Replica.Services
{
    public class CartRepo : ICartRepo 
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor HttpContextAccessor;


        public CartRepo(ApplicationDbContext _context, IHttpContextAccessor _HttpContextAccessor)
        {
            context = _context;
            HttpContextAccessor = _HttpContextAccessor;
        }

        public string GetCartId(Cart cart)
        {
            ISession session = HttpContextAccessor.HttpContext.Session;
            if (session.GetString(cart.CartSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContextAccessor.HttpContext.User.Identity.Name))
                {
                    session.SetString(cart.CartSessionKey, HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                }
                else
                {
                    // Generate a new random GUID using System.Guid class.     
                    Guid tempCartId = Guid.NewGuid();
                    session.SetString(cart.CartSessionKey, tempCartId.ToString());
                }
            }
            return session.GetString(cart.CartSessionKey);
        }

        //public string SetCartIdToUserId(Cart cart)
        //{
        //    ISession session = HttpContextAccessor.HttpContext.Session;

        //    if (!string.IsNullOrWhiteSpace(HttpContextAccessor.HttpContext.User.Identity.Name))
        //    {
        //        session.SetString(cart.CartSessionKey, HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        //    }

            

        //    return session.GetString(cart.CartSessionKey);
        //}

        public void AddToCart(int id)
        {
            // Retrieve the product from the database.           
            string ShoppingCartId = GetCartId(new Cart());

            var cartItem = context.CartItems.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == id);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists.                 
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    ProductId = id,
                    CartId = ShoppingCartId,
                    Product = context.Products.SingleOrDefault(
                   p => p.Id == id),
                    Quantity = 1
                };

                context.CartItems.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart,                  
                // then add one to the quantity.                 
                cartItem.Quantity++;
            }
            context.SaveChanges();
        }

        public List<CartItem> GetCartItems()
        {
            string ShoppingCartId = GetCartId(new Cart());

            return context.CartItems.Where(c => c.CartId == ShoppingCartId).Include(c => c.Product).ToList();
        }

        public void ClearCart()
        {
            string ShoppingCartId = GetCartId(new Cart());

            var cartItems = context.CartItems.Where(c => c.CartId == ShoppingCartId);
            context.CartItems.RemoveRange(cartItems);
            context.SaveChanges();
        }

        public void ReduceQuantity(string id)
        {
            string ShoppingCartId = GetCartId(new Cart());
            var cartItem = context.CartItems.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ItemId == id); 

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else
                {
                    context.CartItems.Remove(cartItem);
                }
            }
            context.SaveChanges();
        }

        public void RemoveFromCart(string id)
        {
            string ShoppingCartId = GetCartId(new Cart());
            var cartItem = context.CartItems.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ItemId == id);

            if (cartItem != null)
            {
                context.CartItems.Remove(cartItem);
            }
            context.SaveChanges();
        }

        public void IncreaseQuantity(string id)
        {
            string ShoppingCartId = GetCartId(new Cart());
            var cartItem = context.CartItems.Where(c => c.CartId == ShoppingCartId && c.ItemId == id).Include(c => c.Product).FirstOrDefault();

            if (cartItem != null)
            {
                if (cartItem.Quantity > 0 && cartItem.Quantity < cartItem.Product.NumInStock)
                {
                    cartItem.Quantity++;
                }
            }

            context.SaveChanges();
        }

        public decimal GetCartTotal()
        {
            string ShoppingCartId = GetCartId(new Cart());
            return context.CartItems
                .Where(c => c.CartId == ShoppingCartId)
                .Select(c => c.Product.Price * c.Quantity)
                .Sum();
        }

        public CartItem GetCartItem(string id)
        {
            return context.CartItems.SingleOrDefault(c => c.ItemId == id);
        }
    }
}
