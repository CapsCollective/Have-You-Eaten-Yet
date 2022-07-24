using UnityEngine;
using Yarn.Unity;

public class Services : MonoBehaviour
{
    public static InMemoryVariableStorage Dialogue;
    private void OnDrawGizmos()
    {
        Dialogue = FindObjectOfType<InMemoryVariableStorage>();
    }
}
