using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    #region Singleton
    private static SceneChangeManager _Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능

    // 인스턴스에 접근하기 위한 프로퍼티
    public static SceneChangeManager Instance
    {
        get { return _Instance; }   // SceneChangeManager 인스턴스 변수를 리턴
    }

    // 인스턴스 변수 초기화
    void Awake()
    {
        _Instance = GetComponent<SceneChangeManager>();    // _sceneManager에 SceneChangeManager의 컴포넌트(자기 자신)에 대한 참조를 얻음
    }
    #endregion

    
}
