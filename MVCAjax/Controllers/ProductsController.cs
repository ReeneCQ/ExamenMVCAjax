using Microsoft.AspNetCore.Mvc;
using MVCAjax.Models;

namespace MVCAjax.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MarketContext _context;

        public ProductsController(MarketContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        // Acción para abrir el modal de crear Producto
        public IActionResult CreateProduct()
        {
            return PartialView("_CreateProductPartial");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm(string name, int price)
        {
            var product = new Products
            {
                Name = name,
                Price = price,
                FechaVencimiento = "00000000" // Puedes modificar esto según necesites
            };

            _context.Productos.Add(product);
            await _context.SaveChangesAsync();

            return Json(new { message = "Producto registrado con éxito." });
        }

        //[HttpGet] // Decorador para manejar solicitudes GET
        //public IActionResult GetProducts()
        //{
        //    var products = _context.Productos.ToList();
        //    return Json(products);
        //}

        [HttpGet] // Decorador para manejar solicitudes GET
        public IActionResult GetProducts(string filter)
        {
            IQueryable<Products> query = _context.Productos;

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(p => p.Name.Contains(filter));
            }

            var products = query.ToList();

            return Json(products);
        }
    }
}
