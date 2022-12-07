namespace AdventOfCode2022.Problems.Day07;

class Day7 : ICodingProblem
{
    private long sumPart1 = 0;
    
    public void Run()
    {
        var root = new Directory { Name = "/" };
        var current = root;
        foreach (var line in System.IO.File.ReadAllLines("Problems\\Day07\\input.txt"))
        {
            if (line[0] == '$')
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                switch (parts[1])
                {
                    case "cd":
                        if (parts[2] == "/")
                            current = root;
                        else if (parts[2] == "..")
                            current = current.Parent;
                        else
                            current = (Directory)current.Children.First(x => x.Name == parts[2]);
                        break;
                    case "ls":
                        break;
                }
            }
            else
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (parts[0] == "dir")
                    current.Children.Add(new Directory {Parent = current, Name = parts[1]});
                else
                    current.Children.Add(new File { Size = long.Parse(parts[0]), Name = parts[1]});
            }
        }

        CalculateSize(root);
        Console.WriteLine($"Part 1: {sumPart1}");
    }

    long CalculateSize(Entry entry)
    {
        if (entry is File file)
            return file.Size;

        var directory = (Directory)entry;
        if (directory.Size > 0)
            return directory.Size;
        
        foreach (var child in directory.Children)
            directory.Size += CalculateSize(child);

        if (directory.Size <= 100000)
            sumPart1 += directory.Size;
        
        return directory.Size;
    }

    abstract class Entry
    {
        public string Name { get; set; }
        public long Size { get; set; }
    };
    
    class File: Entry {}

    class Directory : Entry
    {
        public Directory? Parent { get; set; }
        public List<Entry> Children { get; } = new ();
    }
}