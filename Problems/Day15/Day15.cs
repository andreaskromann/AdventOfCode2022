using System.Text.RegularExpressions;

namespace AdventOfCode2022.Problems.Day15;

class Day15 : ICodingProblem
{
    public void Run()
    {
        var sensors = new Dictionary<Position, int>();
        var beacons = new HashSet<Position>();
        var x_min = int.MaxValue;
        var x_max = int.MinValue;

        var row = 2000000;
        var max = 4000000;
        //var row = 10;
        //var max = 20; 

        foreach (var line in File.ReadAllLines("Problems\\Day15\\input.txt"))
        {
            var match = Regex.Match(
                line,
                @"Sensor at x=(?<sx>[-\d]+), y=(?<sy>[-\d]+): closest beacon is at x=(?<bx>[-\d]+), y=(?<by>[-\d]+)");
            var sensor = new Position(int.Parse(match.Groups["sx"].Value), int.Parse(match.Groups["sy"].Value));
            var beacon = new Position(int.Parse(match.Groups["bx"].Value), int.Parse(match.Groups["by"].Value));
            beacons.Add(beacon);
            var distance = Distance(sensor, beacon);
            sensors.Add(sensor, distance);
            if (sensor.X - distance < x_min) x_min = sensor.X - distance;
            if (sensor.X + distance > x_max) x_max = sensor.X + distance;
        }

        var count = 0;
        Parallel.For(x_min, x_max + 1, x =>
        {
            var current = new Position(x, row);
            if (sensors.Any(s => Distance(s.Key, current) <= s.Value) &&
                !sensors.ContainsKey(current) &&
                !beacons.Contains(current))
                Interlocked.Increment(ref count);
        });

        Console.WriteLine($"Part 1: {count}");

        Parallel.For(0, max + 1, j =>
        {
            for (var i = 0; i <= max;)
            {
                var current = new Position(i, j);
                var found = true;
                var newi = i + 1;
                foreach (var sensor in sensors)
                {
                    var d = Distance(sensor.Key, current);
                    if (d <= sensor.Value)
                    {
                        found = false;
                        newi = Math.Max(newi, i + sensor.Value - d);
                    }
                }

                if (found)
                {
                    Console.WriteLine($"Part 2: {current.X * 4000000L + current.Y}");
                    return;
                }

                i = newi;
            }
        });
    }

    int Distance(Position a, Position b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

    record Position(int X, int Y);
}