using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Stripe.Checkout;

namespace WebApplication1.Controllers
{
    public class CheckOutController : Controller
    {
        public IActionResult Index()
        {
            List<ProductEntity> productList = new List<ProductEntity>();
            productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Product="Аквамен",
                    Rate=250,
                    Quanity=2,
                    ImagePath="img/Image1.png"
                },
                new ProductEntity
                {
                    Product="Энканто",
                    Rate=100,
                    Quanity=1,
                    ImagePath="img/Image2.png"
                },
                new ProductEntity
                {
                    Product="Курманжан Датка",
                    Rate=360,
                    Quanity=3,
                    ImagePath="img/Image3.png"
                }
            };
            return View(productList);
        }

        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());

            if (session.PaymentStatus == "paid")
            {
                var transaction = session.PaymentIntentId.ToString();
                return View("Success");
            }
            return View("Login");
        }

        public IActionResult Success()
        {
            return View();  
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult CheckOut()
        {
            List<ProductEntity> productList = new List<ProductEntity>();
            productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Product="Аквамен",
                    Rate=12500,
                    Quanity=2,
                    ImagePath="img/Image1.png"
                },
                new ProductEntity
                {
                    Product="Энканто",
                    Rate=10000,
                    Quanity=1,
                    ImagePath="img/Image2.png"
                },
                new ProductEntity
                {
                    Product="Курманжан Датка",
                    Rate=12000,
                    Quanity=3,
                    ImagePath="img/Image3.png"
                }
            };

            var domain = "http://localhost:5177/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"CheckOut/OrderConfirmation",
                CancelUrl = domain + "CheckOut/Login",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = "digitaltechjoint@gmail.com",

            
            };
            foreach (var item in productList)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Rate * item.Quanity),
                        Currency = "kgs",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.ToString(),
                        }
                    },
                    Quantity = item.Quanity,
                };
                options.LineItems.Add(sessionListItem);
            }
            var service=new SessionService();
            Session session=service.Create(options);

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
         }
    }
}
