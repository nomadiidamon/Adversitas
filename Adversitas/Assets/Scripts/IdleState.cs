using System;

[System.Serializable]
public class IdleState : AIStateBase
{
    public IdleState(AIBrain ai) : base(ai) { }

    public override void Enter()
    {
        ai.animator.CrossFade("Standing_Idle1", 0.5f);
    }

    public override void Update()
    {

    }

    public override void FixedUpdate() 
    { 
    
    }

    public override void Exit()
    {
        
    }
}