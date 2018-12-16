using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class Turn
{   
    private readonly Die[] _firstRow;
    private readonly PossibleValues[] _willBe;
        
    public readonly UnityEvent OnChange = new UnityEvent();

    private class PossibleValues
    {
        private Random _random;
        public readonly int Seed;

        public PossibleValues(long seed)
        {
            while (seed > int.MaxValue)
            {
                seed -= int.MaxValue;
            }

            Seed = (int) seed;
            Restart();
        }

        public void Restart()
        {
            _random = new Random(Seed);
        }

        public uint ClaimValue()
        {
            return (uint) _random.Next(1, 7);
        }
    }

    public Turn()
    {
        _willBe = new PossibleValues[6];

        for (var i = 0; i < _willBe.Length; i++)
        {
            _willBe[i] = new PossibleValues(UniqueSeed());
        }

        var random = new Random(UniqueSeed());
        _firstRow = new Die[5];
        for (uint i = 0; i < 5; i++)
        {
            var die = new Die {Number = (uint) random.Next(1, 7)};
            _firstRow[i] = die;
        }

        UpdateDice();
    }

    private static int _numberOfTimesRun;
    private static int UniqueSeed()
    {
        _numberOfTimesRun++;
            
        var seed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        var top = int.MaxValue - _numberOfTimesRun;
        while (seed > top)
        {
            seed -= top;
        }

        return ((int) seed) + top;
    }

    public void UpdateDice()
    {
        foreach (var v in _willBe)
        {
            v.Restart();
        }

        //breath first search for dies 
        var toProcess = new Queue<Die>(_firstRow);
        while (toProcess.Count > 0)
        {
            var die = toProcess.Dequeue();
            if (die.NextRoll != null)
            {
                die.NextRoll.Number = _willBe[die.Number - 1].ClaimValue();
                toProcess.Enqueue(die.NextRoll);
            }
        }

        OnChange.Invoke();
    }

    public uint[] GetValues()
    {
        return _firstRow
            .Select(dice =>
            {
                while (dice.NextRoll != null) dice = dice.NextRoll;
                return dice.Number;
            }).ToArray();
    }
    
    public Die[] GetTopRow()
    {
        return _firstRow;
    }

    public int[] GetSeeds()
    {
        return _willBe.Select(p => p.Seed).ToArray();
    }

    public void RestoreFromSavedInfo(SaveInfo.TurnInfo dataTurnInfo)
    {
        for (var i = 0; i < dataTurnInfo.FirstRow.Length; i++)
        {
            _firstRow[i].Number = dataTurnInfo.FirstRow[i];
        }

        for (var i = 0; i < dataTurnInfo.Seeds.Length; i++)
        {
            _willBe[i] = new PossibleValues(dataTurnInfo.Seeds[i]);
        }

        for (var i = 0; i < dataTurnInfo.NumberOfReRolls.Length; i++)
        {
            _firstRow[i].RestoreRolls(dataTurnInfo.NumberOfReRolls[i]);
        }
    }
}