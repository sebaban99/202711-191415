using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using IMMRequest.DataAccess;

namespace IMMRequest.DataAccess
{
    public enum ContextType {
        MEMORY, SQL
    }

    public class ContextFactory: IDesignTimeDbContextFactory<IMMRequestContext>
    {
        public IMMRequestContext CreateDbContext(string[] args) {
            return GetNewContext();
        }

        public static IMMRequestContext GetNewContext(ContextType type = ContextType.SQL) {
            var builder = new DbContextOptionsBuilder<IMMRequestContext>();
            DbContextOptions options = null;
            if (type == ContextType.MEMORY) {
                options = GetMemoryConfig(builder);
            } else {
                options = GetSqlConfig(builder);
            }
            return new IMMRequestContext(options);
        }

        private static DbContextOptions GetMemoryConfig(DbContextOptionsBuilder builder) {
            builder.UseInMemoryDatabase("IMMRequests");
            return builder.Options;
        }

        private static DbContextOptions GetSqlConfig(DbContextOptionsBuilder builder) {
            builder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=IMMRequests;Trusted_Connection=True;");
            return builder.Options;
        } 
    }
}