using Recursive.Models.CSharpModels;
using Recursive.Models.ViewModels;
using System.Text.Json;

namespace Recursive;

internal class Program
{
    static List<Model> BuildModelTreeFromOrigin(List<Model> modelList)
    {
        foreach (var item in modelList)
        {
            item.Children = modelList.Where(x => x.ParentId == item.Id).ToList();
            ViewModel viewModel = new();
            foreach (var i in item.Children)
            {
                RecursiveBuildModel(item.Children, i);
            }
        }
        return modelList;
    }
    static void Main(string[] args)
    {
        List<Model> virtualDbList = Model.CreateModelList();
        List<Model> parentList = virtualDbList.Where(x => x.ParentId == null).ToList();

        virtualDbList = BuildModelTreeFromOrigin(virtualDbList);

        Console.WriteLine("BuildViewModelsFromParent:");
        Console.WriteLine(JsonSerializer.Serialize<List<ViewModel>>(BuildViewModelsFromParent(parentList), new JsonSerializerOptions { WriteIndented = true }));

        Console.WriteLine("\nBuildViewModelsFromFirstChild");
        Console.WriteLine(JsonSerializer.Serialize<List<ParentViewModel>>(BuildViewModelsFromFirstChild(parentList), new JsonSerializerOptions { WriteIndented = true }));
    }

    //if parent is different than subelement
    static List<ParentViewModel> BuildViewModelsFromFirstChild(List<Model> parentList)
    {
        List<ParentViewModel> viewList = new();
        foreach (Model dr in parentList)
        {
            ParentViewModel root = new() { Id = dr.Id, Name = dr.Name };
            foreach (var drChild in dr.Children)
            {
                ViewModel rootChild = new() { Id = drChild.Id, Pid = drChild.ParentId, Name = drChild.Name };
                root.SubList.Add(rootChild);
                RecursiveBuildViews(rootChild, drChild, root.SubList);
            }
            viewList.Add(root);
        }
        return viewList;
    }

    static List<ViewModel> BuildViewModelsFromParent(List<Model> parentList)
    {
        List<ViewModel> viewList = new();

        foreach (Model dr in parentList)
        {
            ViewModel root = new() { Id = dr.Id, Pid = dr.ParentId, Name = dr.Name };
            viewList.Add(root);
            RecursiveBuildViews(root, dr, viewList);
        }
        return viewList;
    }

    static void RecursiveBuildViews(ViewModel root, Model model, List<ViewModel> viewList)
    {
        if (model.Children?.Count > 0)
        {
            foreach (var dr in model.Children)
            {
                ViewModel child = new() { Id = dr.Id, Pid = dr.ParentId, Name = dr.Name };
                root.SubList.Add(child);
                RecursiveBuildViews(child, dr, viewList);
            }
        }
    }

    static void RecursiveBuildModel(List<Model> source, Model buildNode)
    {
        List<Model> newSource = source.Where(a => a.Id == buildNode.ParentId).ToList();
        foreach (var i in newSource)
        {
            Model newNode = new() { Id = i.Id, ParentId = i.ParentId, Name = i.Name };

            if (buildNode is not null)
            {
                buildNode.Children.Add(newNode);
            }
            RecursiveBuildModel(source, newNode);
        }
    }
}