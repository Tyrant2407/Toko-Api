using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace TokoApi; 

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // KITA UBAH BARIS INI: Gunakan kunci asli tanpa password ke TokoApiDb
        var connectionString = "Server=localhost;Port=3306;Database=TokoApiDb;User=root;Password=;";
        
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 30)); 
        
        optionsBuilder.UseMySql(connectionString, serverVersion);

        return new AppDbContext(optionsBuilder.Options);
    }
}