using Amazon_Replica.Data;
using Amazon_Replica.Models;
using Microsoft.EntityFrameworkCore;

namespace Amazon_Replica.Services
{
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext context;

        public ProductRepo(ApplicationDbContext _context)
        {
            context = _context;
        }

        public List<Product> GetAll()
        {
            return context.Products.Include(p => p.Category).ToList();
        }

        public Product GetDetails(int? id)
        {
            return context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
        }

        public List<Product> GetProductByCategoryId(int? categoryId)
        {
            return context.Products.Where(p => p.CategoryId == categoryId).ToList();
        }

        public void Insert(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
        }

        public void UpdateProduct(int id, Product product)
        {
            Product OldPrd = context.Products.Find(id);
            OldPrd.Name = product.Name;
            OldPrd.Description = product.Description;
            OldPrd.Price = product.Price;
            OldPrd.NumInStock = product.NumInStock;
            if(product.Image != null)
                OldPrd.Image = product.Image;
            OldPrd.CategoryId = product.CategoryId;

            context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            context.Remove(context.Products.Find(id));
            context.SaveChanges();
        }

        public ICollection<Product> SearchByName(string name)
        {
            return context.Products.Include("Category").Where(p => p.Name.Contains(name) || p.Description.Contains(name) || p.Category.Name.Contains(name)).ToList();  
        }

        public ICollection<Product> SearchByMinPrice(string minPrice)
        {
            var min = decimal.Parse(minPrice);
            return context.Products.Include("Category").Where(p => p.Price >= min).ToList();
            
        }

        public ICollection<Product> SearchByMaxPrice(string maxPrice)
        {
            var max = decimal.Parse(maxPrice);
            return context.Products.Include("Category").Where(p => p.Price <= max).ToList();
        }

        public ICollection<Product> SearchByPriceRange(string minPrice, string maxPrice)
        {
            decimal max;
            decimal min;
            if (maxPrice != null)
                max = decimal.Parse(maxPrice);
            else
                max = 100000000;

            if (minPrice != null)
                min = decimal.Parse(minPrice);
            else
                min = 0;

            return context.Products.Include("Category").Where(p => p.Price >= min && p.Price <= max).ToList();
        }
    }
}
