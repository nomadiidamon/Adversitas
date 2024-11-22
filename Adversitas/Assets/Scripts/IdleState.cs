using System;

public class IdleState : AIStateBase
{
    public IdleState(MyAI ai) : base(ai) { }

    public override void Enter()
    {
        ai.animator.CrossFade("Standing_Idle1", 0.5f);
    }

    public override void Update()
    {

    }

    public virtual void FixedUpdate() 
    { 
    
    }

    public override void Exit()
    {
        
    }
}