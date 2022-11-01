namespace Recursive.Models.CSharpModels;

public class Model
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Name { get; set; }
    public List<Model> Children { get; set; } = new();
    
    public static List<Model> CreateModelList()
    {
        List<Model> virtualDbList = new();
        int id = 0;
        for (int i = 1; i < 3; i++)
        {
            id++;
            Model w = new()
            {
                Id = id,
                Name = i.ToString()
            };
            virtualDbList.Add(w);
            for (int j = 1; j < 3; j++)
            {
                id++;
                Model x = new()
                {
                    Id = id,
                    ParentId = w.Id,
                    Name = $"{i}.{j}"
                };
                virtualDbList.Add(x);
                for (int k = 1; k < 3; k++)
                {
                    id++;
                    Model y = new()
                    {
                        Id = id,
                        ParentId = x.Id,
                        Name = $"{i}.{j}.{k}"
                    };
                    virtualDbList.Add(y);
                    for (int l = 1; l < 3; l++)
                    {
                        id++;
                        Model z = new()
                        {
                            Id = id,
                            ParentId = y.Id,
                            Name = $"{i}.{j}.{k}.{l}"
                        };
                        virtualDbList.Add(z);
                        for (int m = 1; m < 3; m++)
                        {
                            id++;
                            Model v = new()
                            {
                                Id = id,
                                ParentId = y.Id,
                                Name = $"{i}.{j}.{k}.{l}.{m}"
                            };
                            virtualDbList.Add(v);
                            for (int n = 1; n < 3; n++)
                            {
                                id++;
                                Model u = new()
                                {
                                    Id = id,
                                    ParentId = y.Id,
                                    Name = $"{i}.{j}.{k}.{l}.{m}.{n}"
                                };
                                virtualDbList.Add(u);
                            }
                        }
                    }
                }
            }
        }
        return virtualDbList.OrderBy(x => x.ParentId).ThenBy(x => x.Id).ToList();
    }
}