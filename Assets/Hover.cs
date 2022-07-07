using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hover : MonoBehaviour
{
	private Vector3 centerPos;
    private float randomOffset;
    
    [SerializeField] private float amplitude = 1, speed = 1;
    
    
    // Start is called before the first frame update
    void Start()
    {
        centerPos = transform.position;
        randomOffset = Random.Range(0, 2 * Mathf.PI);
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = centerPos + new Vector3(0, Mathf.Sin(randomOffset + Time.time * speed) * amplitude, 0);
    }
}
