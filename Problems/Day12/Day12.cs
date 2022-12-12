namespace AdventOfCode2022.Problems.Day12;

class Day12 : ICodingProblem
{
    public void Run()
    {
        var rows = File.ReadAllLines("Problems\\Day12\\input.txt");
        var height = rows.Length;
        var width = rows[0].Length;
        var map = new Node[width, height];
        
        // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
        
        var unvisitedNodes = new List<Node>();
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var node = new Node
                {
                    Symbol = rows[y][x] == 'S' ? 'a' : (rows[y][x] == 'E' ? 'z' : rows[y][x]), 
                    Distance = rows[y][x] == 'E' ? 0 : int.MaxValue, 
                    Visited = false, 
                    X = x, 
                    Y = y
                };
                map[x, y] = node;
                unvisitedNodes.Add(node);
            }
        }

        do
        {
            var current = unvisitedNodes.MinBy(x => x.Distance);
            var x = current!.X;
            var y = current.Y;
            if (x > 0 && map[x - 1, y].Symbol - current.Symbol >= -1)
                map[x - 1, y].Distance = Math.Min(map[x - 1, y].Distance, current.Distance + 1);
            if (x < width - 1 && map[x + 1, y].Symbol - current.Symbol >= -1)
                map[x + 1, y].Distance = Math.Min(map[x + 1, y].Distance, current.Distance + 1);
            if (y > 0 && map[x, y - 1].Symbol - current.Symbol >= -1)
                map[x, y - 1].Distance = Math.Min(map[x, y - 1].Distance, current.Distance + 1);
            if (y < height - 1 && map[x, y + 1].Symbol - current.Symbol >= -1)
                map[x, y + 1].Distance = Math.Min(map[x, y + 1].Distance, current.Distance + 1);
            current.Visited = true;
            unvisitedNodes.Remove(current);
        } while (unvisitedNodes.Any() &&
                 !unvisitedNodes.Where(x => x.Symbol == 'a').All(x => x.Visited) &&
                 unvisitedNodes.Min(x => x.Distance) != int.MaxValue);

        Node? minNode = null;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (map[x, y].Symbol == 'a' && (minNode is null || minNode.Distance > map[x, y].Distance))
                    minNode = map[x, y];
            }
        }

        Console.WriteLine($"Part 2: {minNode!.Distance}");
    }

    class Node
    {
        public char Symbol { get; init; } 
        public int Distance { get; set; }
        public bool Visited { get; set; }
        public int X { get; init; } 
        public int Y { get; init; }
    }
}