using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;

    [SerializeField] string tagFilterName;

    public aiControls aiControls;


    private void OnTriggerEnter(Collider other)
    {
        CheckTagsEnter(other);
        KnockBack(other);
    }

    private void OnTriggerExit(Collider other)
    {
        CheckTagsExit(other);
    }

    private void CheckTagsEnter(Collider other)
    {
        if(!string.IsNullOrEmpty(tagFilterName) && other.gameObject.CompareTag(tagFilterName))
        {
            onTriggerEnter?.Invoke();
        }
    }

    private void CheckTagsExit(Collider other)
    {
        if(!string.IsNullOrEmpty(tagFilterName) && other.gameObject.CompareTag(tagFilterName))
        {
            onTriggerExit?.Invoke();
        }
    }

    public void KnockBack(Collider other)
    {
        aiControls.OnPunchHit(other);
    }
}
