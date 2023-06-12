using Amazon_Replica.Models;

namespace Amazon_Replica.Services
{
    public interface IProductRepo
    {
        public List<Product> GetAll();
        public Product GetDetails(int? id);
        public void Insert(Product product);
        public void UpdateProduct(int id, Product product);
        public void DeleteProduct(int id);
        public List<Product> GetProductByCategoryId(int? categoryId);

        public ICollection<Product> SearchByName(string productName);
        public ICollection<Product> SearchByMinPrice(string minPrice);
        public ICollection<Product> SearchByMaxPrice(string maxPrice);
        public ICollection<Product> SearchByPriceRange(string minPrice, string maxPrice);
    }
}
