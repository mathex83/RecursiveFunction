using Recursive.Models.CSharpModels;
using Recursive.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Reflection;
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

        List<PViewModel> parentViewList = new();
        foreach (Model a in parentList)
        {
            PViewModel aItem = new() { Id = a.Id, Name = a.Name };
            a.Children = virtualDbList.Where(x => x.ParentId == a.Id).ToList();
            foreach (Model b in a.Children)
            {
                b.Children = virtualDbList.Where(x => x.ParentId == b.Id).ToList();
                SViewModel bItem = new() { Id = b.Id, Pid = (int)b.ParentId, Name = b.Name };
                List<ThirdPlusView> thirdPlusViewList = new();
                foreach (Model c in b.Children)
                {
                    Re(c,thirdPlusViewList);
                }
                bItem.SecondChildList = thirdPlusViewList;
                aItem.FirstChildList.Add(bItem);
            }
            parentViewList.Add(aItem);
        }
        Console.WriteLine("BuildViewModelsFlatFromThird:");
        Console.WriteLine(JsonSerializer.Serialize<List<PViewModel>>(parentViewList, new JsonSerializerOptions { WriteIndented = true }));
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

    static void R(List<ViewModel> viewList, Model model, List<Model> modelList)
    {
        Model newItem = modelList.First(x => x.Id == model.ParentId);
        if (newItem is not null)
        {
            ViewModel viewModel = new() { Id=newItem.Id, Pid = newItem.ParentId, Name = newItem.Name };
            viewList.Add(viewModel);
            R(viewList, newItem, modelList);
        }
    }
    static void Re(Model c, List<ThirdPlusView> viewList)
    {
        if (c.Children?.Count > 0)
        {
            viewList.Add(new() { Id = c.Id, Pid = (int)c.ParentId, Name = c.Name });
            foreach (var model in c.Children)
            {
                ThirdPlusView view = new() { Id = model.Id, Pid = (int)model.ParentId, Name = model.Name };
                viewList.Add(view);
                Re(model, viewList);
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