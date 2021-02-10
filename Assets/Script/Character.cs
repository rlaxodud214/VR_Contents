using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    // 싱글톤 패턴
    #region Singleton
    private static Character _Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능

    // 이동관련 변수
    public Vector3 vec;
    public float obstacleTimer;
    private bool isTimer = false;

    // 캐릭터 오브젝트 관련 변수들
    public GameObject character;        
    public Vector3 Position;
    public float movespeed = 2.2f;

    // 점프관련 변수
    public bool isGround = true;
    public float JumpPower = 4f;
    public bool isjump = false;

    // 게임 클리어 관련 변수
    public int Star_Amount = 0; // 별을 먹은 갯수

    // 게임 클리어 포탈
    public GameObject[] ClearPotal = new GameObject[9];
    public int isClear;

    // 블루포탈 관련 변수들
    public GameObject[] BluePotal = new GameObject[6];
    public Vector3[] Blueposition = new Vector3[6];

    // 레드포탈 관련 변수들
    public GameObject[] RedPotal = new GameObject[8];
    public Vector3[] Redposition = new Vector3[8];

    // 포탈 관련 변수들(공통)
    public float stayTime = 0f;        // 포탈에 머무른 시간 : stayTime += Time.deltatime
    public float PotalStayTime = 2f;   // 2초 머물러야 이동

    // 인스턴스에 접근하기 위한 프로퍼티
    public static Character Instance
    {
        get { return _Instance; }  // UIManager 인스턴스 변수를 리턴
    }

    #endregion
    private void Awake()
    {
        _Instance = GetComponent<Character>();  // _uiManager에 UIManager의 컴포넌트(자기 자신)에 대한 참조를 얻음
        for (int i=0; i< Blueposition.Length; i++)
        {
            Blueposition[i] = BluePotal[i].transform.position;
        }
        for (int i = 0; i < Redposition.Length; i++)
        {
            Redposition[i] = RedPotal[i].transform.position;
        }
        isClear = 3;
        //isClear = Random.Range(0, 10);
        //if (isClear == 3) // 3은 스폰장소에 있는 탈출구이므로 최대한 3이 안나오게 하기 위해 추가함
        //    isClear = Random.Range(0, 10);
    }
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(DestroyObject(null, 0));
        //L.Add(null);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isTimer)
        {
            obstacleTimer += Time.deltaTime;
        }
        //// x축, y축, z축의 값을 가진 Vector3 생성
        // vec = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.deltaTime * movespeed;
        jump();
        inputkey();
    }
    void jump()
    {
        vec = new Vector3(0f, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            vec += Vector3.up * JumpPower;
            isGround = false;
            Invoke("isGround_On", 1.2f);
            isjump = true;
        }

        transform.Translate(vec); // Vector3 : 3차원의 벡터 값

        if (isjump)
        {
            SoundManager.Instance.Jump();
            isjump = false;
        }
    }
    void inputkey()
    {
        if (Input.GetKey(KeyCode.W))                                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        if (Input.GetKey(KeyCode.A))                                transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
        if (Input.GetKey(KeyCode.S))                                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        if (Input.GetKey(KeyCode.D))                                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))     transform.rotation = Quaternion.Euler(new Vector3(0, -45, 0));
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))     transform.rotation = Quaternion.Euler(new Vector3(0, 45, 0));
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))     transform.rotation = Quaternion.Euler(new Vector3(0, -135, 0));
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))     transform.rotation = Quaternion.Euler(new Vector3(0, 135, 0));

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.forward * 2f * Time.deltaTime);
    }
    void isGround_On() { isGround = true; }

    private void OnCollisionEnter(Collision collision) {


        // Debug.Log("OnCollisionEnter " + collision.gameObject.name);

        if (collision.gameObject.tag == "RedPotal" || collision.gameObject.tag == "BluePotal" || collision.gameObject.tag == "Clear")
            stayTime = 0f;

        if (collision.gameObject.tag == "Wood")
        {
            Debug.Log("나무판 : 나무판이 사라짐");
            StartCoroutine(DestroyObject(collision.gameObject));
            StopCoroutine(DestroyObject(null));
        }
              
        if (collision.gameObject.tag == "LightBlue")
        {
            // Debug.Log("LightBlue : 일정한 확률로 떨어짐");
            int num = Random.Range(0, 6);
            if(num < 7)
            {
                StartCoroutine(DestroyObject(collision.gameObject));
                StopCoroutine(DestroyObject(null));
            }
        }
        // HP 적용할지 말지 고민중 - 안하면 아래 if문이랑 합치기
        if (collision.gameObject.tag == "Trap" || collision.gameObject.name == "Ground")
        {
            UIManager.Instance.GameOver();
        }

        if (collision.gameObject.tag == "Star")
        {
            UIManager.Instance.StarUI.transform.GetChild(Star_Amount).gameObject.SetActive(true);
            Star_Amount++;
            Debug.Log("Star_Amount : " + Star_Amount);
            collision.gameObject.GetComponent<CapsuleCollider>().enabled = false;  // 콜라이더 비활성화
            collision.gameObject.transform.Find("FX").gameObject.SetActive(false); // 별모양 비활성화
            collision.gameObject.transform.GetChild(1).transform.gameObject.SetActive(true); // 자식 오브젝트중 1번째 Light를 활성화
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.tag != "RedPotal" && collision.gameObject.tag != "BluePotal" && collision.gameObject.tag != "Clear")
            return;

        stayTime += Time.deltaTime;
        if (stayTime >= PotalStayTime)
        {
            if (collision.gameObject.tag == "RedPotal") 
            { 
                Debug.Log("레드포탈 : 랜덤 위치로 이동~~");
                Redteleport();
            }

            if (collision.gameObject.tag == "BluePotal")
            {
                Debug.Log("블루포탈 : 다른 블루포탈로 이동~~");
                Blueteleport();
            }

            if (collision.gameObject.tag == "Clear")
            {
                Check_Clear(collision);
            }
            stayTime = 0f;
        }
    }
    private void OnCollisionExit(Collision collision) { stayTime = 0f; }

    private IEnumerator DestroyObject(GameObject Object)
    {
        yield return new WaitForSeconds(1f);
        Destroy(Object);
    }

    private void Blueteleport()
    {
        int num = Random.Range(0, 6);
        gameObject.transform.position = Blueposition[num];
        SoundManager.Instance.Blue_Potal();
    }

    private void Redteleport()
    {
        int num = Random.Range(0, 8);
        gameObject.transform.position = Redposition[num];
        SoundManager.Instance.Red_Potal();
    }

    private void Check_Clear(Collision collision)
    {
        Debug.Log("탈출구 : " + ("Waypoint" + isClear.ToString()));
        Debug.Log("현재 통로 : " + collision.gameObject.name);
        if (collision.gameObject.name == ("Waypoint" + isClear.ToString()))
        {
            UIManager.Instance.Answer_Ok();
            if (Star_Amount >= 10)
            {
                UIManager.Instance.GameClear();
            }
            else
                UIManager.Instance.Star_print();
        }
        
        else
        {
            Debug.Log("탈출구 아님!");
            UIManager.Instance.Answer_Non();
            SoundManager.Instance.NonPassed();
        }
            
    }
}

