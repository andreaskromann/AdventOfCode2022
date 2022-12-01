namespace AdventOfCode2022.Problems.Day01;

class Day1 : ICodingProblem
{
    public void Run()
    {
        var caloriesMap = new Dictionary<int, int>();
        var current = 0;
        foreach (var line in File.ReadAllLines("Problems\\Day01\\input.txt"))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                current++;
                continue;
            }

            var calories = int.Parse(line);
            if (!caloriesMap.ContainsKey(current))
                caloriesMap[current] = 0;
            caloriesMap[current] += calories;
        }
        Console.WriteLine($@"Number of elves: {caloriesMap.Count}");
        Console.WriteLine($"Part 1: {caloriesMap.Values.Max()}");
        Console.WriteLine($"Part 2: {caloriesMap.Values.OrderDescending().Take(3).Sum()}");
    }
    
    /*
        Number of elves: 255
        Part 1: 75501
        Part 2: 215594

        Elapsed = 6ms
     */
}