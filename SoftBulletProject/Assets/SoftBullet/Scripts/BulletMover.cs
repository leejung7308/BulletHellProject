using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    public List<bulletMoveInfo> bulletMoveInfos;
    private Rigidbody2D rb;
    private GameObject player;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private IEnumerator ApplyMovementPhases()
    {
        Coroutine tempMovementCoroutine = null;

        foreach (bulletMoveInfo moveInfo in bulletMoveInfos)
        {

            if (moveInfo.isDirectional)
            {
                float angle = Vector2.SignedAngle(Vector2.up, moveInfo.direction);
                Vector2 currentDirection = rb.velocity.normalized;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Vector2 newDirection = rotation * currentDirection;
                Vector2 movement = newDirection * moveInfo.speed;
                rb.velocity = movement;
            }
            else if (moveInfo.isTargetPosition)
            {
                Vector2 directionToTarget = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
                Vector2 movement = directionToTarget * moveInfo.speed;
                rb.velocity = movement;
            }
            else
            {
                // 속도만 변경하고, 기존의 방향을 유지
                Vector2 movement = rb.velocity.normalized * moveInfo.speed;
                rb.velocity = movement;
            }

            if (moveInfo.isContinuous)
            {
                tempMovementCoroutine = StartCoroutine(CurvedMovement(moveInfo));
            }

            yield return new WaitForSeconds(moveInfo.startTime);

            if (tempMovementCoroutine != null)
            {
                StopCoroutine(tempMovementCoroutine);
                tempMovementCoroutine = null;
            }
        }
    }

    private IEnumerator CurvedMovement(bulletMoveInfo moveInfo)
    {
        float desiredSpeed = moveInfo.speed;

        while (true)
        {
            Vector2 currentVelocity = rb.velocity;

            float angle = Vector2.SignedAngle(Vector2.up, moveInfo.direction);
            Vector2 currentDirection = currentVelocity.normalized;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector2 newDirection = rotation * currentDirection;

            Vector2 newVelocity = newDirection * desiredSpeed;

            // Rigidbody의 속도를 업데이트합니다.
            rb.velocity = newVelocity;

            yield return new WaitForSeconds(0.1f); // 다음 프레임까지 대기
        }
    }

    public void MoveInfoAllocate(List<bulletMoveInfo> b)
    {
        bulletMoveInfos = new List<bulletMoveInfo>(b);
        StartCoroutine(ApplyMovementPhases());
    }

}
