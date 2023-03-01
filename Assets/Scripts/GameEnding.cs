using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    // 일정 기간이 지나면 화면을 페이드아웃
    // 페이드가 발생하는 적정한 기본값은 1초

    public float displayImageDuration = 1f;

    public GameObject player;
    //JohnLemon이 트리거와 충돌할 때만 발생하도록 하려면 해당 게임 오브젝트에 대한 레퍼런스

    public CanvasGroup exitBackgroundImageCanvasGroup;
    // Canvas Group 컴포넌트에 대한 public 변수를 만들어 인스펙터에 할당될 수 있도록 합니다.
    public AudioSource exitAudio;

    bool m_IsPlayerAtExit;
    // 페이드인과 페이드아웃 두 종류의 상태

    float m_Timer;
    // 페이드가 끝나기 전에 게임이 종료되지 않도록 타이머가 필요합니다

    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource caughtAudio;

    bool m_IsPlayerCaught;
    // 변수는 JohnLemon이 적에게 잡혔는지 확인
    bool m_HasAudioPlayed;

    void OnTriggerEnter(Collider other) {
        if(other.gameObject == player) { // 충돌 시
            // other 콜라이더의 게임 오브젝트(트리거로 진입한 오브젝트)가 JohnLemon의 게임 오브젝트에 대한 레퍼런스와 동일하다면 코드를 실행하라
            m_IsPlayerAtExit = true;
        }
    }
    public void CaughtPlayer ()
    {
        m_IsPlayerCaught = true;
    }

    void Update ()
    {
        //프레임마다 호출되어 캔버스 그룹의 알파 값을 점진적으로 변화
        if(m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel (caughtBackgroundImageCanvasGroup, true, caughtAudio);
            //m_IsPlayerCaught가 참이면 caughtBackgroundImageCanvasGroup을 페이드인
        }

    }

    void EndLevel (CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if(!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }
        //EndLevel 메서드는 캔버스 그룹을 페이드한 다음 게임을 종료해야 합니다
        m_Timer += Time.deltaTime;
        // 타이머를 카운트

        imageCanvasGroup.alpha = m_Timer / fadeDuration;
        //타이머가 fadeDuration에 도달하면 1이어야 합니다

        if(m_Timer > fadeDuration + displayImageDuration)
        {
            if(doRestart) {
                SceneManager.LoadScene(0);
                // 씬이 하나밖에 없기 때문에 씬 컬렉션의 첫 번째에 자리하므로 인덱스는 0이 됩니다.
            }
            // 페이드가 끝나면 게임이 종료되어야 합니다. 
            // 타이머가 지정된 시간을 넘으면 페이드는 종료됩니다. 
            else {
                Application.Quit();
            }
        }
    }   
}
