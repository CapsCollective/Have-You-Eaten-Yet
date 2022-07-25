using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueStarter : MonoBehaviour
{
    const string DUMPLING_START = "Dumpling_1";

    // This will need changing
    const string RESTAURANT_START = "Start";

    [SerializeField] private DialogueRunner Runner;

    // Need to fill this with restaurant nodes
    // only works with dumpling nodes right now
    private List<string> debugNodes = new List<string>
    {
        "Dumpling_1",
        "Dumpling_2",
        "Night_2_Shift"
    };

    private void Awake()
    {
        SceneManager.OnDumplingsSceneLoad += StartDumplingsDialogue;
        SceneManager.OnRestaurantSceneLoad += StartRestaurantDialogue;
    }

    private void StartDumplingsDialogue()
    {
        Runner.StartDialogue(debugNodes[0]);
    }

    private void StartRestaurantDialogue()
    {
        Runner.StartDialogue(debugNodes[0]);
    }

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (Runner.IsDialogueRunning) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Runner.StartDialogue(debugNodes[0]);
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            Runner.StartDialogue(debugNodes[1]);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            Runner.StartDialogue(debugNodes[2]);
    }
#endif
}
