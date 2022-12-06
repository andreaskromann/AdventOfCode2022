namespace AdventOfCode2022.Problems.Day06;

class Day6 : ICodingProblem
{
    public void Run()
    {
        var signal = File.ReadAllText("Problems\\Day06\\input.txt").Trim();
        var counts = new Dictionary<char, int>();
        for (var index = 0; index < signal.Length; index++)
        {
            if (index > 3)
            {
                var toBeRemoved = signal[index - 4];
                counts[toBeRemoved] -= 1;
            }

            var toBeAdded = signal[index];
            if (!counts.ContainsKey(toBeAdded))
                counts[toBeAdded] = 0;
            counts[toBeAdded] += 1;

            if (index >= 3 && counts.Values.All(x => x < 2))
            {
                Console.WriteLine($"Part 1: {index + 1}");
                break;
            }
        }
    }
}