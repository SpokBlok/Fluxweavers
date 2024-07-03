using UnityEngine;

public abstract class PhaseBase
{
    public abstract void EnterState(PhaseHandler ph);
    public abstract void UpdateState(PhaseHandler ph);
}
