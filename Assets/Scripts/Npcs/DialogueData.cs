using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacters
{
    public string characterName;
    public Sprite characterIcon;
}

[System.Serializable]
public class DialogueLines
{
    public DialogueCharacters character;
    
    [TextArea(2, 10)]
    public string line;
    public bool skip;
}

[CreateAssetMenu(menuName = "Dialogue/Dialogue Sequence")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLines> lines = new();
    public Vector3 adjustCharacterIcon ;
}
