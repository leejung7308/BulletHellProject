using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatternHomming : PatternInterface
{
    public float angleOffset;//지향성 탄막일때 추가 각도 조정
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
            if (player != null)
            {
                Vector2 directionToPlayer = (Vector2)player.transform.position - ((Vector2)transform.position + spawnOffset);
                baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg + angleOffset;
            }
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
