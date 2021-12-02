namespace AdventOfCode.Utility
{
    public readonly unsafe struct WrappedPointer<TContext>
    {
        public readonly delegate*<ref TContext, void> Delegate;

        public WrappedPointer(delegate*<ref TContext, void> @delegate)
        {
            this.Delegate = @delegate;
        }
    }
}