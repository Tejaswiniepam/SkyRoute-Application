public class BookingResponse
{
    public string BookingReference { get; set; } = string.Empty;

    public decimal FarePerPassenger { get; set; }

    public decimal TotalFare { get; set; }
    public int PassengerCount { get; set; }
    public List<string> PassengerNames { get; set; } = new();
}