using System;

public abstract class AIStateBase()
{
    protected MyAI ai;

    public AIStateBase(MyAI ai)
    {
        this.ai = ai;
    }

    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void Exit() { }


}
