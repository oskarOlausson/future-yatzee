using System;
using System.Collections.Generic;
using System.Linq;


public static class TypeFunctions
{
    public static string NameOf(Type type)
    {
        return type.ToString().Replace('_', ' ');
    }

    public static uint PointsFor(uint[] values, Type type)
    {
        if (type == Type.Chans)
        {
            return PointsForChans(values);
        }

        if (type == Type.Yatzee)
        {
            return PointsForYatzee(values);
        }

        var howManyOfEach = CountHowMany(values);

        if (type == Type.Ettor)
        {
            return PointsForDiceOfValue(1, howManyOfEach);
        }

        if (type == Type.Tvåor)
        {
            return PointsForDiceOfValue(2, howManyOfEach);
        }

        if (type == Type.Treor)
        {
            return PointsForDiceOfValue(3, howManyOfEach);
        }

        if (type == Type.Fyror)
        {
            return PointsForDiceOfValue(4, howManyOfEach);
        }

        if (type == Type.Femmor)
        {
            return PointsForDiceOfValue(5, howManyOfEach);
        }

        if (type == Type.Sexor)
        {
            return PointsForDiceOfValue(6, howManyOfEach);
        }

        if (type == Type.Par)
        {
            return HighestWithNMatching(2, howManyOfEach);
        }

        if (type == Type.Tvåpar)
        {
            return PointsForTvåPar(howManyOfEach);
        }

        if (type == Type.Triss)
        {
            return HighestWithNMatching(3, howManyOfEach);
        }

        if (type == Type.Fyrtal)
        {
            return HighestWithNMatching(4, howManyOfEach);
        }

        if (type == Type.Liten_stege)
        {
            return PointsForLitenStege(howManyOfEach);
        }

        if (type == Type.Stor_stege)
        {
            return PointsForStorStege(howManyOfEach);
        }

        if (type == Type.Kåk)
        {
            return PointsForKåk(howManyOfEach);
        }

        throw new ArgumentOutOfRangeException();
    }

    private static uint PointsForYatzee(uint[] values)
    {
        var target = values.First();
        return (values.All(v => v == target)) ? (uint) 50 : 0;
    }

    private static uint PointsForChans(IEnumerable<uint> values)
    {
        return values.Aggregate((uint) 0, (a, b) => a + b);
    }

    private static uint PointsForLitenStege(IDictionary<uint, uint> howManyOfEach)
    {
        for (uint i = 1; i <= 5; i++)
        {
            if (howManyOfEach[i] != 1) return 0;
        }

        return 15;
    }

    private static uint PointsForStorStege(IDictionary<uint, uint> howManyOfEach)
    {
        for (uint i = 2; i <= 6; i++)
        {
            if (howManyOfEach[i] != 1) return 0;
        }

        return 20;
    }

    private static uint PointsForTvåPar(IDictionary<uint, uint> howManyOfEach)
    {
        uint sum = 0;

        for (uint i = 6; i >= 1; i--)
        {
            if (howManyOfEach[i] >= 2)
            {
                if (sum == 0) sum = i * 2;
                else return i * 2 + sum;
            }
        }

        return 0;
    }

    private static uint PointsForKåk(IDictionary<uint, uint> howManyOfEach)
    {
        var parFound = false;
        var trissFound = false;
        uint sum = 0;

        for (uint i = 6; i >= 1 && (!trissFound || !parFound); i--)
        {
            if (!trissFound && howManyOfEach[i] == 3)
            {
                sum += i * 3;
                trissFound = true;
            }
            else if (!parFound && howManyOfEach[i] == 2)
            {
                sum += i * 2;
                parFound = true;
            }
        }

        return (trissFound && parFound) ? sum : 0;
    }

    private static Dictionary<uint, uint> CountHowMany(IEnumerable<uint> howManyOfEach)
    {
        var counter = new Dictionary<uint, uint>();

        foreach (var dieNumber in howManyOfEach)
        {
            if (!counter.ContainsKey(dieNumber)) counter.Add(dieNumber, 0);
            counter[dieNumber]++;
        }

        for (uint i = 1; i <= 6; i++)
        {
            if (!counter.ContainsKey(i)) counter.Add(i, 0);
        }

        return counter;
    }


    private static uint HighestWithNMatching(uint n, IDictionary<uint, uint> howManyOfEach)
    {
        for (uint i = 6; i >= 1; i--)
        {
            if (howManyOfEach[i] >= n) return i * n;
        }

        return 0;
    }

    private static uint PointsForDiceOfValue(uint target, IDictionary<uint, uint> howManyOfEach)
    {
        return target * howManyOfEach[target];
    }
}