using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    //쿼터니언으로 회전을 저장할 수 있으며, 이를 통해 회전을 3D 벡터로 저장할 때 발생하는 몇 가지 문제를 해결할 수 있습니다


    void Start()
    {
        m_Animator = GetComponent<Animator> ();
        //GetComponent는 MonoBehaviour의 일부이고, 
        //현재 코드를 작성하고 있는 클래스가 MonoBehaviour이므로 이미 접근 권한이 주어진 상태입니다.
        //Animator 유형의 컴포넌트에 대한 레퍼런스를 구하여 m_Animator라는 변수에 할당

        m_Rigidbody = GetComponent<Rigidbody> ();

        m_AudioSource = GetComponent<AudioSource>();
        // Public 변수가 아니기 때문에 인스펙터 창에서 값을 할당할 수 없습니다. 
        // 대신 Animator 및 Rigidbody 컴포넌트의 경우처럼 코드 내에서 값을 할당해야 합니다.

    }

    void FixedUpdate() // 자동으로 호출되는 또 하나의 특수 메서드로, 물리에 맞추어 적시에 호출됩니다
    // FixedUpdate는 렌더링된 프레임에 맞춰 호출되는 것이 아니라 물리 시스템이 충돌 및 상호 작용을 계산하기 전 호출됩니다
    {
        float horizontal = Input.GetAxis("Horizontal"); // 수평
        float vertical = Input.GetAxis ("Vertical"); // 수직
        // Horizontal , Vertical 축의 값을 찾는 코드 라인 추가 변수에 저장

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize(); // 정규화 ?? 

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        // 2개의 플로트 값이 유사하면 참, 그렇지 않으면 거짓을 반환함
        // horizontal 변수가 0에 가까우면 메서드가 참
        // 즉, 수평값이 0이 아니면 hasHorizontalInput은 참이 됩니다.

        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);

        bool isWalking = hasHorizontalInput || hasVerticalInput; // or 연산자 사용

        m_Animator.SetBool ("IsWalking", isWalking);

        if(isWalking)
        {
            if(!m_AudioSource.isPlaying) // 오디오 플레이 중 아니라면
            {
                m_AudioSource.Play ();
            }
        }
        else
        { // 걷고 있지 않으면 오디오 스탑
            m_AudioSource.Stop ();
        }

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        // transform.forward로 시작하여 m_Movement 변수를 목표로 합니다. transform.forward를 이용하면 Transform 컴포넌트에 바로 접근하여 전방 벡터를 구할 수 있습니다.
        // 시작 벡터와 목표 벡터 사이의 변화량으로, 첫 번째는 각도의 변화(단위: 라디안)이고 두 번째는 크기의 변화입니다.  이 코드는 각도를 turnSpeed * Time.deltaTime만큼, 크기를 0만큼 변경합니다.

        m_Rotation = Quaternion.LookRotation (desiredForward);
        //LookRotation 메서드를 호출하여 해당 파라미터 방향으로 바라보는 회전을 생성합니다.
    }

    void OnAnimatorMove ()
    {
        // 루트 모션을 적용하여 이동과 회전을 개별적 적용
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        // deltaPosition은 루트 모션으로 인한 프레임당 위치의 이동량을 말합니다. 
        // 여기서는 deltaPosition의 크기(길이)에 캐릭터의 이동 방향을 나타내는 이동 벡터를 곱했습니다.  

        m_Rigidbody.MoveRotation (m_Rotation);

    }
}
