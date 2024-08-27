using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct bulletMoveInfo
{
    public float speed;
    public float startTime;
    public Vector2 direction;
    public bool isDirectional; // 속도만 바꿀 때 False
    public bool isContinuous; // 지속적 힘 줄 때 True
    public bool isTargetPosition; // 절대좌표로 움직일 때 True

    public Vector2 TargetVector; // 나중에 절대좌표로 움직일 좌표
}

public abstract class PatternInterface : MonoBehaviour
{
    public GameObject objectPool; // 탄막의 오브젝트 풀

    public int waveCount; // 패턴 반복 수
    public int burstCount; // 한 점사에서 발사할 총 탄막 수
    public float baseAngle; // 탄막 생성 시작 각도
    public float shotAngle;// 탄막 사이 각도
    public float rotateAngle; // 회전각
    public int bulletsPerShot; // 한 번 발사할 때 생성할 탄막 수

    public float shotDelay; // 발사 간격
    public float startDelay;//패턴 시작 딜레이
    public float burstDelay; // 점사 간격

    public Vector2 spawnOffset;//탄막 생성 위치
    public GameObject bulletSpawnPoint;

    public List<bulletMoveInfo> bulletMoveInfos; //탄막 움직임정보값
    public abstract IEnumerator BulletSet(); // 탄환의 초기 위치,방향을 설정하고 BulletSpawn을 호출하는 함수

    public void BulletSpawn(float angle) // 탄환을 생성하는 함수
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
        if (bulletMoveInfos.Count > 0) // 리스트에 요소가 있는지 확인
        {
            bulletMoveInfo tempMoveInfo = bulletMoveInfos[0]; // 임시 변수에 할당
            tempMoveInfo.direction = direction; // 원하는 값으로 수정
            bulletMoveInfos[0] = tempMoveInfo; // 리스트에 다시 할당
        }
        */
        bullet.GetComponent<BulletMover>().MoveInfoAllocate(bulletMoveInfos);
    }

    public void NoPoolingBulletSpawn(float angle) // 탄환을 생성하는 함수
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
        if (bulletMoveInfos.Count > 0) // 리스트에 요소가 있는지 확인
        {
            bulletMoveInfo tempMoveInfo = bulletMoveInfos[0]; // 임시 변수에 할당
            tempMoveInfo.direction = direction; // 원하는 값으로 수정
            bulletMoveInfos[0] = tempMoveInfo; // 리스트에 다시 할당
        }
        */
        bullet.GetComponent<BulletMover>().MoveInfoAllocate(bulletMoveInfos);
    }
}

