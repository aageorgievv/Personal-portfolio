using System;
using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    public event Action OnQuestCompleted;

    public bool IsCompleted { get; private set; }

    protected void CompleteQuest()
    {
        IsCompleted = true;
        OnQuestCompleted?.Invoke();
    }
}
