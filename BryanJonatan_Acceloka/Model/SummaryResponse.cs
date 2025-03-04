namespace BryanJonatan_Acceloka.Model
{
    public class SummaryResponse
    {
        public Dictionary<string, int> CategoryTotals { get; set; } = new();
        public int GrandTotal { get; set; }
    }
}
