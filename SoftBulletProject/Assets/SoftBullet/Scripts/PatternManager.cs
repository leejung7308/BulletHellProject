using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    [System.Serializable]
    public class BPActivation
    {
        public GameObject BP; //Bullet Pattern 
        public float activationTime; // 실행 시간
        public float deactivationTime; // 종료 시간
    }

    public List<BPActivation> activations;
    public bool isBoss = false;
    private bool pTrigger = false;

    private void Start()
    {
        if(isBoss)
        {
            StartCoroutine(ActivateBBPs());
        }
        else
        {
            StartCoroutine(ActivateBPs());
        }
        
    }

    IEnumerator ActivateBPs()
    {
        foreach (var activation in activations)
        {
            yield return new WaitForSeconds(activation.activationTime);
            activation.BP.SetActive(true);

            if (activation.deactivationTime > 0)
            {
                yield return new WaitForSeconds(activation.deactivationTime);
                activation.BP.SetActive(false);
            }
        }
    }

    IEnumerator ActivateBBPs()
    {
        int cur = 0;
        while(true)
        {
            activations[cur].BP.SetActive(true);
            yield return new WaitUntil(() => pTrigger);
            activations[cur].BP.SetActive(false);
            cur++;
            pTrigger = false;
        }


    }

    public void SkipPattern()
    {
        pTrigger = true;
    }
}
