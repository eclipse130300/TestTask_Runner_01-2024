namespace CodeBase.Interactions
{
    /// <summary>
    /// Abstraction for flying entity
    /// </summary>
    public interface IFlyer
    {
        public void StartFlying();

        //actually we can separate, but whatever
        public void StopFlying();
    }
}