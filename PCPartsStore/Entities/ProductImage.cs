using System.ComponentModel.DataAnnotations.Schema;

namespace PCPartsStore.Entities;

public class ProductImage
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string Name { get; set; }

    public string ImagePath { get; set; }

    public ICollection<Product> Products { get; }
}