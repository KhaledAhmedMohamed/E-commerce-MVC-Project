using Amazon_Replica.Models;

namespace Amazon_Replica.Services
{
    public interface IOrderRepo
    {
        public void CreateOrder(Order order);
        public ICollection<Order> GetAll();
    }
}
