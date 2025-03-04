namespace BryanJonatan_Acceloka.Model
{
    public class GetBookedTicketResponse
    {
        public string KodeTiket { get; set; } = string.Empty;
        public string NamaTiket { get; set; } = string.Empty;
        public EventDateRange TanggalEvent { get; set; } = new();
        public int Quantity { get; set; }
        public string Kategori { get; set; } = string.Empty;
    }
}
