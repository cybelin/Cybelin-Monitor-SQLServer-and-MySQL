using Microsoft.EntityFrameworkCore;
using System;
using WpfRequestResponseLogger.Models;

public class DataContext2 : DbContext
{
    private readonly string _connectionString;
    private readonly string _provider; // sqlserver or mysql

    public DataContext2(string connectionString, string provider)
    {
        _connectionString = connectionString;
        _provider = provider.ToLower(); 
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_provider == "sqlserver")
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        else if (_provider == "mariadb" || _provider == "mysql")
        {
            optionsBuilder.UseMySql(
                _connectionString,
                new MariaDbServerVersion(new Version(10, 4, 32)) // Version of MariaDB
            );
        }
        else
        {
            throw new InvalidOperationException("Database not supported");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogData>().HasNoKey();
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<RequestLog> RequestLogs { get; set; }
    public DbSet<ResponseLog> ResponseLogs { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<BlacklistedIp> BlacklistedIps { get; set; }
}
