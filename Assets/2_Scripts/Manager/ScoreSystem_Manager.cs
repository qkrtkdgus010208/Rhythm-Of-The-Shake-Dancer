using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem_Manager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static ScoreSystem_Manager Instance;

    // 점수와 보너스 변수
    private int score = 0;
    private float bonus = 0f;
    // 팝업 텍스트 데이터를 저장할 리스트
    private List<PopTextData> popTextDataList = null;

    // UI 요소들
    [SerializeField] private TextMeshProUGUI scoreTmp = null;
    [SerializeField] private TextMeshProUGUI bonusTmp = null;
    [SerializeField] private Image scoreImg = null;

    // 초기화 함수
    public void Init_Func()
    {
        // 싱글톤 인스턴스 설정
        Instance = this;

        // 팝업 텍스트 데이터 리스트 초기화
        this.popTextDataList = new List<PopTextData>();

        // 비활성화 함수 호출
        this.Deactivate_Func(true);
    }

    // 활성화 함수
    public void Activate_Func()
    {
        // 점수와 보너스 초기화
        this.score = 0;
        this.bonus = 0f;

        // 점수 이미지 초기화
        this.scoreImg.fillAmount = 0f;

        // 점수와 보너스 텍스트 설정
        SetScoreTmp_Func();
        SetBonusTmp_Func();

        // 팝업 텍스트 코루틴 시작
        StartCoroutine(OnPopText_Cor());
    }

    // 팝업 텍스트 코루틴
    private IEnumerator OnPopText_Cor()
    {
        while (true)
        {
            if (this.popTextDataList.Count > 0)
            {
                // 리스트에서 첫 번째 팝업 텍스트 데이터 가져오기
                PopTextData _popTextData = this.popTextDataList[0];
                this.popTextDataList.RemoveAt(0);

                // 팝업 텍스트 활성화 함수 호출
                PopText_Script.ForActivate_Func(_popTextData.str, _popTextData.color, _popTextData.pos);

                // 다음 팝업 텍스트까지 대기
                yield return new WaitForSeconds(DataBase_Manager.Instance.popTextInterval);
            }
            else
            {
                yield return null;
            }
        }
    }

    // 텍스트 메시 프로 애니메이션 함수
    public void TmpBounc_Func(TextMeshProUGUI _tmp)
    {
        Transform _trf = _tmp.transform;
        Sequence _seq = DOTween.Sequence();
        _seq.Append(_trf.DOScale(2f, 0.1f))
           .Append(_trf.DOScale(1f, 0.1f));
    }

    // 점수 추가 함수
    public void AddScore_Func(Vector2 _pos)
    {
        int _itemScore = DataBase_Manager.Instance.itemScore;
        this.popTextDataList.Add(new PopTextData(_itemScore.ToString(), Color.white, _pos));

        int _bonusScore = (int)(_itemScore * this.bonus);
        this.popTextDataList.Add(new PopTextData(_bonusScore.ToString(), Color.white, _pos));

        this.score += _itemScore + _bonusScore;

        this.scoreImg.fillAmount = ((float)this.score / DataBase_Manager.Instance.goalScore);

        SetScoreTmp_Func();

        if (this.score >= DataBase_Manager.Instance.goalScore)
            GameSystem_Manager.Instance.OnGameClaer_Func();
    }

    // 점수 텍스트 설정 함수
    public void SetScoreTmp_Func()
    {
        this.scoreTmp.text = this.score.ToString();

        TmpBounc_Func(this.scoreTmp);
    }

    // 보너스 추가 함수
    public void AddBonus_Func(Vector2 _pos, float _bonusValue)
    {
        this.bonus += _bonusValue;

        string _str = $"Bonus +{_bonusValue * 100}%";
        this.popTextDataList.Add(new PopTextData(_str, Color.yellow, _pos));

        SetBonusTmp_Func();
    }

    // 보너스 초기화 함수
    public void OnResetBonus_Func(Vector2 _pos)
    {
        this.bonus = 0f;

        string _str = $"Bonus Reset";
        this.popTextDataList.Add(new PopTextData(_str, Color.grey, _pos));

        SetBonusTmp_Func();
    }

    // 보너스 텍스트 설정 함수
    public void SetBonusTmp_Func()
    {
        int _bonusInt = (int)(this.bonus * 100);
        this.bonusTmp.text = $"Bonus {_bonusInt}%";

        TmpBounc_Func(this.bonusTmp);
    }

    // 비활성화 함수
    public void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            // 초기화가 아닌 경우 추가 작업 가능
        }

        // 팝업 텍스트 데이터 리스트 초기화
        this.popTextDataList.Clear();
    }

    // 팝업 텍스트 데이터 구조체
    public struct PopTextData
    {
        public string str;
        public Color color;
        public Vector2 pos;

        public PopTextData(string _str, Color _color, Vector2 _pos)
        {
            this.str = _str;
            this.color = _color;
            this.pos = _pos;
        }
    }
}