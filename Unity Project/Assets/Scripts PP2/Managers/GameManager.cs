using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IManager
{
    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private UIManager uiManager;

    private static Dictionary<Type, IManager> managers = new Dictionary<Type, IManager>();

    private void Awake()
    {
        ValidationUtility.ValidateReference(dialogueManager, nameof(dialogueManager));
        ValidationUtility.ValidateReference(uiManager, nameof(uiManager));

        managers.Clear();
        managers.Add(typeof(GameManager), this);
        managers.Add(typeof(DialogueManager), dialogueManager);
        managers.Add(typeof(UIManager), uiManager);
    }

    public static T GetManager<T>() where T : IManager
    {
        return (T)managers[typeof(T)];
    }
}
