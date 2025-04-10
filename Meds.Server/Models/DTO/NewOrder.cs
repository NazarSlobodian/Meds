
public class NewOrder
{
    public List<int> TestTypesIds { get; set; }
    public List<int> PanelsIds { get; set; }
    public NewOrder(List<int> testTypes, List<int> panels)
    {
        TestTypesIds = testTypes;
        PanelsIds = panels;
    }
    public NewOrder () { }

}
