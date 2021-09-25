using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContextMySql : DataContext
    {
        public DataContextMySql(DbContextOptions<DataContextMySql> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}