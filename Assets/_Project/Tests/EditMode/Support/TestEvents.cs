namespace MaintGames.Bush.Tests.EditMode.Support
{
    public readonly struct SomethingHappened
    {
        public SomethingHappened(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public readonly struct SomethingElseHappened
    {
    }
}
