namespace AdventOfCode2022.Problems.Day03;

class Day3 : ICodingProblem
{
    public void Run()
    {
        // Part 1
        var sum = 0;
        foreach (var rucksack in File.ReadAllLines("Problems\\Day03\\input.txt"))
        {
            var compartment1 = new HashSet<char>(rucksack.Substring(0, rucksack.Length / 2));
            var compartment2 = new HashSet<char>(rucksack.Substring(rucksack.Length / 2));
            compartment1.IntersectWith(compartment2);
            sum += GetScore(compartment1.First());
        }
        Console.WriteLine($"Part 1: {sum}");
        
        // Part 2
        sum = 0;
        foreach (var group in File.ReadAllLines("Problems\\Day03\\input.txt").Chunk(3))
        {
            var rucksack1 = new HashSet<char>(group[0]);
            var rucksack2 = new HashSet<char>(group[1]);
            var rucksack3 = new HashSet<char>(group[2]);
            rucksack1.IntersectWith(rucksack2);
            rucksack1.IntersectWith(rucksack3);
            sum += GetScore(rucksack1.First());
        }
        Console.WriteLine($"Part 2: {sum}");
    }

    private int GetScore(char c)
    {
        if (c is >= 'a' and <= 'z')
            return c - 'a' + 1;
        return c - 'A' + 27;
    }
}