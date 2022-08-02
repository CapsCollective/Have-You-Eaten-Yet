using UnityEngine;
using Yarn.Unity;

public class Services : MonoBehaviour
{
    public static InMemoryVariableStorage Dialogue;
    public static SceneManager Scene;

    private void Awake()
    {
        Dialogue = FindObjectOfType<InMemoryVariableStorage>();
        Scene = FindObjectOfType<SceneManager>();
    }
}
