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

    private Dictionary<int, string> dumplingDialogueStarts = new Dictionary<int, string>
    {
        { 1, "Restaurant_tutorial_scene_0" },
        { 2, "Restaurant_Busy_Scene" },
        { 3, "Restaurant_quiet_scene" },
    };

    private Dictionary<int, List<string>> restaurantDialogueStarts = new Dictionary<int, List<string>>
    {
        { 1, new List<string>{ "Night_1_Bar_Conversation", "Night_1_Table_Conversation" } },
        { 2, new List<string>{ "Night_2_Complaining_Conversation", "Night_2_Table_Conversation", "Night_2_Table_Conversation_2" } },
        { 3, new List<string>{ "Night_3_Bar_Conversation", "Night_3_Table_Conversation" } },
    };

    private void Awake()
    {
        SceneManager.OnDumplingsSceneLoad += StartDumplingsDialogue;
        SceneManager.OnRestaurantSceneLoad += StartRestaurantDialogue;
    }

    private void StartDumplingsDialogue(int night)
    {
        dumplingDialogueRunner.StartDialogue(dumplingDialogueStarts[night]);
    }

    private void StartRestaurantDialogue(int night)
    {
        //var dialogues = restaurantDialogueStarts[night];
        //foreach (string d in dialogues) 
        //{
        //    DialogueRunner newDr = Instantiate(restauarantDialogueRunner, restauarantDialogueRunner.transform.parent);
        //    newDr.StartDialogue(d);
        //}
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

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    dumplingDialogueRunner.StartDialogue(debugNodes[0]);
        //else if(Input.GetKeyDown(KeyCode.Alpha2))
        //    dumplingDialogueRunner.StartDialogue(debugNodes[1]);
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //    dumplingDialogueRunner.StartDialogue(debugNodes[2]);
    }
#endif
}
