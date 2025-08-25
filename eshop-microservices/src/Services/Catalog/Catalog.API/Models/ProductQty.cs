using System.ComponentModel;

namespace Catalog.API.Models;

public class ProductQty
{
    
    [Description("Available for sale stock. Might exclude reserved or damaged items.")]
    public int Available { get; set; }

    [Description("Total physically present items in the warehouse")]
    public int Onhand { get; set; }

    [Description("Quantity of this product ordered from supplier but not yet received")]
    public int OnOrder { get; set; }

    [Description("Quantity already ordered by customers but not yet fulfilled")]
    public int OnCustOrder { get; set; }

    [Description("Quantity of this product that is currently on backorder")]
    public int OnBackOrder { get; set; }

    [Description("Quantity that is in the reorder process")]
    public int OnReOrder { get; set; }

    [Description("Quantity of product being returned or under quality check")]
    public int OnReturn { get; set; }
}
