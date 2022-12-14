namespace AdventOfCode2022.Problems.Day14;

class Day14 : ICodingProblem
{
    public void Run()
    {
        var lines = File.ReadAllLines("Problems\\Day14\\input.txt");
        var positions = new List<List<Position>>();
        foreach (var line in lines)
        {
            var current = new List<Position>();
            var points = line.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (var point in points)
            {
                var parts = point.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                current.Add(new Position(int.Parse(parts[0]), int.Parse(parts[1])));
            }
            positions.Add(current);
        }

        var x_min = positions.SelectMany(x => x.Select(y => y.X)).Min();
        var y_min = positions.SelectMany(x => x.Select(y => y.Y)).Min();
        var x_max = positions.SelectMany(x => x.Select(y => y.X)).Max();
        var y_max = positions.SelectMany(x => x.Select(y => y.Y)).Max();
        var min = Math.Min(x_min, 500 - y_max - 1);
        var width = Math.Max(x_max + 3, 500 + y_max + 3) - min;
        var height = y_max + 3;

        var map = new char[width, height];
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
                map[i, j] = '.';
        }

        void MapSet(int x, int y, char c)
        {
            map[x + 1 - min, y] = c;
        }

        foreach (var line in positions)
        {
            for (var i = 0; i < line.Count - 1; i++)
            {
                if (line[i].Y == line[i + 1].Y)
                {
                    for (var x = Math.Min(line[i].X, line[i+1].X); x <= Math.Max(line[i].X, line[i + 1].X); x++)
                        MapSet(x, line[i].Y, '#');
                }
                else
                {
                    for (var y = Math.Min(line[i].Y,line[i + 1].Y); y <= Math.Max(line[i].Y,line[i + 1].Y); y++)
                        MapSet(line[i].X, y, '#');
                }
            }
        }

        for (var x = 0; x < width; x++)
            map[x, height - 1] = '#';

        var sand = new Position(500 + 1 - min, 0);
        MapSet(500, 0, '+');
        
        Print(map, width, height);

        var unitsOfSand = 0;
        while(sand.Y < height - 1)
        {
            // Fall down
            if (sand.Y < height - 1 && map[sand.X, sand.Y + 1] == '.')
            {
                sand = sand with { Y = sand.Y + 1 };
                continue;
            }
            // Fall left
            if (sand.Y < height - 1 && sand.X > 0 && map[sand.X - 1, sand.Y + 1] == '.')
            {
                sand = sand with { X = sand.X - 1, Y = sand.Y + 1 };
                continue;
            }
            // Fall right
            if (sand.Y < height - 1 && sand.X < width - 1 && map[sand.X + 1, sand.Y + 1] == '.')
            {
                sand = sand with { X = sand.X + 1, Y = sand.Y + 1 };
                continue;
            }
            // Comes at rest
            map[sand.X, sand.Y] = 'o';
            unitsOfSand++;
            
            //Console.WriteLine($"After {unitsOfSand} units of sand:");
            //Print(map, width, height);
            //Console.WriteLine();
            
            // If comes at rest at origin stop
            if (sand.X == 500 + 1 - min && sand.Y == 0)
                break;
            sand = new Position(500 + 1 - min, 0);
        }
        Console.WriteLine($"Part 2: {unitsOfSand}");
    }

    private static void Print(char[,] map, int width, int height)
    {
        for (var j = 0; j < height; j++)
        {
            for (var i = 0; i < width; i++)
                Console.Write(map[i,j]);
            Console.Write('\n');
        }
    }

    record Position(int X, int Y);
}