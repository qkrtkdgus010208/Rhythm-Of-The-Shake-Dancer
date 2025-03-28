using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem_Manager : MonoBehaviour
{
    public static SoundSystem_Manager Instance;

    [SerializeField] private AudioSource bgmAS = null; // 배경 음악 AudioSource
    [SerializeField] private AudioSource[] sfxAsArr = null; // 효과음 AudioSource 배열

    private int sfxAsID; // 현재 사용 중인 효과음 AudioSource의 인덱스

    // 초기화 함수
    public void Init_Func()
    {
        // 싱글톤 인스턴스 설정
        Instance = this;
        // 효과음 인덱스 초기화
        this.sfxAsID = 0;
    }

    // 배경 음악 재생 함수
    public void PlayBgm_Func(BgmType _bgmType)
    {
        // 데이터베이스에서 배경 음악 데이터 가져오기
        DataBase_Manager.BgmData _bgmData = DataBase_Manager.Instance.GetBgmData_Func(_bgmType);
        // AudioSource에 클립과 볼륨 설정
        this.bgmAS.clip = _bgmData.clip;
        this.bgmAS.volume = _bgmData.volume;
        // 배경 음악 재생
        this.bgmAS.Play();
    }

    // 배경 음악 정지 함수
    public void StopBgm_Func()
    {
        // 배경 음악 정지
        this.bgmAS.Stop();
    }

    // 효과음 재생 함수
    public void PlaySfx_Func(SfxType _sfxType)
    {
        // 데이터베이스에서 효과음 데이터 가져오기
        DataBase_Manager.SfxData _sfxData = DataBase_Manager.Instance.GetSfxData_Func(_sfxType);

        // 현재 인덱스의 AudioSource 가져오기
        AudioSource _sfxAS = this.sfxAsArr[this.sfxAsID];
        // AudioSource에 볼륨 설정
        _sfxAS.volume = _sfxData.volume;
        // 효과음 재생
        _sfxAS.PlayOneShot(_sfxData.clip);

        // 다음 효과음 AudioSource로 인덱스 이동
        if (this.sfxAsID + 1 < this.sfxAsArr.Length)
            this.sfxAsID++;
        else
            this.sfxAsID = 0;
    }

    // AudioSource 초기화 함수
    private void Reset()
    {
        // 배경 음악 AudioSource가 null인 경우 새로 생성
        if (this.bgmAS == null)
        {
            GameObject _bgmObj = new GameObject("BgmAS");
            _bgmObj.transform.SetParent(this.transform);
            this.bgmAS = _bgmObj.AddComponent<AudioSource>();
        }

        // 효과음 AudioSource 배열이 null인 경우 새로 생성
        if (this.sfxAsArr == null)
        {
            this.sfxAsArr = new AudioSource[10];

            // 10개의 AudioSource를 배열에 추가
            for (int i = 0; i < 10; i++)
            {
                GameObject _sfxObj = new GameObject("SfxAS_" + i);
                _sfxObj.transform.SetParent(this.transform);
                this.sfxAsArr[i] = _sfxObj.AddComponent<AudioSource>();
            }
        }

        // 게임 오브젝트 이름 설정
        this.gameObject.name = typeof(SoundSystem_Manager).Name;
    }
}

// 배경 음악 타입 열거형
public enum BgmType
{
    None = 0,

    Ingame = 10,
    GameOver = 20,
    GameClear = 30,
}

// 효과음 타입 열거형
public enum SfxType
{
    None = 0,

    GetItem = 10,
    Wrong = 20,
    Correct = 30,
    Die = 40,
}