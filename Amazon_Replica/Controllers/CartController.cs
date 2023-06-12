using Amazon_Replica.Services;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_Replica.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepo productRepo;
        private readonly ICartRepo cartRepo;

        public CartController(IProductRepo _productRepo, ICartRepo _cartRepo)
        {
            productRepo = _productRepo;
            cartRepo = _cartRepo;
        }

        public IActionResult Index()
        {
            var CartItems = cartRepo.GetCartItems();
            return View(CartItems);
        }

        public IActionResult AddToCart(int id)
        {
            cartRepo.AddToCart(id);

            return RedirectToAction("Index", "Store");
        }

        public IActionResult RemoveFromCart(string id)
        {
            var selectedProduct = cartRepo.GetCartItem(id);

            if (selectedProduct != null)
            {
                cartRepo.RemoveFromCart(id);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ReduceQuantity(string id)
        {
            var selectedProduct = cartRepo.GetCartItem(id);

            if (selectedProduct != null)
            {
                cartRepo.ReduceQuantity(id);
            }

            return RedirectToAction("Index");
        }

        public IActionResult IncreaseQuantity(string id)
        {
            var selectedProduct = cartRepo.GetCartItem(id);

            if (selectedProduct != null)
            {
                cartRepo.IncreaseQuantity(id);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            cartRepo.ClearCart();

            return RedirectToAction("Index");
        }
    }
}
