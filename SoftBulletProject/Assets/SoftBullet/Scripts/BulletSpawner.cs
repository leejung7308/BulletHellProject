using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class temp
{
    public int a = 0;
    public int b = 0;
}

public class BulletSpawner : MonoBehaviour
{
    public List<PatternBase> patterns;

    public List<temp> tmp;

    private void OnEnable()
    {
        foreach(PatternInterface pattern in patterns)
        {
            StartCoroutine(pattern.BulletSet());
        }
    }

}
