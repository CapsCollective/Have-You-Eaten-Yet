using UnityEngine;

public class Hover : MonoBehaviour
{
	private Vector3 centerPos;
    [SerializeField] private float amplitude = 1, speed = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        centerPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = centerPos + new Vector3(0, Mathf.Sin(Time.time * speed) * amplitude, 0);
    }
}
