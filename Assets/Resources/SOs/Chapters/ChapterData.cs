using UnityEngine;

[CreateAssetMenu(fileName = "ChapterData", menuName = "ScriptableObjects/ChapterData", order = 1)]
public class ChapterData : ScriptableObject
{
    [TextArea(10, 20)]
    public string chapterText;
}
