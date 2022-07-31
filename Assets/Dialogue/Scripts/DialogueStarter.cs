using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueStarter : MonoBehaviour
{
    const string DUMPLING_START = "Dumpling_1";

    // This will need changing
    const string RESTAURANT_START = "Start";

    [SerializeField] private DialogueRunner dumplingDialogueRunner;
    [SerializeField] private DialogueRunner restauarantDialogueRunner;

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
        StartTutorialDialogue(1);
    }

    private void StartRestaurantDialogue()
    {
        restauarantDialogueRunner.StartDialogue(debugNodes[0]);
    }

    public void StartTutorialDialogue(int part)
    {
        dumplingDialogueRunner.StartDialogue($"Restaurant_tutorial_scene_{part}");
    }

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (dumplingDialogueRunner.IsDialogueRunning) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            dumplingDialogueRunner.StartDialogue(debugNodes[0]);
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            dumplingDialogueRunner.StartDialogue(debugNodes[1]);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            dumplingDialogueRunner.StartDialogue(debugNodes[2]);
    }
#endif
}
