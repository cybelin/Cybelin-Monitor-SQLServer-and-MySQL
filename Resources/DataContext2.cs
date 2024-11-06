using Microsoft.EntityFrameworkCore;
using WpfRequestResponseLogger.Models;

public class DataContext2 : DbContext
{
    
    public DataContext2(string connectionString) : base(GetOptions(connectionString))
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<LogData>().HasNoKey();
        base.OnModelCreating(modelBuilder);
    }


    
    private static DbContextOptions<DataContext2> GetOptions(string connectionString)
    {
        return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<DataContext2>(), connectionString).Options;
    }

    public DbSet<RequestLog> RequestLogs { get; set; }
    public DbSet<ResponseLog> ResponseLogs { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<BlacklistedIp> BlacklistedIps { get; set;}


}
