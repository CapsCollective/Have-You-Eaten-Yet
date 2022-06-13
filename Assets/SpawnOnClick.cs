using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnClick : MonoBehaviour
{
    public GameObject item;
    
    void OnMouseDown()
    {
        GameObject instance = Instantiate(item, transform.position, item.transform.rotation);
        //instance.GetComponent<DragAndDrop>()?.OnMouseDown(); // Init click if draggable component
    }
}
