namespace TokoApi.DTOs;

/// <summary>Request body untuk pembayaran cicilan kasbon.</summary>
public class BayarCicilanRequest
{
    /// <summary>Jumlah uang yang dibayarkan sekarang (dalam Rupiah).</summary>
    public decimal JumlahBayar { get; set; }
}
