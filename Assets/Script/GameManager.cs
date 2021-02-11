using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton                             // 싱글톤 패턴은 하나의 인스턴스에 전역적인 접근을 시키며 보통 호출될 때 인스턴스화 되므로 사용하지 않는다면 생성되지도 않습니다.

    public Light Directional_Light; // 2초후에 사라질 빛
    public float play_time;
    public float lightOffTime = 2f; // 2초후에 불이 꺼지게 하는 시간값
    private static GameManager _Instance;             // 싱글톤 객체 선언, 어디에서든지 접근할 수 있도록 하기위해 

    public static GameManager Instance                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _Instance; }                     // GameManager 객체 리턴
    }

    void Awake()                                      // 제일 처음 호출되는 함수
    {
        _Instance = GetComponent<GameManager>();      // _gManager라는 변수에 자신의 GameManager 컴포넌트를 참조하는 값을 저장, Game속성에 set코드를 짜면 다르게 대입가능
    }                                               // Start is called before the first frame update
    #endregion

    void Start()
    {
        Invoke("lightOff", lightOffTime);
    }

    // Update is called once per frame
    void Update()
    {
        play_time += Time.deltaTime;
    }

    void lightOff() { 
        Destroy(Directional_Light);
        UIManager.Instance.PlayTimePanel.SetActive(true);
    }
}