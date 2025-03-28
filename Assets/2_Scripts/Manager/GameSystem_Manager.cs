using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSystem_Manager : Library_C.GameSystem_Manager
{
    // �̱��� �ν��Ͻ�
    public static GameSystem_Manager Instance;

    // �� �ý��� �Ŵ����� UI ��ҵ�
    [SerializeField] private BeatSystem_Manager beatSystem_Manager = null;
    [SerializeField] private CharacterSystem_Manager characterSystem_Manager = null;
    [SerializeField] private BlockSystem_Manager blockSystem_Manager = null;
    [SerializeField] private ScoreSystem_Manager scoreSystem_Manager = null;
    [SerializeField] private TextMeshProUGUI timerTmp = null;
    [SerializeField] private Image timerImg = null;
    [SerializeField] private GameObject gameOverGroup = null;
    [SerializeField] private GameObject gameClearGroup = null;

    // ���� �ð��� ���� ���� ���� ����
    private float remainTime = 0f;
    private bool isGameOver = false;

    // �ʱ�ȭ �Լ�
    public override void Init_Func()
    {
        // �̱��� �ν��Ͻ� ����
        Instance = this;

        // �� �ý��� �Ŵ��� �ʱ�ȭ
        this.beatSystem_Manager.Init_Func();
        this.characterSystem_Manager.Init_Func();
        this.blockSystem_Manager.Init_Func();
        this.scoreSystem_Manager.Init_Func();

        // ���� ���� �� Ŭ���� UI ��Ȱ��ȭ
        this.gameOverGroup.SetActive(false);
        this.gameClearGroup.SetActive(false);

        base.Init_Func();
    }

    // Ȱ��ȭ �Լ�
    public override void Activate_Func()
    {
        base.Activate_Func();

        // �� �ý��� �Ŵ��� Ȱ��ȭ
        BeatSystem_Manager.Instance.Activate_Func();
        CharacterSystem_Manager.Instance.Activate_Func();
        BlockSystem_Manager.Instance.Activate_Func();
        ScoreSystem_Manager.Instance.Activate_Func();

        // ���� �ð� ����
        this.remainTime = DataBase_Manager.Instance.playTime;

        // Ÿ�̸� �̹��� �ʱ�ȭ
        this.timerImg.fillAmount = 0f;

        // ��� ���� ���
        SoundSystem_Manager.Instance.PlayBgm_Func(BgmType.Ingame);
    }

    // �� �����Ӹ��� ȣ��Ǵ� ������Ʈ �Լ�
    private void Update()
    {
        if (isGameOver)
            return;

        // ���� �ð� ����
        this.remainTime -= Time.deltaTime;

        // Ÿ�̸� �ؽ�Ʈ �� �̹��� ������Ʈ
        this.timerTmp.text = $"{(int)this.remainTime}";
        this.timerImg.fillAmount = 1f - (this.remainTime / DataBase_Manager.Instance.playTime);

        // �ð��� �� �Ǹ� ���� ���� �Լ� ȣ��
        if (this.remainTime <= 0f)
            this.OnGameOver_Func();
    }

    // ���� ���� ó�� �Լ�
    public void OnGameOver_Func()
    {
        this.isGameOver = true;

        // ���� ȿ�� ���
        SoundSystem_Manager.Instance.PlaySfx_Func(SfxType.Die);

        // ��� ���� ���� �� ���� ���� ���� ���
        SoundSystem_Manager.Instance.StopBgm_Func();
        SoundSystem_Manager.Instance.PlayBgm_Func(BgmType.GameOver);

        // ���� ���� UI Ȱ��ȭ
        this.gameOverGroup.SetActive(true);

        // ��Ȱ��ȭ �Լ� ȣ��
        this.Deactivate_Func();
    }

    // ���� Ŭ���� ó�� �Լ�
    public void OnGameClaer_Func()
    {
        // ��� ���� ���� �� ���� Ŭ���� ���� ���
        SoundSystem_Manager.Instance.StopBgm_Func();
        SoundSystem_Manager.Instance.PlayBgm_Func(BgmType.GameClear);

        // ���� Ŭ���� UI Ȱ��ȭ
        this.gameClearGroup.SetActive(true);

        // ��Ȱ��ȭ �Լ� ȣ��
        this.Deactivate_Func();
    }

    // ����� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void CallBtn_Retry_Func()
    {
        SceneManager.LoadScene(0);
    }

    // ��Ȱ��ȭ �Լ�
    public override void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            Debug.Log("���� ����");

            // ��� �ڷ�ƾ ����
            StopAllCoroutines();

            BeatSystem_Manager.Instance.StopAllCoroutines();
            CharacterSystem_Manager.Instance.StopAllCoroutines();
            BlockSystem_Manager.Instance.StopAllCoroutines();
            ScoreSystem_Manager.Instance.StopAllCoroutines();

            // �� �ý��� �Ŵ��� ��Ȱ��ȭ
            BeatSystem_Manager.Instance.Deactivate_Func();
            CharacterSystem_Manager.Instance.Deactivate_Func();
            BlockSystem_Manager.Instance.Deactivate_Func();
            ScoreSystem_Manager.Instance.Deactivate_Func();
        }
    }
}