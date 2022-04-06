using System.Numerics;

public class InputManager : SingletonBehaviour<InputManager>
{
    public GameControls Controller { get; private set; }

    public Vector2 TouchPosition { get; private set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Controller = new GameControls();
        Controller.Enable();
    }

    protected override void OnDelete()
    {
        Controller.Disable();
        base.OnDelete();
    }
}