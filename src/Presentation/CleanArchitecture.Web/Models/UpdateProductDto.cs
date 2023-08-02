namespace CleanArchitecture.Web.Models;

public class UpdateProductDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
}
