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
    public GameObject PlayTimePanel;
    public GameObject StarPanel;
    public GameObject StarUI;
    public GameObject SpeedPanel;
    public Text speed;
    public Text StarPanel_Text;
    public Text play_time; // 결과
    public Text Star;      // 결과, 탈출, 별
    public float Panel_Print_Time;


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
        Panel_Print_Time = 1.5f;
    }
    #endregion

    public void GameOver() //게임오버+결과화면 함수
    {
        Time.timeScale = 0f;
        play_time.text = "Play Time : " + GameManager.Instance.play_time.ToString("N2") + "초";
        SoundManager.Instance.GameOver();
        PlayTimePanel.SetActive(false);
        EscapePanel.SetActive(false);
        SpeedPanel.SetActive(false);
        StarUI.SetActive(false);
        ResultPanel.SetActive(true);
        ResultPanel.gameObject.transform.GetChild(0).transform.gameObject.SetActive(true); // 자식 오브젝트중 1번째 Light를 활성화
    }
    public void GameClear()
    {
        Time.timeScale = 0f;
        play_time.text = "Clear Time : " + GameManager.Instance.play_time.ToString("N2") + "초";
        SoundManager.Instance.GameClear();
        PlayTimePanel.SetActive(false);
        EscapePanel.SetActive(false);
        SpeedPanel.SetActive(false);
        StarUI.SetActive(false);
        ResultPanel.SetActive(true);
        ResultPanel.gameObject.transform.GetChild(1).transform.gameObject.SetActive(true); // 자식 오브젝트중 1번째 Light를 활성화
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void Answer_Ok()
    {
        EscapePanel.SetActive(true);
        EscapePanel.gameObject.transform.GetChild(0).transform.gameObject.SetActive(true); // 자식 오브젝트중 0번째 활성화
        Star.text = "별을 " + (10 - Character.Instance.Star_Amount) + "개 더 모으세요";
        EscapePanel.gameObject.transform.GetChild(2).transform.gameObject.SetActive(true);   // 자식 오브젝트중 0번째 활성화
        Invoke("Answer_Ok_off", Panel_Print_Time);
    }

    public void Answer_Ok_off()
    {
        EscapePanel.gameObject.transform.GetChild(0).transform.gameObject.SetActive(false); // 자식 오브젝트중 0번째 활성화
        EscapePanel.gameObject.transform.GetChild(2).transform.gameObject.SetActive(false);   // 자식 오브젝트중 0번째 활성화
        EscapePanel.SetActive(false);
    }

    public void Answer_Non()
    {
        EscapePanel.SetActive(true);
        EscapePanel.gameObject.transform.GetChild(1).transform.gameObject.SetActive(true); // 자식 오브젝트중 1번째 활성화
        Invoke("Answer_Non_off", Panel_Print_Time);
    }

    public void Answer_Non_off()
    {
        EscapePanel.gameObject.transform.GetChild(1).transform.gameObject.SetActive(false); // 자식 오브젝트중 1번째 활성화
        EscapePanel.SetActive(false);
    }

    public void print_Star()
    {
        StarPanel_Text.text = "별 획득 (" + Character.Instance.Star_Amount + " / 10)";
        StarPanel.SetActive(true);
        Invoke("print_Star_off", Panel_Print_Time);
    }

    public void print_Star_off()
    {
        StarPanel.SetActive(false);
    }

    public void Speed_Panel(float s)
    {
        SoundManager.Instance.buttonClick();
        SpeedPanel.SetActive(true);
        speed.text = "이동속도 : " + s;
        Invoke("Speed_Panel_off", Panel_Print_Time);
    }
    public void Speed_Panel_off()
    {
        SpeedPanel.SetActive(false);
    }
}
