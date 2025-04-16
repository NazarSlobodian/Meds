public class ActivityLogRequest
{
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }
    public int Page {  get; set; }
    public int PageSize { get; set; }
}