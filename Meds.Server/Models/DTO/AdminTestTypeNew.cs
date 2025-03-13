public class AdminTestTypeNew
{ 

    public string TestName { get; set; } = null!;

    public decimal Cost { get; set; }
    public int DaysTillOverdue { get; set; }
    public string MeasurementsUnit { get; set; }

    public AdminTestTypeNew() { }
}