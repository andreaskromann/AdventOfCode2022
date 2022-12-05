namespace AdventOfCode2022.Problems.Day05;

class Day5 : ICodingProblem
{
    public void Run()
    {
        var parts = File.ReadAllText("Problems\\Day05\\input.txt").Split("\n\n");
        var stackLines = parts[0].Split("\n");
        var numberOfStacks = int.Parse(stackLines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
        var moves = parts[1].Split("\n");

        var stacks = new Stack<char>[numberOfStacks];
        for (int i = 0; i < numberOfStacks; i++)
            stacks[i] = new Stack<char>();
        
        foreach (var stackLine in stackLines.Reverse().Skip(1))
        {
            for (var i = 1; i <= numberOfStacks; i++)
            {
                var index = 4 * (i - 1) + 1;
                if (stackLine.Length > index && stackLine[index] != ' ')
                    stacks[i - 1].Push(stackLine[index]);
            }
        }
        
        foreach (var move in moves)
        {
            if (string.IsNullOrWhiteSpace(move))
                continue;
            var moveParts = move.Split(' ');
            var numberOfCrates = int.Parse(moveParts[1]);
            var from = int.Parse(moveParts[3]);
            var to = int.Parse(moveParts[5]);
            for (var i = 0; i < numberOfCrates; i++)
            {
                var crate = stacks[from - 1].Pop();
                stacks[to - 1].Push(crate);
            }
        }
        
        Console.Write("Part 1: ");
        foreach (var stack in stacks)
            Console.Write(stack.Peek());
        Console.Write("\n");
    }
}