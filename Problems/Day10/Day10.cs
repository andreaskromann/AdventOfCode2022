namespace AdventOfCode2022.Problems.Day10;

class Day10 : ICodingProblem
{
    public void Run()
    {
        var lines = File.ReadAllLines("Problems\\Day10\\input.txt");
        var register = 1;
        var cycle = 0;
        var sum = 0;

        void incCycle()
        {
            cycle++;
            if (cycle == 20 || (cycle >= 60 && (cycle - 60) % 40 == 0))
                sum += cycle * register;
        }
        
        foreach (var line in lines)
        {
            if (line.StartsWith("noop"))
                incCycle();
            
            if (line.StartsWith("addx"))
            {
                var value = int.Parse(line.Split(' ')[1]);
                for (var i = 0; i < 2; i++)
                    incCycle();
                register += value;
            }
        }
        
        Console.WriteLine($"Part 1: {sum}");
    }
}