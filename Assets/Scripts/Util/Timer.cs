
using System;

[Serializable]
public struct Timer {
    public float MaxTime;
    public float CurrentTime;
    public bool Active;

    public bool Advance(float delta) {
        if (!Active) return false;
        CurrentTime += delta;   
        if (CurrentTime > MaxTime) {
            CurrentTime = MaxTime;
            return true;
        } else {
            return false;
        }
    }
}