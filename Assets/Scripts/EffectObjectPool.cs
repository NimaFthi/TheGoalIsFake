using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EffectObjectPool : MonoBehaviour
{
    private List<GameObject> pool;
    [SerializeField] private int poolSize;
    [SerializeField] private GameObject objectToPool;

    private void Awake()
    {
        pool = new List<GameObject>();
        for (var i=0;i<poolSize;i++)
        {
            var newGO=Instantiate(objectToPool,transform);
            pool.Add(newGO);
            newGO.SetActive(false);
        }
    }

    public GameObject GetObject()
    {
        for (var i=0;i<poolSize; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null;
    }
}
