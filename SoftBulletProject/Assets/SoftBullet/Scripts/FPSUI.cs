using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSUI : MonoBehaviour
{
    public GameObject fpsGui;
    private void Start()
    {
        StartCoroutine(work());
    }

    IEnumerator work()
    {
        yield return new WaitForSeconds(2f);
        fpsGui.SetActive(true);
    }
}
