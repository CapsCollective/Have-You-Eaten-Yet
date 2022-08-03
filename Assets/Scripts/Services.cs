using UnityEngine;
using Yarn;
using Yarn.Unity;

public class Services : MonoBehaviour
{
    public static InMemoryVariableStorage DialogueStorage;
    public static DialogueStarter DialogueStarter;
    public static SceneManager Scene;

    [SerializeField] private InMemoryVariableStorage storage;

    private void Awake()
    {
        DialogueStorage = storage;
        DialogueStarter = FindObjectOfType<DialogueStarter>();
        Scene = FindObjectOfType<SceneManager>();
    }
}
