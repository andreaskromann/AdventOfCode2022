namespace AdventOfCode2022.Problems.Day11;

class Day11 : ICodingProblem
{
    public void Run()
    {
        var input = File.ReadAllText("Problems\\Day11\\input.txt");
        var monkeys = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(ParseMonkey)
            .ToArray();
        
        Console.WriteLine($"Total number of items: {monkeys.Sum(x => x.Items.Count)}");

        for (var round = 0; round < 20; round++)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.TryDequeue(out var item))
                {
                    monkey.Inspections++;
                    var worryLevel = monkey.Operation(item);
                    worryLevel /= 3;
                    var to = monkey.Test(worryLevel);
                    monkeys[to].Items.Enqueue(worryLevel);
                }
            }
        }

        foreach (var monkey in monkeys)
            Console.WriteLine($"Monkey inspected items {monkey.Inspections} times.");
        
        var monkeyBusinessLevel = monkeys
            .Select(x => x.Inspections)
            .OrderDescending()
            .Take(2)
            .Aggregate((x, y) => x * y);
        Console.WriteLine($"Part 1: {monkeyBusinessLevel}");
    }

    private Monkey ParseMonkey(string s)
    {
        var lines = s.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var items = lines[1]
            .Substring(lines[1].IndexOf(':') + 1)
            .Split(new []{' ', ','}, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse);
        
        var operationParts = lines[2]
            .Substring(lines[2].IndexOf("old", StringComparison.Ordinal) + 4)
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        Func<long, long> operation;
        if (operationParts[0] == "*")
        {
            if (operationParts[1] == "old")
                operation = x => x * x;
            else
                operation = x => x * int.Parse(operationParts[1]);
        }
        else
        {
            if (operationParts[1] == "old")
                operation = x => x + x;
            else
                operation = x => x + int.Parse(operationParts[1]);
        }

        var testDivNumber = long.Parse(lines[3].Substring(lines[3].IndexOf("by", StringComparison.Ordinal) + 3));
        var trueMonkey = long.Parse(lines[4].Substring(lines[4].IndexOf("monkey", StringComparison.Ordinal) + 7));
        var falseMonkey = long.Parse(lines[5].Substring(lines[5].IndexOf("monkey", StringComparison.Ordinal) + 7));
        Func<long, long> test = x => x % testDivNumber == 0 ? trueMonkey : falseMonkey;
        
        return new Monkey
        {
            Items = new Queue<long>(items),
            Operation = operation,
            Test = test
        };
    }

    class Monkey
    {
        public int Inspections;
        public Queue<long> Items { get; init; }
        public Func<long,long> Operation { get; init; }
        public Func<long,long> Test { get; init; }
    }
}