using MJ.EditorTools;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    [Header("Unity String")]
    public string normalString;
    
    [Space(20)][Header("Persian Inspector Tool")]
    
    // Basic usage
    [PersianText]
    public string characterName;

    // Advanced usage with line limits
    [PersianText(minLines: 3, maxLines: 10, fontSize:15)]
    public string storyDescription;
}