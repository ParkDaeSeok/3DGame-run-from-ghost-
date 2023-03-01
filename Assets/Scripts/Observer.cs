using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public GameEnding gameEnding;

    public Transform player;
    //게임 오브젝트가 아닌 플레이어 캐릭터의 Transform을 확인

    bool m_IsPlayerInRange;

    void OnTriggerEnter (Collider other)
    {
        //실제로 공격 범위에 있는지 확인
        if(other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }

    //OnTriggerEnter의 반대인 특수 메서드 OnTriggerExit
    void OnTriggerExit (Collider other)
    {
        if(other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update ()
    {
        if(m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            // A부터 B까지의 벡터는 B - A
            // PointOfView 게임 오브젝트에서 JohnLemon까지의 방향은 JohnLemon의 위치에서 PointOfView 게임 오브젝트의 위치를 뺀 값
            // Vector3.up은 (0, 1, 0)을 간단히 표현

            Ray ray = new Ray (transform.position, direction);

            RaycastHit raycastHit;

           if(Physics.Raycast(ray, out raycastHit)) // ???
            {
                // 레이캐스트 메서드 무언가에 부딪히면 참, 부딪히지 않으면 거짓
                // 레이캐스트가 무언가에 부딪힐 때에만 if 문의 코드가 실행
                if(raycastHit.collider.transform == player)
                {
                    // 스크립트가 플레이어 캐릭터가 공격 범위에 있음을 식별
                    gameEnding.CaughtPlayer();
                }   

            }
        }
    }
}
