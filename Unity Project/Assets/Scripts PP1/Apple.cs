using System;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public event Action OnDestroyed;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }

}
