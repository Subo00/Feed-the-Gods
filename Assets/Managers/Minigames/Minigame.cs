using UnityEngine;

public class Minigame : MonoBehaviour, IMyUpdate
{
    protected MinigameManager manager;
    public virtual void StartMinigame(uint uintValue = 0, MinigameManager manager = null)
    {
        this.manager = manager;
        UpdateManager.Instance.AddUpdatable(this);
    }
    public virtual void EndMinigame()
    {
        UpdateManager.Instance.RemoveUpdatable(this);
    }
    public virtual void DisruptMinigame() { }

    protected virtual void OnUpdate() { }
    void IMyUpdate.MyUpdate()
    {
        OnUpdate();
    }
}
