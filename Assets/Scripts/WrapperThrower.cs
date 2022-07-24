using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class WrapperThrower : MonoBehaviour
{
    public static int SpawnedWrappers;

    [Header("Prefabs")]
    [SerializeField] private GameObject wrapperPrefab;
    [SerializeField] private GameObject faultyWrapperPrefab;
    
    [Header("Config")]
    [SerializeField] private int maxSpawnCount = 3;
    [Range(0, 1)] [SerializeField] private float faultyThrowChance;
    [SerializeField] private float minDelay, maxDelay;
    
    private int _index;
    private float _timer, _delay;
    
    private readonly List<Vector3> _positions = new List<Vector3> {
        new Vector3(-20, -17, 0),
        new Vector3(-33, -32, 0),
        new Vector3(-9, -32, 0),
        new Vector3(-4, -1, 0),
        new Vector3(16, -6, 0),
        new Vector3(3, -18, 0),
        new Vector3(13, -32, 0),
        new Vector3(-31, -4, 0),
        new Vector3(-23,11,0)
    };

    [Button] private void Throw()
    {
        bool throwFaulty = Random.Range(0f, 1f) < faultyThrowChance;
        Transform wrapper = Instantiate(throwFaulty ? faultyWrapperPrefab : wrapperPrefab, transform).transform;
        Vector3 targetPos = _positions[_index % _positions.Count];
        wrapper.DOMove(targetPos, 1f);
        wrapper.eulerAngles = new Vector3(0, 0, 180);
        wrapper.DORotate(new Vector3(0, 0, 360), 1f);
        _index += Random.Range(1, 3);
        if (!throwFaulty) SpawnedWrappers++;
    }

    private void Update()
    {
        if (SpawnedWrappers >= maxSpawnCount) return;
        _timer += Time.deltaTime;
        if (_timer < _delay) return;
        _timer = 0;
        _delay = Random.Range(minDelay, maxDelay);
        Throw();
    }
}
