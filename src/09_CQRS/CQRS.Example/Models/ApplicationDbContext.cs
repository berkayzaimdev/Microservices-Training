namespace CQRS.Example.Models;

public static class ApplicationDbContext
{
    public static ICollection<Product> Products { get; set; } = [];
}