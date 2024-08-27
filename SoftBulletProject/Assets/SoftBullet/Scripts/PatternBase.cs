using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternBase : PatternInterface
{
    private GameObject player;
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(BulletSet());
    }
    public override IEnumerator BulletSet()
    {
        yield return new WaitForSeconds(startDelay);
        float angle;
        for (int wave = 0; wave < waveCount; wave++)
        {
            for (int i = 0; i < burstCount; i++)
            {
                for (int j = 0; j < bulletsPerShot; j++)
                {
                    angle = baseAngle + (j * shotAngle + i * rotateAngle);
                    if (player.GetComponent<PlayerController>().isPooling) BulletSpawn(angle);
                    else NoPoolingBulletSpawn(angle);
                }
                yield return new WaitForSeconds(shotDelay);
            }
            yield return new WaitForSeconds(burstDelay);
        }
    }
}
