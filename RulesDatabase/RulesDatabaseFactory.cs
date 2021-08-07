using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RulesDatabase
{
    public class RulesDatabaseFactory : IDesignTimeDbContextFactory<RulesDbContext>
    {
        public RulesDbContext CreateDbContext(string[] args)
        {
            return new RulesDbContext( new DbContextOptionsBuilder<RulesDbContext>().UseSqlServer("Server=127.0.0.1;Database=rulesdb;User Id=sa;Password=l0cal!PW;").Options );
        }
    }
}
