using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContextSqlServer: DataContext
    {
        public DataContextSqlServer(DbContextOptions<DataContextSqlServer> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}