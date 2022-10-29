namespace Recursive.Models.ViewModels;

public class ViewModel
{
    public int Id { get; set; }
    public int? Pid { get; set; }
    public string Name { get; set; }
    public List<ViewModel> SubList { get; set; } = new();
}