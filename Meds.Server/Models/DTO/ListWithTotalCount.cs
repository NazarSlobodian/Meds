public class ListWithTotalCount<T>
{
    public List<T> List { get; set; }
    public int TotalCount { get; set; }
    public ListWithTotalCount(List<T> list, int totalCount)
    {
        this.List = list;
        this.TotalCount = totalCount;
    }
    public ListWithTotalCount() {
        List = new List<T>();
        TotalCount = 0;
    }
}