using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Sequence")]
public class DialogueDataTeste : ScriptableObject
{
    [TextArea(2, 5)]
    public string[] lines;
}
