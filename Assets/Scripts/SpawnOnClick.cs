using UnityEngine;
using Yarn.Unity;

public class SpawnOnClick : MonoBehaviour
{
    public GameObject item;

    private static bool TutorialFlag, Active;
    
    void OnMouseDown()
    {
        if(!Active) return;
        
        GameObject instance = Instantiate(item, transform.position, item.transform.rotation);
        instance.GetComponent<DragAndDrop>()?.Select(); // Init click if draggable component

        if (TutorialFlag) return;
        TutorialFlag = true;
        Services.DialogueStarter.StartTutorialDialogue(1);
    }

    [YarnCommand("SetActive")]
    private void SetActive(bool active)
    {
        Active = active;
    }
}
