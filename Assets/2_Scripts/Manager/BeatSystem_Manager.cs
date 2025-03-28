using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BeatSystem_Manager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static BeatSystem_Manager Instance;

    // UI 비트 클래스
    [SerializeField] private UI_Beat_Script uiBeatClass = null;
    // 현재 비트와 게임 오버 상태 변수
    private float currentBeat = 0f;
    private bool isGameOver = false;

    // 초기화 함수
    public void Init_Func()
    {
        // 싱글톤 인스턴스 설정
        Instance = this;

        // UI 비트 클래스 초기화
        this.uiBeatClass.Init_Func();

        // 비활성화 함수 호출
        this.Deactivate_Func(true);
    }

    // 활성화 함수
    public void Activate_Func()
    {
        // UI 비트 클래스 활성화
        this.uiBeatClass.Activate_Func();

        // 비트 코루틴 시작
        StartCoroutine(OnBeat_Cor());
    }

    // 비트 코루틴
    private IEnumerator OnBeat_Cor()
    {
        while (true)
        {
            // 현재 비트 증가
            this.currentBeat += Time.deltaTime;

            // 비트 리마인더 계산 및 설정
            float _beatReminder = this.GetBeatReminder_Func();
            this.uiBeatClass.SetBeat_Func(_beatReminder);

            // 비트 간격을 초과하면 블럭 시스템의 비트 완료 함수 호출
            if (this.currentBeat > DataBase_Manager.Instance.beatInterval)
            {
                BlockSystem_Manager.Instance.OnBeatDone_Func();

                // 현재 비트 초기화
                this.currentBeat = 0f;
            }

            yield return null;
        }
    }

    // 비트 리마인더 계산 함수
    public float GetBeatReminder_Func()
    {
        float _beatPer = this.currentBeat / DataBase_Manager.Instance.beatInterval;
        return _beatPer % 1f;
    }

    // 매 프레임마다 호출되는 업데이트 함수
    private void Update()
    {
        if (this.isGameOver)
            return;

        // 왼쪽 또는 오른쪽 키 입력 처리
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _OnTryCheckBeat_Func(false);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _OnTryCheckBeat_Func(true);
        }

        // 비트 정확도 체크 함수
        void _OnTryCheckBeat_Func(bool _isRight)
        {
            float _reminder = this.GetBeatReminder_Func();
            Color _color = Color.clear;

            string _popTextStr = string.Empty;
            Color _popTextColor = Color.clear;
            int _fontSize = 0;

            bool _isBeatCorrect = false;

            // 비트 정확도 데이터 배열을 순회하며 정확도 체크
            for (int i = 0; i < DataBase_Manager.Instance.beatAccuracyDataArr.Length; i++)
            {
                DataBase_Manager.BeatAccuracyData _beatAccuracyData = DataBase_Manager.Instance.beatAccuracyDataArr[i];

                float _min = 1f - _beatAccuracyData.accuracyValue;
                if (_reminder >= _min || _reminder <= _beatAccuracyData.accuracyValue)
                {
                    _isBeatCorrect = true;

                    _color = _beatAccuracyData.AccuracyImgColor;

                    _popTextStr = _beatAccuracyData.popTextStr;
                    _popTextColor = _beatAccuracyData.popTextColor;
                    _fontSize = _beatAccuracyData.fontSize;

                    // 캐릭터가 바닥에 있을 때만 이동
                    if (CharacterSystem_Manager.Instance.transform.position.y == 0)
                        CharacterSystem_Manager.Instance.OnMove_Func(_isRight);

                    // 보너스 추가
                    ScoreSystem_Manager.Instance.AddBonus_Func(CharacterSystem_Manager.Instance.transform.position, _beatAccuracyData.bonusValue);

                    break;
                }
            }

            // 비트가 정확하지 않은 경우
            if (!_isBeatCorrect)
            {
                _color = DataBase_Manager.Instance.beatFailColor;

                _popTextStr = DataBase_Manager.Instance.beatFailPopTextStr;
                _popTextColor = DataBase_Manager.Instance.beatFailPopTextColor;
                _fontSize = DataBase_Manager.Instance.beatFailPoptextFontSize;

                // 보너스 초기화
                ScoreSystem_Manager.Instance.OnResetBonus_Func(CharacterSystem_Manager.Instance.transform.position);

                // 잘못된 비트 사운드 재생
                SoundSystem_Manager.Instance.PlaySfx_Func(SfxType.Wrong);
            }
            else
            {
                // 정확한 비트 사운드 재생
                SoundSystem_Manager.Instance.PlaySfx_Func(SfxType.Correct);
            }

            // UI 비트 클래스에 정확도 설정
            this.uiBeatClass.SetAccuracy_Func(_reminder, _color);
            this.uiBeatClass.SetAccuracyPopText(_popTextStr, _popTextColor, _fontSize);
        }
    }

    // 비활성화 함수
    public void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            // 게임 오버 상태 설정
            this.isGameOver = true;

            // UI 비트 클래스 비활성화
            this.uiBeatClass.Deactivate_Func();
        }
    }
}