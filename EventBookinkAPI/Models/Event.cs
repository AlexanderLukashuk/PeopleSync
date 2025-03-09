namespace EventBookinkAPI.Models;

public class Event
{
    public int id { get; set; }
    public string event_name { get; set; }
    public int max_participants { get; set; }
    public DateTime event_date { get; set; }
    public DateTime created_at { get; set; }
}