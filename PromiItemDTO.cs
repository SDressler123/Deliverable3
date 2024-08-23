public class PromiItemDTO
{
    public int EmployeeID { get; set; }
    public string? FName { get; set; }
    public bool IsManager { get; set; }

    public PromiItemDTO() { }
    public PromiItemDTO(Promi PromiItem) =>
    (EmployeeID, FName, IsManager) = (PromiItem.EmployeeID, PromiItem.FName, PromiItem.IsManager);
}