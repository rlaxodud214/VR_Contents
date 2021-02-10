using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 싱글톤 패턴
    #region Singleton
    private static UIManager _Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능
    public GameObject ResultPanel;
    public GameObject EscapePanel;
    public GameObject StarPanel;
    public GameObject StarUI;
    public Text play_time; // 결과
    public Text Star;      // 결과, 탈출, 별


    // 인스턴스에 접근하기 위한 프로퍼티
    public static UIManager Instance
    {
        get { return _Instance; }  // UIManager 인스턴스 변수를 리턴
    }

    // 인스턴스 변수 초기화
    void Awake()
    {
        _Instance = GetComponent<UIManager>();  // _uiManager에 UIManager의 컴포넌트(자기 자신)에 대한 참조를 얻음
        play_time.text = "0";
    }
    #endregion

    public void GameOver() //게임오버+결과화면 함수
    {
        Time.timeScale = 0f;
        play_time.text = "플레이 시간 : " + GameManager.Instance.play_time.ToString("N2");
        SoundManager.Instance.GameOver();
        MoveCamera.Instance.Play_Time_Text.gameObject.SetActive(false);
        MoveCamera.Instance.play_Time.gameObject.SetActive(false);
        ResultPanel.SetActive(true);
        ResultPanel.gameObject.transform.GetChild(0).transform.gameObject.SetActive(true); // 자식 오브젝트중 1번째 Light를 활성화
    }
    public void GameClear()
    {
        Time.timeScale = 0f;
        ResultPanel.SetActive(true);
        ResultPanel.gameObject.transform.GetChild(1).transform.gameObject.SetActive(true); // 자식 오브젝트중 1번째 Light를 활성화
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
    }

    public void Answer_Ok()
    {
        EscapePanel.SetActive(true);
        EscapePanel.gameObject.transform.GetChild(0).transform.gameObject.SetActive(true); // 자식 오브젝트중 0번째 활성화
    }

    public void Answer_Non()
    {
        EscapePanel.SetActive(true);
        EscapePanel.gameObject.transform.GetChild(1).transform.gameObject.SetActive(true); // 자식 오브젝트중 1번째 활성화
    }

    public void Star_print()
    {
        Star.text = "탈출 불가: 별을 " + (Character.Instance.Star_Amount-10) + "개 더 모으세요";
        StarPanel.gameObject.transform.GetChild(0).transform.gameObject.SetActive(true);   // 자식 오브젝트중 0번째 활성화
        Invoke("Star_print_off", 1f);
    }

    public void Star_print_off()
    {
        StarPanel.gameObject.transform.GetChild(0).transform.gameObject.SetActive(false);   // 자식 오브젝트중 0번째 활성화
    }
}
