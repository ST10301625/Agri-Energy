using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnects.Data;
using AgriEnergyConnects.Models;
using AgriEnergyConnects.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace AgriEnergyConnects.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Products
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var products = await _context.Products
                    .Where(p => p.Farmer.UserId == userId)
                    .Include(p => p.Farmer)
                    .ToListAsync();

                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products for the user.");
                return View("Error");
            }
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var product = await _context.Products
                    .Include(p => p.Farmer)
                    .FirstOrDefaultAsync(m => m.Id == id && m.Farmer.UserId == userId);

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product details.");
                return View("Error");
            }
        }

        // GET: Products/Create
        [Authorize(Roles = "Farmer")]
        public IActionResult Create()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var farmer = _context.Farmers.FirstOrDefault(f => f.UserId == userId);

                if (farmer == null)
                {
                    _logger.LogWarning("Farmer not found for user {UserId}", userId);
                    return Unauthorized();
                }

                // Pass a new product with the correct FarmerId and default ProductionDate
                var product = new Product
                {
                    FarmerId = farmer.Id,
                    ProductionDate = DateTime.Now // Set default to today's date
                };

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing create view.");
                return View("Error");
            }
        }


        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create(Product product)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find farmer by user ID
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);
            if (farmer == null)
            {
                ModelState.AddModelError(string.Empty, "Farmer not found.");
                return View(product);
            }

            // Assign the FK only — not the navigation property
            product.FarmerId = farmer.Id;

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }



        // GET: Products/Edit/5
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var product = await _context.Products
                    .Include(p => p.Farmer)
                    .FirstOrDefaultAsync(m => m.Id == id && m.Farmer.UserId == userId);

                if (product == null)
                {
                    return NotFound();
                }

                ViewData["FarmerId"] = new SelectList(_context.Farmers.Where(f => f.UserId == userId), "Id", "Email", product.FarmerId);
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product for editing.");
                return View("Error");
            }
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,ProductionDate,FarmerId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var productToUpdate = await _context.Products
                        .Include(p => p.Farmer)
                        .FirstOrDefaultAsync(p => p.Id == id && p.Farmer.UserId == userId);

                    if (productToUpdate == null)
                    {
                        return Unauthorized();
                    }

                    productToUpdate.Name = product.Name;
                    productToUpdate.Category = product.Category;
                    productToUpdate.ProductionDate = product.ProductionDate;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                var userGuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewData["FarmerId"] = new SelectList(_context.Farmers.Where(f => f.UserId == userGuid), "Id", "Email", product.FarmerId);
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing product.");
                return View("Error");
            }
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var product = await _context.Products
                    .Include(p => p.Farmer)
                    .FirstOrDefaultAsync(m => m.Id == id && m.Farmer.UserId == userId);

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product for deletion.");
                return View("Error");
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var product = await _context.Products
                    .Include(p => p.Farmer)
                    .FirstOrDefaultAsync(p => p.Id == id && p.Farmer.UserId == userId);

                if (product == null)
                {
                    return Unauthorized();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product.");
                return View("Error");
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> EmployeeView(string? category, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Products.Include(p => p.Farmer).AsQueryable();

            var categories = await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            if (startDate.HasValue)
                query = query.Where(p => p.ProductionDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.ProductionDate <= endDate.Value);

            var viewModel = new ProductFilterViewModel
            {
                Category = category,
                StartDate = startDate,
                EndDate = endDate,
                Categories = categories,
                Products = await query.ToListAsync()
            };

            return View(viewModel);
        }
    }
}
