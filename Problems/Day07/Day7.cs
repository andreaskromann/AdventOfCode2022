namespace AdventOfCode2022.Problems.Day07;

class Day7 : ICodingProblem
{
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
        long sumPart1 = 0;
        var usedSpace = CalculateSize(root, ref sumPart1);
        Console.WriteLine($"Part 1: {sumPart1}");

        const long fileSystemSize = 70000000;
        const long neededFreeSpace = 30000000;
        var unused = fileSystemSize - usedSpace;
        var needed = neededFreeSpace - unused;
        var result = FindDirectory(root, needed);
        Console.WriteLine($"Part 2: {result.Size}");
    }

    static Directory? FindDirectory(Directory directory, long size)
    {
        Directory? result = null;
        foreach (var child in directory.Children)
        {
            if (child is Directory d)
            {
                var candidate = FindDirectory(d, size);
                if (candidate is not null && candidate.Size < (result?.Size ?? long.MaxValue))
                    result = candidate;
            }
        }

        if (directory.Size >= size && directory.Size < (result?.Size ?? long.MaxValue))
            result = directory;

        return result;
    }

    static long CalculateSize(Entry entry, ref long sum)
    {
        if (entry is File file)
            return file.Size;

        var directory = (Directory)entry;
        if (directory.Size > 0)
            return directory.Size;
        
        foreach (var child in directory.Children)
            directory.Size += CalculateSize(child, ref sum);

        if (directory.Size <= 100000)
            sum += directory.Size;
        
        return directory.Size;
    }

    abstract class Entry
    {
        public string Name { get; init; }
        public long Size { get; set; }
    };
    
    class File: Entry {}

    class Directory : Entry
    {
        public Directory? Parent { get; init; }
        public List<Entry> Children { get; } = new ();
    }
}