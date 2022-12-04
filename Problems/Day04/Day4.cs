namespace AdventOfCode2022.Problems.Day04;

class Day4 : ICodingProblem
{
    public void Run()
    {
        // Part 1
        var countPart1 = 0;
        var countPart2 = 0; 
        foreach (var pair in File.ReadAllLines("Problems\\Day04\\input.txt"))
        {
            var assignments = pair.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var firstRange = assignments[0].Split('-').Select(int.Parse).ToArray();
            var secondRange = assignments[1].Split('-').Select(int.Parse).ToArray();
            if ((firstRange[0] >= secondRange[0] && firstRange[1] <= secondRange[1]) ||
                (firstRange[0] <= secondRange[0] && firstRange[1] >= secondRange[1]))
                countPart1++;
            if ((firstRange[0] <= secondRange[1] && firstRange[1] >= secondRange[0]))
                countPart2++;
        }
        Console.WriteLine($"Part 1: {countPart1}");
        Console.WriteLine($"Part 2: {countPart2}");
    }
}