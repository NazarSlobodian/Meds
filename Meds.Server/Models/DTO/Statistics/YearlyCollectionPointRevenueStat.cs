
public class YearlyCollectionPointRevenueStat
{
    public int Year { get; set; }
    public string LabAddress { get; set; }
    public List<MonthlyCollectionPointRevenueStat> Stats { get; set; }
    public YearlyCollectionPointRevenueStat() { }
}