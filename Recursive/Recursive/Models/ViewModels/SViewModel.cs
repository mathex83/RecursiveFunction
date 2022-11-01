namespace Recursive.Models.ViewModels;

public class SViewModel
{
    public int Id { get; set; }
    public int Pid { get; set; }
    public string Name { get; set; }
    public List<ThirdPlusView> SecondChildList { get; set; } = new();
}