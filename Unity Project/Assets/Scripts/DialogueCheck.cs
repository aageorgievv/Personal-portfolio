using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    [TextArea(5, 15)]
    public string dialogueText;
    public List<DialogueResponse> responses;

    internal bool IsLastNode()
    {
        return string.IsNullOrEmpty(dialogueText) && responses.Count <= 0;
    }
}