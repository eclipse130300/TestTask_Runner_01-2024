namespace CodeBase.Infrastructure.States
{
    public class GameplayPausedState : IPayloadedState<bool>
    {
        public GameplayPausedState(GameStateMachine stateMachine)
        {
        }

        public void Enter(bool payload)
        {
            
        }

        public void Exit()
        {
        
        }

        public void Enter()
        {
        }
    }
}