using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    // 이동관련 변수
    public Vector3 vec;
    public float obstacleTimer;
    private bool isTimer = false;
    // 맵 오브젝트 제거를 위한 변수들
    //public List<Collision> L = new List<Collision> { };
    //public int idx = 0;

    // 캐릭터 오브젝트 관련 변수들
    public GameObject character;        
    public Vector3 Position;
    public float movespeed = 2.2f;

    // 점프관련 변수
    public bool isGround = true;
    bool isjump = false;

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

    private void Awake()
    {
        for(int i=0; i< Blueposition.Length; i++)
        {
            Blueposition[i] = BluePotal[i].transform.position;
        }
        for (int i = 0; i < Redposition.Length; i++)
        {
            Redposition[i] = RedPotal[i].transform.position;
        }
        isClear = Random.Range(0, 10);
        if (isClear == 3) // 3은 스폰장소에 있는 탈출구이므로 최대한 3이 안나오게 하기 위해 추가함
            isClear = Random.Range(0, 10);
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
        // x축, y축, z축의 값을 가진 Vector3 생성
        vec = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.deltaTime * movespeed;

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            vec += Vector3.up * 5f;
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

        //if (Input.GetKey(KeyCode.A))
        //{
        //    // Debug.Log("0");
        //    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //    transform.Translate(Vector3.forward * 2f * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.W))
        //{
        //    // Debug.Log("90");
        //    transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        //    transform.Translate(Vector3.forward * 2f * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    // Debug.Log("180");
        //    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        //    transform.Translate(Vector3.forward * 2f * Time.deltaTime);
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    // Debug.Log("270");
        //    transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
        //    transform.Translate(Vector3.forward * 2f * Time.deltaTime);
        //}


    }

    void isGround_On() { isGround = true; }

    private void OnCollisionEnter(Collision collision) {


        // Debug.Log("OnCollisionEnter " + collision.gameObject.name);

        if (collision.gameObject.tag == "RedPotal" || collision.gameObject.tag == "BluePotal" || collision.gameObject.tag == "Clear")
            stayTime = 0f;

        if (collision.gameObject.tag == "Wood")
        {
            Debug.Log("나무판 : 나무판이 사라짐");
            //L.Add(collision);
            //idx++;
            StartCoroutine(DestroyObject(collision.gameObject));
            StopCoroutine(DestroyObject(null));
        }

      
        if (collision.gameObject.tag == "LightBlue")
        {
            // Debug.Log("LightBlue : 일정한 확률로 떨어짐");
            int num = Random.Range(0, 6);
            if(num < 7)
            {
                //L.Add(collision);
                //idx++;
                StartCoroutine(DestroyObject(collision.gameObject));
                StopCoroutine(DestroyObject(null));
            }
        }
        // HP 적용할지 말지 고민중 - 안하면 아래 if문이랑 합치기
        if (collision.gameObject.tag == "Trap")
        {
            SceneManager.LoadScene("Game");
            SoundManager.Instance.GameOver();
        }
            
        if (collision.gameObject.name == "Ground")
        {
            SceneManager.LoadScene("Game");
            SoundManager.Instance.GameOver();
        }

        if (collision.gameObject.tag == "Star")
        {
            Star_Amount++;
            Debug.Log("Star_Amount : " + Star_Amount);
            Destroy(collision.gameObject);
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
    private void OnCollisionExit (Collision collision) {
        stayTime = 0f;
        }

    private IEnumerator DestroyObject(GameObject Object)
    {
        Debug.Log("DestroyObject 호출완료");
        if (name != null)
        {
            yield return new WaitForSeconds(2f);
            Destroy(Object);
        }
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
            Debug.Log("탈출구 맞음!");
            if(Star_Amount >= 10)
            {
                Debug.Log("게임 클리어");
            }
            else
                Debug.Log("별을 " + (10 - Star_Amount) + "개 더 모으세요");
        }
            
        else
        {
            Debug.Log("탈출구 아님!");
            SoundManager.Instance.NonPassed();
        }
            
    }
}

