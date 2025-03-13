public class RoleData
{
    public string RoleName { get; set; }
    public int AccountId { get; set; }
    public RoleData(string name, int id)
    {
        RoleName = name;
        AccountId = id;
    }

}