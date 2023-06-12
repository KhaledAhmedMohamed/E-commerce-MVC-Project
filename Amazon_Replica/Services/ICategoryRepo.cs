using Amazon_Replica.Models;

namespace Amazon_Replica.Services
{
    public interface ICategoryRepo
    {
        public List<Category> GetAll();
        public Category GetDetails(int? id);
        public void Insert(Category category);
        public void UpdateCategory(int id, Category category);
        public void DeleteCategory(int id);
    }
}
