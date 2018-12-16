
using System;

[Serializable]
public class SaveInfo
{
    [Serializable]
    public struct TurnInfo
    {
        public uint[] FirstRow;
        public int[] Seeds;
        public uint[] NumberOfReRolls;
        public int IndexOfConnectedTo;
    }

    public TurnInfo[] TurnInfos;
    public int SelectedIndex;

    public static SaveInfo Default()
    {
        return new SaveInfo {TurnInfos = new TurnInfo[0]};
    }
}