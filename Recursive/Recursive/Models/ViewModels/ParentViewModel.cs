namespace Recursive.Models.ViewModels;

public class ParentViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ViewModel> SubList { get; set; } = new();
}