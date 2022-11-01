namespace Recursive.Models.ViewModels;

public class PViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<SViewModel> FirstChildList { get; set; } = new();
}
