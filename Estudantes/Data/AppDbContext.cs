// db context deu ruim ? lembre de dar o using. Não tem o pacote ? instala via CLI e deixa de ser bobao. nao sabe oque é CLI ? é basicamente oq tu pode rodar no console. 
//dotnet add package Microsoft.EntityFrameworkCore --version 8.0.7
using MinhaWebAPI.Estudantes;
using Microsoft.EntityFrameworkCore; 
namespace MinhaWebAPI.Data;


// AppDbContext o que caralhinhos voadores seria esse Fucking DBContext ? AppDbContext é um nome que geralmente é utilizado para tudo que tem uma representação para um conjunto de tabelas... ou seja é as tables.
public class AppDbContext : DbContext 
{
    public DbSet<Estudante> Estudantes {get; set;}

// dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);// Pra quê isso ? aqui estou dizendo com o LogTo q as informaçoes sobre oq o EF está fazendo devem aparecer no terminal. E o log level ? quando tu faz os dados aparecerem no terminal tu pode filtrar oque seria interessante, nesse caso LogLevel Information só me traz informaçoes sobre pesquisas, inserts e etc...
        optionsBuilder.UseSqlServer("Server=localhost;Database=MinhaWebAPI;User Id=sa;Password=MinhaSenhaForte$;TrustServerCertificate=True;");
        optionsBuilder.EnableSensitiveDataLogging(); // aqui estou dizendo que as informaçoes sensiveis podem ser exibidas... apenas é interessanta usar em banco de desenvolvimento.
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estudante>()
            .Property(e => e.Nome)
            .HasMaxLength(100)
            .IsRequired();
        base.OnModelCreating(modelBuilder);
    }
}