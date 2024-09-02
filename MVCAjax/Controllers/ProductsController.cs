using Microsoft.AspNetCore.Mvc;
using MVCAjax.Models;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet]
        public IActionResult GetProducts(string filter)
        {
            IQueryable<Products> query = _context.Productos.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(p => p.Name.Contains(filter));
            }

            var products = query.ToList();
            return Json(products);
        }

        // Acción para abrir el modal de editar Producto
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = _context.Productos.Find(id);
            if (product == null || !product.IsActive) // Verificar que el producto existe y está activo
            {
                return NotFound();
            }

            // Pasar datos del producto a la vista parcial usando ViewBag
            ViewBag.ProductId = product.ProductId;
            ViewBag.Name = product.Name;
            ViewBag.Price = product.Price;
            ViewBag.FechaVencimiento = product.FechaVencimiento;

            return PartialView("_UpdateProductPartial");
        }

        // Acción para actualizar el producto
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int productId, string name, int price, string fechaVencimiento)
        {
            var product = await _context.Productos.FindAsync(productId);
            if (product == null || !product.IsActive)
            {
                return Json(new { message = "Producto no encontrado o inactivo." });
            }

            // Actualizar propiedades del producto
            product.Name = name;
            product.Price = price;
            product.FechaVencimiento = fechaVencimiento;

            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();

            return Json(new { message = "Producto actualizado con éxito." });
        }

        // Acción para eliminar un producto (eliminación lógica)
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _context.Productos.Find(id);
            if (product == null || !product.IsActive)
            {
                return Json(new { message = "Producto no encontrado o ya ha sido eliminado." });
            }

            // Marcar el producto como inactivo en lugar de eliminarlo físicamente
            product.IsActive = false;
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();

            return Json(new { message = "Producto eliminado con éxito." });
        }
    }
}
