namespace AdventOfCode2022.Problems.Day06;

class Day6 : ICodingProblem
{
    public void Run()
    {
        var signal = File.ReadAllText("Problems\\Day06\\input.txt").Trim();
        SearchForUniqueString(signal, 4, 1);
        SearchForUniqueString(signal, 14, 2);
    }

    private static void SearchForUniqueString(string signal, int patternLength, int partNumber)
    {
        var counts = new Dictionary<char, int>();
        for (var index = 0; index < signal.Length; index++)
        {
            if (index > patternLength - 1)
            {
                var toBeRemoved = signal[index - patternLength];
                counts[toBeRemoved] -= 1;
            }

            var toBeAdded = signal[index];
            if (!counts.ContainsKey(toBeAdded))
                counts[toBeAdded] = 0;
            counts[toBeAdded] += 1;

            if (index >= patternLength - 1 && counts.Values.All(x => x < 2))
            {
                Console.WriteLine($"Part {partNumber}: {index + 1}");
                return;
            }
        }
    }
}