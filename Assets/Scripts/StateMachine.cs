//General state interface
//States are created as classes WITHIN a script (i.e. classes inside a controller class)
public interface IState
{
    //Called at the start of the state
    void Enter();
    //Called on update on active state
    void Execute();
    //Called when leaving the state
    void Exit();
}

//Interface used to handle requests for state changing
public interface IStateManaged {
    //Functions like a flow chart of which states can transfer to others
    void RequestState(IState requestedState);
}

//State machine class
public class StateMachine
{
    //Reference to current state that the machine is running
    public IState currentState;

    //Processes states changing once request has been approved
    public void ChangeState(IState newState) {
        //Calls exit function on current state
        currentState?.Exit();

        //Sets state to the new state
        currentState = newState;

        //Calls enter function on new state
        currentState.Enter();
    }

    //Needs to be called in scripts that utilize the state machine behaviour
    public void Update() {
        //Executes any functions of active state
        currentState?.Execute();
    }
}
