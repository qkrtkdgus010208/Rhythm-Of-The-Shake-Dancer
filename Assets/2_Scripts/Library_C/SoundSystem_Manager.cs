using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem_Manager : MonoBehaviour
{
    public static SoundSystem_Manager Instance;

    [SerializeField] private AudioSource bgmAS = null; // ��� ���� AudioSource
    [SerializeField] private AudioSource[] sfxAsArr = null; // ȿ���� AudioSource �迭

    private int sfxAsID; // ���� ��� ���� ȿ���� AudioSource�� �ε���

    // �ʱ�ȭ �Լ�
    public void Init_Func()
    {
        // �̱��� �ν��Ͻ� ����
        Instance = this;
        // ȿ���� �ε��� �ʱ�ȭ
        this.sfxAsID = 0;
    }

    // ��� ���� ��� �Լ�
    public void PlayBgm_Func(BgmType _bgmType)
    {
        // �����ͺ��̽����� ��� ���� ������ ��������
        DataBase_Manager.BgmData _bgmData = DataBase_Manager.Instance.GetBgmData_Func(_bgmType);
        // AudioSource�� Ŭ���� ���� ����
        this.bgmAS.clip = _bgmData.clip;
        this.bgmAS.volume = _bgmData.volume;
        // ��� ���� ���
        this.bgmAS.Play();
    }

    // ��� ���� ���� �Լ�
    public void StopBgm_Func()
    {
        // ��� ���� ����
        this.bgmAS.Stop();
    }

    // ȿ���� ��� �Լ�
    public void PlaySfx_Func(SfxType _sfxType)
    {
        // �����ͺ��̽����� ȿ���� ������ ��������
        DataBase_Manager.SfxData _sfxData = DataBase_Manager.Instance.GetSfxData_Func(_sfxType);

        // ���� �ε����� AudioSource ��������
        AudioSource _sfxAS = this.sfxAsArr[this.sfxAsID];
        // AudioSource�� ���� ����
        _sfxAS.volume = _sfxData.volume;
        // ȿ���� ���
        _sfxAS.PlayOneShot(_sfxData.clip);

        // ���� ȿ���� AudioSource�� �ε��� �̵�
        if (this.sfxAsID + 1 < this.sfxAsArr.Length)
            this.sfxAsID++;
        else
            this.sfxAsID = 0;
    }

    // AudioSource �ʱ�ȭ �Լ�
    private void Reset()
    {
        // ��� ���� AudioSource�� null�� ��� ���� ����
        if (this.bgmAS == null)
        {
            GameObject _bgmObj = new GameObject("BgmAS");
            _bgmObj.transform.SetParent(this.transform);
            this.bgmAS = _bgmObj.AddComponent<AudioSource>();
        }

        // ȿ���� AudioSource �迭�� null�� ��� ���� ����
        if (this.sfxAsArr == null)
        {
            this.sfxAsArr = new AudioSource[10];

            // 10���� AudioSource�� �迭�� �߰�
            for (int i = 0; i < 10; i++)
            {
                GameObject _sfxObj = new GameObject("SfxAS_" + i);
                _sfxObj.transform.SetParent(this.transform);
                this.sfxAsArr[i] = _sfxObj.AddComponent<AudioSource>();
            }
        }

        // ���� ������Ʈ �̸� ����
        this.gameObject.name = typeof(SoundSystem_Manager).Name;
    }
}

// ��� ���� Ÿ�� ������
public enum BgmType
{
    None = 0,

    Ingame = 10,
    GameOver = 20,
    GameClear = 30,
}

// ȿ���� Ÿ�� ������
public enum SfxType
{
    None = 0,

    GetItem = 10,
    Wrong = 20,
    Correct = 30,
    Die = 40,
}