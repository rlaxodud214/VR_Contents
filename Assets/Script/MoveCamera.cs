using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 target_position;
    public Text Play_Time_Text;
    public Text play_Time;
    public float camera_move_speed = 2f;
    public float x, y, z;

    #region Singleton                                         // 싱글톤 패턴은 하나의 인스턴스에 전역적인 접근을 시키며 보통 호출될 때 인스턴스화 되므로 사용하지 않는다면 생성되지도 않습니다.

    private static MoveCamera _Instance;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함
    public static MoveCamera Instance                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _Instance; }                         // _sceneManager이 변수값을 리턴받을 수 있음.
    }


    void Awake()                                               // Start()보다 먼저 실행
    {
        Screen.SetResolution(1440, 1660, false);
        _Instance = GetComponent<MoveCamera>();    // _sceneManager변수에 자신의 SceneChangeManager 컴포넌트를 넣는다.
        x = -5.5f;
        y = -2f;
        z = 12f;
    }
    #endregion 

    void Update()
    {
        target_position = new Vector3(target.position.x + x, target.position.y + y, target.position.z + z);
        transform.position = Vector3.Lerp(transform.position, target_position, Time.deltaTime * camera_move_speed);
        play_Time.text = GameManager.Instance.play_time.ToString("N2");
    }
}
