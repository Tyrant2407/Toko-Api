using System.Collections.Generic;

namespace TokoApi.DTOs;

public class CheckoutItemRequest
{
    public int ProdukId { get; set; }
    public int JumlahBeli { get; set; }
}

public class CheckoutRequest
{
    public string Pembeli { get; set; } = string.Empty;
    public string StatusPembayaran { get; set; } = "Lunas"; // "Lunas" atau "Kasbon"
    public List<CheckoutItemRequest> Items { get; set; } = new List<CheckoutItemRequest>();
}
