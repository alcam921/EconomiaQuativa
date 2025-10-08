using UnityEngine;


[CreateAssetMenu(fileName = "ProximityPoint", menuName = "ScriptableObjects/ProximityPoint", order = 1)]
public class ProximityPoint : ScriptableObject
{
    public Sprite img; 
    public string pointName; 
    public string pointText; 
    public string pointScene; 
    public int pointIndex;
    public bool estacao;
}
