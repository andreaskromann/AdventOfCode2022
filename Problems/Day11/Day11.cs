namespace AdventOfCode2022.Problems.Day11;

class Day11 : ICodingProblem
{
    public void Run()
    {
        var input = File.ReadAllText("Problems\\Day11\\input.txt");
        var monkeys = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(ParseMonkey)
            .ToArray();

        Play(20, monkeys, 0);
        var monkeyBusinessLevel = CalcMonkeyBusinessLevel(monkeys);
        Console.WriteLine($"Part 1: {monkeyBusinessLevel}");
        
        monkeys = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(ParseMonkey)
            .ToArray();
        
        // https://en.wikipedia.org/wiki/Least_common_multiple
        // The divisors are all prime numbers, so lcm is just multiplying them together.
        var lcm = monkeys.Select(x => x.Divisor).Aggregate((x, y) => x * y);
        
        Play(10000, monkeys, lcm);
        monkeyBusinessLevel = CalcMonkeyBusinessLevel(monkeys);
        Console.WriteLine($"Part 2: {monkeyBusinessLevel}");
    }

    private static long CalcMonkeyBusinessLevel(Monkey[] monkeys)
    {
        var monkeyBusinessLevel = monkeys
            .Select(x => x.Inspections)
            .OrderDescending()
            .Take(2)
            .Aggregate((x, y) => x * y);
        return monkeyBusinessLevel;
    }

    private static void Play(int rounds, Monkey[] monkeys, int lcm)
    {
        for (var round = 0; round < rounds; round++)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.TryDequeue(out var item))
                {
                    monkey.Inspections++;
                    var worryLevel = monkey.Operation(item);
                    if (lcm == 0)
                        worryLevel /= 3;
                    else
                        worryLevel %= lcm; // Keep worry levels manageable without messing with division tests
                    var to = monkey.Test(worryLevel);
                    monkeys[to].Items.Enqueue(worryLevel);
                }
            }
        }
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

        var testDivNumber = int.Parse(lines[3].Substring(lines[3].IndexOf("by", StringComparison.Ordinal) + 3));
        var trueMonkey = int.Parse(lines[4].Substring(lines[4].IndexOf("monkey", StringComparison.Ordinal) + 7));
        var falseMonkey = int.Parse(lines[5].Substring(lines[5].IndexOf("monkey", StringComparison.Ordinal) + 7));
        Func<long, int> test = x => x % testDivNumber == 0 ? trueMonkey : falseMonkey;
        
        return new Monkey
        {
            Items = new Queue<long>(items),
            Operation = operation,
            Test = test,
            Divisor = testDivNumber
        };
    }

    class Monkey
    {
        public long Inspections;
        public Queue<long> Items { get; init; }
        public Func<long,long> Operation { get; init; }
        public Func<long,int> Test { get; init; }
        public int Divisor;
    }
}