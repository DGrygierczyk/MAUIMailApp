namespace MailApp.Model;

public class EmailEnvelope
{
    public int Id { get; set; }
    public string From { get; set; }
    public string Subject { get; set; }
    public DateTime Date { get; set; }
    public bool IsRead { get; set; }
}