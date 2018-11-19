namespace Scripts
{
    public class Die
    {
        public Die NextRoll;
        //0 means no number
        public uint Number;

        public void Toggle()
        {
            NextRoll = (NextRoll == null) ? new Die() : null;
        }
    }
}