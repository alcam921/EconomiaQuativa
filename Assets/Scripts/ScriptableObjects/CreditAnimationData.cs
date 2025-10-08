using UnityEngine;

[CreateAssetMenu(fileName = "CreditAnimationData", menuName = "ScriptableObjects/CreditAnimationData", order = 1)]
public class CreditAnimationData : ScriptableObject
{
    #region Fields
    [Header("Anim Sprites")]
    public Sprite[] idleImages;
    public Sprite[] removingMaskImages;
    [Header("Student Infos")]
    public string studentName;
    public string studentRole;
    public bool hasDesc = true;
    [TextArea(2, 5)] public string studentDescription;
    public Sprite studentImage;
    public Sprite cardImage;
    public string linkedinURL;
    public string gitURL;
    public string instaURL;
    #endregion

}
