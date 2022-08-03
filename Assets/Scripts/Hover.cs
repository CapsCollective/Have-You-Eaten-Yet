using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hover : MonoBehaviour
{
    [ShowNonSerializedField] private Vector3 centerPos;
    private float randomOffset;
    public bool centerToParent;

    [SerializeField] private float amplitude = 1;
    private const float Speed = 1.5f;
    
    
    private void Awake()
    {
        centerPos = transform.localPosition;
        randomOffset = Random.Range(0, Mathf.PI/2);
    }

    private void Update()
    {
		transform.localPosition = new Vector3(0, Mathf.Sin(randomOffset + Time.time * Speed) * amplitude, 0) + (centerToParent ? Vector3.zero : centerPos);
    }
}
