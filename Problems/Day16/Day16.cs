using System.Text.RegularExpressions;

namespace AdventOfCode2022.Problems.Day16;

class Day16 : ICodingProblem
{
    public void Run()
    {
        var valves = new Dictionary<string, Valve>();
        foreach (var lines in File.ReadAllLines("Problems\\Day16\\input.txt"))
        {
            var match = Regex.Match(lines, @"Valve (?<valve>[A-Z]{2}) has flow rate=(?<flow>\d+); tunnel(s)? lead(s)? to valve(s)? (?<tunnels>.+)");
            var valve = match.Groups["valve"].Value;
            var flow = int.Parse(match.Groups["flow"].Value);
            var tunnels = match.Groups["tunnels"].Value;
            var newValve = new Valve 
            { 
                Id = valve, 
                Flow = flow, 
                Connections = tunnels.Split(new[] {',', ' '}, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries ) 
            };
            valves.Add(valve, newValve);
        }

        void BFS(Valve v)
        {
            foreach(var valve in valves)
                valve.Value.Distance = 0;
            var queue = new Queue<(Valve,int)>();
            queue.Enqueue((v,0));
            while (queue.TryDequeue(out var item))
            {
                item.Item1.Distance = item.Item2;
                foreach(var child in item.Item1.Connections)
                {
                    if (valves[child].Distance == 0)
                        queue.Enqueue((valves[child], item.Item2 + 1));
                }
            }
        }

        var distanceMatrix = new Dictionary<(string,string), int>();
        foreach(var v in valves)
        {
            BFS(v.Value);
            foreach(var o in valves)
            {
                distanceMatrix[(v.Key,o.Key)] = v.Key == o.Key ? 0 : o.Value.Distance;
                distanceMatrix[(o.Key,v.Key)] = v.Key == o.Key ? 0 : o.Value.Distance;
            }
        }

        int CalculateScore(List<string> currentPermutation, bool print)
        {
            currentPermutation.Insert(0, "AA");
            var score = 0;
            var tick = 0;
            var current = 1;
            var distance = distanceMatrix[("AA", currentPermutation[current])];
            var open = new List<string>();
            
            for (var round = 1; round <= 30; round++)
            {
                if (print)
                {
                    Console.WriteLine();
                    Console.WriteLine($"== Minute {round} ==");
                    if (open.Count == 0)
                        Console.WriteLine("No valves are open.");
                    if (open.Count == 1)
                        Console.WriteLine($"Valve {open[0]} is open, releasing {tick} pressure.");
                    if (open.Count > 1)
                        Console.WriteLine($"Valves {string.Join(" and ", open)} are open, releasing {tick} pressure.");
                }

                score += tick;
                if (distance > 0)
                {
                    if (print)
                        Console.WriteLine($"You move to valve {currentPermutation[current]}.");
                    distance--; // Move
                }
                else if (current < currentPermutation.Count)
                {
                    var valve = valves[currentPermutation[current]];
                    if (valve.Flow > 0)
                    {
                        tick += valve.Flow; // Open valve
                        if (print)
                        {
                            Console.WriteLine($"You open valve {valve.Id}.");
                            open.Add(valve.Id);
                        }
                    }

                    // Pick next
                    current++;
                    if (current < currentPermutation.Count)
                        distance = distanceMatrix[(valve.Id, currentPermutation[current])];
                }
            }

            if (print)
                Console.WriteLine($"Total pressure released: {score}.");
            return score;
        }

        //CalculateScore(new List<string> { "DD", "BB", "JJ", "HH", "EE", "CC" }, true);
        
        var best = int.MinValue;
        var skipped = 0L;
        var calculated = 0L;
        void Permute(List<string> currentPermutation, string[] remaining, int currentLength)
        {
            if (remaining.Length > 0)
            {
                foreach (var id in remaining)
                {
                    if (currentPermutation.Count > 0 &&
                        currentLength + distanceMatrix[(currentPermutation[^1], id)] > 28)
                    {
                        skipped++;
                        continue;
                    }

                    var newPermutation = new List<string>(currentPermutation);
                    newPermutation.Add(id);
                    var newRemaining = remaining.Where(x => x != id).ToArray();
                    var newLength = currentPermutation.Count > 0 ? currentLength + distanceMatrix[(currentPermutation[^1],id)] : 0;
                    Permute(newPermutation, newRemaining, newLength);
                }
                if (currentPermutation.Count > 1)
                {
                    calculated++;
                    var currentScore = CalculateScore(currentPermutation, false);
                    if (currentScore > best)
                        best = currentScore;
                }
            }
            else
            {
                calculated++;
                var score = CalculateScore(currentPermutation, false);
                if (score > best)
                    best = score;
            }
        }
        Permute(new List<string>(), valves.Where(x => x.Value.Flow > 0).Select(x => x.Key).ToArray(), 0);
        Console.WriteLine($"Skipped: {skipped}, Calculated: {calculated}."); // We end up doing 12.8 mio calculate calls.
        Console.WriteLine($"Part 1: {best}");
    }

    class Valve 
    {
        public string Id { get; init; }
        public int Distance { get; set; }
        public int Flow { get; init; }
        public string[] Connections { get; init; }
    }
}