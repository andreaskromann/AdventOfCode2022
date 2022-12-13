namespace AdventOfCode2022.Problems.Day13;

class Day13 : ICodingProblem
{
    public void Run()
    {
        var pairs = File.ReadAllText("Problems\\Day13\\input.txt").Split("\n\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var part1 = new List<int>();
        for (var i = 0; i < pairs.Length; i++)
        {
            var pair = pairs[i];
            var packets = pair.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var packet1 = ParsePacket(packets[0]);
            var packet2 = ParsePacket(packets[1]);
            if (ComparePackets(packet1, packet2) == -1)
                part1.Add(i + 1);
        }
        Console.WriteLine(string.Join(",", part1));
        Console.WriteLine($"Part 1: {part1.Sum()}");
    }

    private int ComparePackets(PacketData left, PacketData right)
    {
        if (left is IntegerData leftInt && right is IntegerData rightInt)
        {
            if (leftInt.Value == rightInt.Value)
                return 0;
            if (leftInt.Value < rightInt.Value)
                return -1;
            return 1;
        }

        if (left is ListData leftList && right is ListData rightList)
        {
            for (var i = 0; i < leftList.Values.Count; i++)
            {
                if (rightList.Values.Count <= i)
                    return 1;
                
                var currentResult = ComparePackets(leftList.Values[i], rightList.Values[i]);
                if (currentResult != 0)
                    return currentResult;
            }

            if (leftList.Values.Count < rightList.Values.Count)
                return -1;
            return 0;
        }

        if (left is ListData list && right is IntegerData integer)
        {
            var newList = new ListData();
            newList.Values.Add(integer);
            return ComparePackets(list, newList);
        }
        
        if (left is IntegerData integer2 && right is ListData list2)
        {
            var newList = new ListData();
            newList.Values.Add(integer2);
            return ComparePackets(newList, list2);
        }

        return 0;
    }

    private ListData ParsePacket(string s)
    {
        var packet = new ListData();
        if (s.Length == 2)
            return packet;

        for (var i = 1; i < s.Length - 1;)
        {
            if (s[i] == ',')
                i++;
            else if (s[i] == '[')
            {
                var depth = 0;
                var j = i + 1;
                while (depth != 0 || s[j] != ']')
                {
                    if (s[j] == '[')
                        depth++;
                    if (s[j] == ']')
                        depth--;
                    j++;
                }
                packet.Values.Add(ParsePacket(s.Substring(i, j - i + 1)));
                i += j - i + 1;
            }
            else
            {
                var j = i + 1;
                while (j < s.Length && s[j] != ',')
                    j++;
                var value = j < s.Length ? int.Parse(s.Substring(i, j - i)) : int.Parse(s.Substring(i, s.Length - i - 1));
                packet.Values.Add(new IntegerData { Value = value });
                i += value.ToString().Length;
            }
        }

        return packet;
    }

    abstract class PacketData {}
    class IntegerData : PacketData
    {
        public int Value { get; init; }
    }
    class ListData : PacketData
    {
        public List<PacketData> Values { get; } = new();
    }
}