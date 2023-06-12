using Amazon_Replica.Areas.Identity.Data;
using Amazon_Replica.Models;
using Amazon_Replica.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Amazon_Replica.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        UserManager<ApplicationUser> userManager;
        private readonly IOrderRepo orderRepo;
        private readonly ICartRepo cartRepo;

        public OrderController(IOrderRepo _orderRepo, ICartRepo cart, UserManager<ApplicationUser> _UserManager)
        {
            orderRepo = _orderRepo;
            cartRepo = cart;
            userManager = _UserManager;
        }

        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Index()
        {
            return View(orderRepo.GetAll());
        }

        public IActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(Order order)
        {

            var cartItems = cartRepo.GetCartItems();
      
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var UserId = user.Id.ToString();

            if (cartItems.Count == 0)
            {
                ModelState.AddModelError("", "Empty Cart, please Add Product First");
            }

            if (ModelState.IsValid)
            {
                orderRepo.CreateOrder(order);
                cartRepo.ClearCart();

                return View("CheckOutComplete", order);
            }

            return View(order);
        }

        public IActionResult CheckOutComplete(Order order)
        {
            return View(order);
        }


        public IActionResult Payment(string id, string StripeEmail, string StripeToken)
        {
            var total = (long)decimal.Parse(id);
            var customers = new CustomerService();
            var charges = new ChargeService();
            var cutomer = customers.Create(new CustomerCreateOptions { Email = StripeEmail, Source = StripeToken });
            var charge = charges.Create(new ChargeCreateOptions
            {
                Customer = cutomer.Id,
                Amount = total*100,
                Currency = "usd"
            });
            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                return RedirectToAction("Index", "Store");
            }
            else
            {
                return View();
            }

            return View("Store", "Index");
        }
    }
}
