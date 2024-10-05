using UnityEngine;

public abstract class State
{
    public readonly string name;

    protected State(string name)
    {
        this.name = name;
    }


    public virtual void Enter()
    {
        Debug.Log($"Entrou no estado: {name}");
    }

    public virtual void Exit()
    {
        Debug.Log($"Saiu do estado: {name}");
    }

    public virtual void Update() { }
    public virtual void LateUpdate() { }
    public virtual void FixedUpdate() { }
}
