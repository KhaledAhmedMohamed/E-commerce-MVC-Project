using Amazon_Replica.Areas.Identity.Data;
using Amazon_Replica.Data;
using Amazon_Replica.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Amazon_Replica.Services
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly ICartRepo cartRepo;
        private readonly IProductRepo productRepo;


        public OrderRepo(ApplicationDbContext _context, IHttpContextAccessor _HttpContextAccessor, ICartRepo _cartRepo, IProductRepo _productRepo)
        {
            context = _context;
            HttpContextAccessor = _HttpContextAccessor;
            cartRepo = _cartRepo;
            productRepo = _productRepo;
        }

        public void CreateOrder(Order order)
        {

            Product product = new Product();
            string userId = HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            order.OrderDate = DateTime.Now;
            order.UserId = userId;
            var cartItems = cartRepo.GetCartItems();
          
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem()
                {
                    Quatity = item.Quantity,
                    ProductId = item.Product.Id,
                    Price = item.Product.Price * item.Quantity
                };
                order.OrderItems.Add(orderItem);
                order.OrderTotal += orderItem.Price;


                /**** Decrease Number in Stock Of this product aftre creating the order ****/
                item.Product.NumInStock -= item.Quantity;
                productRepo.UpdateProduct(item.Product.Id, item.Product);
                /**** Decrease Number in Stock Of this product aftre creating the order ****/

            }

            context.Orders.Add(order);
            context.SaveChanges();
            cartRepo.ClearCart();

        }

        public ICollection<Order> GetAll()
        {
            return context.Orders.Include(o => o.User).ToList();
        }
    }
}
