using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
[System.Serializable]
public class EnemyPattern
{
    public float nextPatternDelay;
    public GameObject patternObject;
}
public class StageManager : MonoBehaviour
{
    public List<EnemyPattern> enemyPatterns;
    void Start()
    {
        StartCoroutine(EnemyPatternEnable());
    }
    IEnumerator EnemyPatternEnable()
    {
        for(int i=0; i < enemyPatterns.Count; i++)
        {
            yield return new WaitForSeconds(enemyPatterns[i].nextPatternDelay);
            enemyPatterns[i].patternObject.SetActive(true);
        }
    }
}
