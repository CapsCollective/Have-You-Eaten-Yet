using UnityEngine;
using Random = UnityEngine.Random;

public class Hover : MonoBehaviour
{
	private Vector3 centerPos;
    private float randomOffset;

    [SerializeField] private float amplitude = 1;
    private const float Speed = 1.5f;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        centerPos = transform.position;
        randomOffset = Random.Range(0, Mathf.PI/2);
    }

    // Update is called once per frame
    private void Update()
    {
		transform.position = centerPos + new Vector3(0, Mathf.Sin(randomOffset + Time.time * Speed) * amplitude, 0);
    }
}
