using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class WrapperThrower : MonoBehaviour
{
    public static int SpawnedWrappers;
    [SerializeField] private int maxSpawnCount = 3;
    [SerializeField] private GameObject wrapperPrefab;

    private int _index;
    private float _timer;
    
    private readonly List<Vector3> _positions = new List<Vector3> {
        new Vector3(-20, -17, 0),
        new Vector3(-33, -32, 0),
        new Vector3(-9, -32, 0),
        new Vector3(-4, -1, 0),
        new Vector3(16, -6, 0),
        new Vector3(3, -18, 0),
        new Vector3(13, -32, 0),
        new Vector3(-31, -4, 0),
    };

    [Button()]
    private void Throw()
    {
        if (SpawnedWrappers >= maxSpawnCount) return;
        Wrapper wrapper = Instantiate(wrapperPrefab, transform).GetComponent<Wrapper>();
        wrapper.Throw(_positions[_index % _positions.Count]);
        _index += Random.Range(1, 3);
        SpawnedWrappers++;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 2)
        {
            _timer = 0;
            Throw();
        }
    }
}
