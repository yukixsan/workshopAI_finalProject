using UnityEngine;
using System;

public abstract class State : MonoBehaviour
{
    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void Exit() { }
}
