using UnityEngine;
using UnityEngine.Assertions;

public class Die
{
    public Die NextRoll;
    //0 means no number, do not know if this is used anymore, ugh comments, indeed
    public uint Number;

    public void Toggle()
    {
        NextRoll = (NextRoll == null) ? new Die() : null;
    }

    public void RestoreRolls(uint numberOfReRolls)
    {
        if (numberOfReRolls == 0)
        {
            NextRoll = null;
        }
        else if (numberOfReRolls == 1)
        {
            NextRoll = new Die {NextRoll = null};
        }
        else if (numberOfReRolls == 2)
        {
            NextRoll = new Die {NextRoll = new Die()};
        }
        else
        {
            Debug.Log("Warning: This is too many re-rolls");
            NextRoll = new Die {NextRoll = new Die()};
        }
    }

   //recursion, yeah
    public uint AmountOfReRolls()
    {
        if (NextRoll == null) return 0;
        return 1 + NextRoll.AmountOfReRolls();
    }
}