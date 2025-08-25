using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IManager
{
    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private MoneyManager moneyManager;


    private static Dictionary<Type, IManager> managers = new Dictionary<Type, IManager>();

    private static event Action onInitializedCallback;
    private static bool isInitialized;

    private void Awake()
    {
        ValidationUtility.ValidateReference(dialogueManager, nameof(dialogueManager));
        ValidationUtility.ValidateReference(uiManager, nameof(uiManager));
        ValidationUtility.ValidateReference(mouseManager, nameof(mouseManager));
        ValidationUtility.ValidateReference(moneyManager, nameof(moneyManager));

        managers.Clear();
        managers.Add(typeof(GameManager), this);
        managers.Add(typeof(DialogueManager), dialogueManager);
        managers.Add(typeof(UIManager), uiManager);
        managers.Add(typeof(MouseManager), mouseManager);
        managers.Add(typeof(MoneyManager), moneyManager);

        isInitialized = true;
        onInitializedCallback?.Invoke();
        onInitializedCallback = null;
    }

    public static T GetManager<T>() where T : IManager
    {
        return (T)managers[typeof(T)];
    }

    public static void ExecuteWhenInitialized(Action callback)
    {
        if (isInitialized)
        {
            callback?.Invoke();
        } else
        {
            onInitializedCallback += callback;
        }
    }
}
