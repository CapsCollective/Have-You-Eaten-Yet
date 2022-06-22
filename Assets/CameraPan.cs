using System;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField] private float leftBound, rightBound, screenPercentage, speed;
    [SerializeField] private Transform leftArrow, rightArrow;

    private const float HoverScale = 1.1f;
    
    // Update is called once per frame
    void Update()
    {
        var lPos = leftArrow.position;
        var rPos = rightArrow.position;
        leftArrow.position = new Vector3(lPos.x, Mathf.Sin(Time.time), lPos.z);
        rightArrow.position = new Vector3(rPos.x, Mathf.Sin(Time.time), rPos.z);

        float mousePos = Input.mousePosition.x / Screen.width;
        float xPos = transform.position.x;
        
        if (mousePos < screenPercentage && xPos > leftBound)
        {
            transform.position = new Vector3(Mathf.Max(leftBound, xPos - (speed * Time.deltaTime)), 0, -10);
            leftArrow.localScale = Vector3.one * HoverScale;
        }
        else
        {
            leftArrow.localScale = Vector3.one;
        }
        if (mousePos > 1 - screenPercentage && xPos < rightBound)
        {
            transform.position = new Vector3(Mathf.Max(leftBound, xPos + (speed * Time.deltaTime)), 0, -10);
            rightArrow.localScale = Vector3.one * HoverScale;
        }
        else
        {
            rightArrow.localScale = Vector3.one;
        }
        leftArrow.gameObject.SetActive(xPos > leftBound + 0.5f);
        rightArrow.gameObject.SetActive(xPos < rightBound - 0.5f);
    }
}
