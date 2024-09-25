using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StateInterface
{
    
    public void UpdateState() { }
    public void FixedUpdateState() { }
    public void Enter() { }
    public void Exit() { }
}
