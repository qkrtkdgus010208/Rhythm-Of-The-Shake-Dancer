using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSystem_Manager : Library_C.GameSystem_Manager
{
    // 싱글톤 인스턴스
    public static GameSystem_Manager Instance;

    // 각 시스템 매니저와 UI 요소들
    [SerializeField] private BeatSystem_Manager beatSystem_Manager = null;
    [SerializeField] private CharacterSystem_Manager characterSystem_Manager = null;
    [SerializeField] private BlockSystem_Manager blockSystem_Manager = null;
    [SerializeField] private ScoreSystem_Manager scoreSystem_Manager = null;
    [SerializeField] private TextMeshProUGUI timerTmp = null;
    [SerializeField] private Image timerImg = null;
    [SerializeField] private GameObject gameOverGroup = null;
    [SerializeField] private GameObject gameClearGroup = null;

    // 남은 시간과 게임 오버 상태 변수
    private float remainTime = 0f;
    private bool isGameOver = false;

    // 초기화 함수
    public override void Init_Func()
    {
        // 싱글톤 인스턴스 설정
        Instance = this;

        // 각 시스템 매니저 초기화
        this.beatSystem_Manager.Init_Func();
        this.characterSystem_Manager.Init_Func();
        this.blockSystem_Manager.Init_Func();
        this.scoreSystem_Manager.Init_Func();

        // 게임 오버 및 클리어 UI 비활성화
        this.gameOverGroup.SetActive(false);
        this.gameClearGroup.SetActive(false);

        base.Init_Func();
    }

    // 활성화 함수
    public override void Activate_Func()
    {
        base.Activate_Func();

        // 각 시스템 매니저 활성화
        BeatSystem_Manager.Instance.Activate_Func();
        CharacterSystem_Manager.Instance.Activate_Func();
        BlockSystem_Manager.Instance.Activate_Func();
        ScoreSystem_Manager.Instance.Activate_Func();

        // 남은 시간 설정
        this.remainTime = DataBase_Manager.Instance.playTime;

        // 타이머 이미지 초기화
        this.timerImg.fillAmount = 0f;

        // 배경 음악 재생
        SoundSystem_Manager.Instance.PlayBgm_Func(BgmType.Ingame);
    }

    // 매 프레임마다 호출되는 업데이트 함수
    private void Update()
    {
        if (isGameOver)
            return;

        // 남은 시간 감소
        this.remainTime -= Time.deltaTime;

        // 타이머 텍스트 및 이미지 업데이트
        this.timerTmp.text = $"{(int)this.remainTime}";
        this.timerImg.fillAmount = 1f - (this.remainTime / DataBase_Manager.Instance.playTime);

        // 시간이 다 되면 게임 오버 함수 호출
        if (this.remainTime <= 0f)
            this.OnGameOver_Func();
    }

    // 게임 오버 처리 함수
    public void OnGameOver_Func()
    {
        this.isGameOver = true;

        // 사운드 효과 재생
        SoundSystem_Manager.Instance.PlaySfx_Func(SfxType.Die);

        // 배경 음악 정지 및 게임 오버 음악 재생
        SoundSystem_Manager.Instance.StopBgm_Func();
        SoundSystem_Manager.Instance.PlayBgm_Func(BgmType.GameOver);

        // 게임 오버 UI 활성화
        this.gameOverGroup.SetActive(true);

        // 비활성화 함수 호출
        this.Deactivate_Func();
    }

    // 게임 클리어 처리 함수
    public void OnGameClaer_Func()
    {
        // 배경 음악 정지 및 게임 클리어 음악 재생
        SoundSystem_Manager.Instance.StopBgm_Func();
        SoundSystem_Manager.Instance.PlayBgm_Func(BgmType.GameClear);

        // 게임 클리어 UI 활성화
        this.gameClearGroup.SetActive(true);

        // 비활성화 함수 호출
        this.Deactivate_Func();
    }

    // 재시작 버튼 클릭 시 호출되는 함수
    public void CallBtn_Retry_Func()
    {
        SceneManager.LoadScene(0);
    }

    // 비활성화 함수
    public override void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            Debug.Log("게임 종료");

            // 모든 코루틴 정지
            StopAllCoroutines();

            BeatSystem_Manager.Instance.StopAllCoroutines();
            CharacterSystem_Manager.Instance.StopAllCoroutines();
            BlockSystem_Manager.Instance.StopAllCoroutines();
            ScoreSystem_Manager.Instance.StopAllCoroutines();

            // 각 시스템 매니저 비활성화
            BeatSystem_Manager.Instance.Deactivate_Func();
            CharacterSystem_Manager.Instance.Deactivate_Func();
            BlockSystem_Manager.Instance.Deactivate_Func();
            ScoreSystem_Manager.Instance.Deactivate_Func();
        }
    }
}