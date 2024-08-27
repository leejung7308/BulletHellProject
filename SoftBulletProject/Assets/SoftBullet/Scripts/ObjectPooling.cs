using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> pool;
    [HideInInspector]
    public List<int> indexPool;
    public GameObject bulletPrefab;
    [HideInInspector]
    public int removedIndex=0;
    void Start()
    {
        for (int i = 0; i < 4000; i++)
        {
            GameObject g = Instantiate(bulletPrefab);
            pool.Add(g);
            Disable(i);
        }
    }
    public void CreateBullet()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject g = Instantiate(bulletPrefab);
            pool.Add(g);
            Disable(indexPool.Count + i);
        }
    }
    public void Disable(int index)
    {
        pool[index].SetActive(false);
        indexPool.Add(index);
    }
    public GameObject Enable()
    {
        removedIndex = indexPool[indexPool.Count-1];
        GameObject pulledObject = pool[removedIndex];
        pulledObject.gameObject.GetComponent<BulletDestroyer>().index = removedIndex;
        indexPool.RemoveAt(indexPool.Count - 1);
        pulledObject.SetActive(true);
        return pulledObject;
    }
}
