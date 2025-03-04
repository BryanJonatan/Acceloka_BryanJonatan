namespace BryanJonatan_Acceloka.Model
{
    public class EditBookedTicketResponse
    {
        public string KodeTicket { get; set; } = string.Empty;
        public string NamaTicket { get; set; } = string.Empty;
        public string NamaKategori { get; set; } = string.Empty;
        public int SisaQuantity { get; set; }
    }
}
