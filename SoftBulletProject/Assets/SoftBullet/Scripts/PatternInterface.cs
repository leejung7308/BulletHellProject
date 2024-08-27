using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct bulletMoveInfo
{
    public float speed;
    public float startTime;
    public Vector2 direction;
    public bool isDirectional; // �ӵ��� �ٲ� �� False
    public bool isContinuous; // ������ �� �� �� True
    public bool isTargetPosition; // ������ǥ�� ������ �� True

    public Vector2 TargetVector; // ���߿� ������ǥ�� ������ ��ǥ
}

public abstract class PatternInterface : MonoBehaviour
{
    public GameObject objectPool; // ź���� ������Ʈ Ǯ

    public int waveCount; // ���� �ݺ� ��
    public int burstCount; // �� ���翡�� �߻��� �� ź�� ��
    public float baseAngle; // ź�� ���� ���� ����
    public float shotAngle;// ź�� ���� ����
    public float rotateAngle; // ȸ����
    public int bulletsPerShot; // �� �� �߻��� �� ������ ź�� ��

    public float shotDelay; // �߻� ����
    public float startDelay;//���� ���� ������
    public float burstDelay; // ���� ����

    public Vector2 spawnOffset;//ź�� ���� ��ġ
    public GameObject bulletSpawnPoint;

    public List<bulletMoveInfo> bulletMoveInfos; //ź�� ������������
    public abstract IEnumerator BulletSet(); // źȯ�� �ʱ� ��ġ,������ �����ϰ� BulletSpawn�� ȣ���ϴ� �Լ�

    public void BulletSpawn(float angle) // źȯ�� �����ϴ� �Լ�
    {
        Vector2 spawnPosition = (Vector2)bulletSpawnPoint.transform.position + spawnOffset;
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        if (objectPool.GetComponent<ObjectPooling>().pool.Count == 0)
        {
            objectPool.GetComponent<ObjectPooling>().CreateBullet();
        }
        GameObject bullet = objectPool.GetComponent<ObjectPooling>().Enable();
        
        bullet.transform.position = spawnPosition;
        bullet.transform.rotation = new Quaternion(0, 0, 0, 0);
        bullet.transform.up = direction.normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletMoveInfos[0].speed;
        /*
        if (bulletMoveInfos.Count > 0) // ����Ʈ�� ��Ұ� �ִ��� Ȯ��
        {
            bulletMoveInfo tempMoveInfo = bulletMoveInfos[0]; // �ӽ� ������ �Ҵ�
            tempMoveInfo.direction = direction; // ���ϴ� ������ ����
            bulletMoveInfos[0] = tempMoveInfo; // ����Ʈ�� �ٽ� �Ҵ�
        }
        */
        bullet.GetComponent<BulletMover>().MoveInfoAllocate(bulletMoveInfos);
    }

    public void NoPoolingBulletSpawn(float angle) // źȯ�� �����ϴ� �Լ�
    {
        Vector2 spawnPosition = (Vector2)bulletSpawnPoint.transform.position + spawnOffset;
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        if (objectPool.GetComponent<ObjectPooling>().pool.Count == 0)
        {
            objectPool.GetComponent<ObjectPooling>().CreateBullet();
        }
        GameObject bullet = Instantiate(objectPool.GetComponent<ObjectPooling>().bulletPrefab);
        bullet.SetActive(true);
        bullet.transform.position = spawnPosition;
        bullet.transform.rotation = new Quaternion(0, 0, 0, 0);
        bullet.transform.up = direction.normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletMoveInfos[0].speed;
        /*
        if (bulletMoveInfos.Count > 0) // ����Ʈ�� ��Ұ� �ִ��� Ȯ��
        {
            bulletMoveInfo tempMoveInfo = bulletMoveInfos[0]; // �ӽ� ������ �Ҵ�
            tempMoveInfo.direction = direction; // ���ϴ� ������ ����
            bulletMoveInfos[0] = tempMoveInfo; // ����Ʈ�� �ٽ� �Ҵ�
        }
        */
        bullet.GetComponent<BulletMover>().MoveInfoAllocate(bulletMoveInfos);
    }
}

