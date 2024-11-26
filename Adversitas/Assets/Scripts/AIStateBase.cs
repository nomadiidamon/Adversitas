using System;
using UnityEngine;

[System.Serializable]
[SerializeField]
public class AIStateBase : ScriptableObject
{
    [SerializeField] protected AIBrain ai;

    public AIStateBase(AIBrain _ai)
    {
        ai = _ai;
    }

    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void Exit() { }


}
