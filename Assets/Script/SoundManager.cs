﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource myAudio;
    public AudioClip Blue_potal;    // 순간이동
    public AudioClip Red_potal;     // 순간이동
    public AudioClip Gameover;      // 말그대로
    public AudioClip jump;          // 점프시
    public AudioClip nonPassed;     // 탈출구가 아닐 때
//    public AudioClip Passed;        // 탈출구가 맞을 떄
//    public AudioClip Clear;         // 클리어시

    // 싱글톤 패턴
    #region Singleton
    public static SoundManager Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능

    // 인스턴스 변수 초기화
    void Awake()
    {
        // 인스턴스가 생성되지 않았을 때 (인스턴스 중복 생성을 막기 위함)
        if (SoundManager.Instance == null)
        {
            SoundManager.Instance = this;   // 자기 자신을 참조하는 인스턴스 생성
        }
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        myAudio = GetComponent<AudioSource>();  // 오디오 소스(SoundManager 오브젝트)를 가져와 myAudio에 저장
        DontDestroyOnLoad(gameObject);  // 씬 넘어가도 SoundManager 오브젝트 살리기 (Main씬을 넘어가도 SoundManager 오브젝트는 살아있음)
    }
    public void Blue_Potal() // 포탈 이동음
    {
        myAudio.PlayOneShot(Blue_potal); // 오디오 소스로 소리를 한 번 재생시킴
    }
    public void Red_Potal() // 포탈 이동음
    {
        myAudio.PlayOneShot(Red_potal); // 오디오 소스로 소리를 한 번 재생시킴
    }

    public void GameOver() // 게임오버
    {
        myAudio.PlayOneShot(Gameover); // 오디오 소스로 소리를 한 번 재생시킴
    }
    public void Jump() // life 감소음
    {
        myAudio.PlayOneShot(jump); // 오디오 소스로 소리를 한 번 재생시킴
    }

    public void NonPassed()
    {
        myAudio.PlayOneShot(nonPassed); // 오디오 소스로 소리를 한 번 재생시킴
    }
}

