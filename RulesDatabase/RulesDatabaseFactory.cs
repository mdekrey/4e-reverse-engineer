using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RulesDatabase
{
    public class RulesDatabaseFactory : IDesignTimeDbContextFactory<RulesDbContext>
    {
        public RulesDbContext CreateDbContext(string[] args)
        {
            return new RulesDbContext( new DbContextOptionsBuilder<RulesDbContext>().UseSqlite("Data Source=../4e.db;Cache=Shared").Options );
        }
    }
}
