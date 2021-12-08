using Microsoft.EntityFrameworkCore;
namespace Model
{
    public class ReviewRepository
    {
        private ServiceContext context;
        public ReviewRepository(ServiceContext context)
        {
            this.context = context;
        }
    }
}