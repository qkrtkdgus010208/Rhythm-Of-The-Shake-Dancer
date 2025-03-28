using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DB_Manager : ScriptableObject
{
    public static DB_Manager Instance;

    [Header("사운드")]
    [SerializeField] private BgmData[] bgmDataArr = null; // 배경 음악 데이터 배열
    [SerializeField] private SfxData[] sfxDataArr = null; // 효과음 데이터 배열
    private Dictionary<BgmType, BgmData> bgmDataDic; // 배경 음악 데이터 딕셔너리
    private Dictionary<SfxType, SfxData> sfxDataDic; // 효과음 데이터 딕셔너리

    // 초기화 함수
    public virtual void Init_Func()
    {
        // 싱글톤 인스턴스 설정
        Instance = this;

        // 배경 음악 데이터 딕셔너리 초기화
        this.bgmDataDic = new Dictionary<BgmType, BgmData>();
        foreach (BgmData _bgmData in this.bgmDataArr)
            this.bgmDataDic.Add(_bgmData.bgmType, _bgmData);

        // 효과음 데이터 딕셔너리 초기화
        this.sfxDataDic = new Dictionary<SfxType, SfxData>();
        foreach (SfxData _sfxData in this.sfxDataArr)
            this.sfxDataDic.Add(_sfxData.sfxType, _sfxData);
    }

    // 배경 음악 데이터 가져오기 함수
    public BgmData GetBgmData_Func(BgmType _bgmType)
    {
        return this.bgmDataDic[_bgmType];
    }

    // 효과음 데이터 가져오기 함수
    public SfxData GetSfxData_Func(SfxType _sfxType)
    {
        return this.sfxDataDic[_sfxType];
    }

    // 사운드 데이터 클래스
    [System.Serializable]
    public class SoundData
    {
        public AudioClip clip; // 오디오 클립
        public float volume = 1f; // 볼륨 (기본값 1)
    }

    // 배경 음악 데이터 클래스
    [System.Serializable]
    public class BgmData : SoundData
    {
        public BgmType bgmType; // 배경 음악 타입
    }

    // 효과음 데이터 클래스
    [System.Serializable]
    public class SfxData : SoundData
    {
        public SfxType sfxType; // 효과음 타입
    }
}