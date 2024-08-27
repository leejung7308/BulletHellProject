using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class BossHpGauge : MonoBehaviour
{
    private float maxHp;
    private EnemyManagement em;

    public GameObject hpGauge;


    void Start()
    {
        em = gameObject.GetComponent<EnemyManagement>();
        maxHp = em.health;
    }

    void Update()
    {
        hpGauge.GetComponent<Disc>().AngRadiansEnd = 2 * Mathf.PI * (em.health / maxHp) + Mathf.PI * 0.5f;
    }
}
