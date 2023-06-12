using Amazon_Replica.Data;
using Amazon_Replica.Models;

namespace Amazon_Replica.Services
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly ApplicationDbContext context;

        public CategoryRepo(ApplicationDbContext _context)
        {
            context = _context;
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public Category GetDetails(int? id)
        {
            return context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public void Insert(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void UpdateCategory(int id, Category category)
        {
            Category OldCat = context.Categories.Find(id);
            OldCat.Name = category.Name;
            OldCat.Description = category.Description;
            if (category.Image != null)
                OldCat.Image = category.Image;

            context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            context.Remove(context.Categories.Find(id));
            context.SaveChanges();
        }
    }
}
