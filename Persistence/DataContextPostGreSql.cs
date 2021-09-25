using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContextPostGreSql : DataContext 
    {
        public DataContextPostGreSql(DbContextOptions<DataContextPostGreSql> options) 
            : base(options) 
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}