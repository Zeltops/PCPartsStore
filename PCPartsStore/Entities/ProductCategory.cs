using System.ComponentModel.DataAnnotations.Schema;

namespace PCPartsStore.Entities;

public class ProductCategory
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string Name { get; set; }
    
    public ICollection<Product> Products { get; }
}