using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem_Manager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static ScoreSystem_Manager Instance;

    // ������ ���ʽ� ����
    private int score = 0;
    private float bonus = 0f;
    // �˾� �ؽ�Ʈ �����͸� ������ ����Ʈ
    private List<PopTextData> popTextDataList = null;

    // UI ��ҵ�
    [SerializeField] private TextMeshProUGUI scoreTmp = null;
    [SerializeField] private TextMeshProUGUI bonusTmp = null;
    [SerializeField] private Image scoreImg = null;

    // �ʱ�ȭ �Լ�
    public void Init_Func()
    {
        // �̱��� �ν��Ͻ� ����
        Instance = this;

        // �˾� �ؽ�Ʈ ������ ����Ʈ �ʱ�ȭ
        this.popTextDataList = new List<PopTextData>();

        // ��Ȱ��ȭ �Լ� ȣ��
        this.Deactivate_Func(true);
    }

    // Ȱ��ȭ �Լ�
    public void Activate_Func()
    {
        // ������ ���ʽ� �ʱ�ȭ
        this.score = 0;
        this.bonus = 0f;

        // ���� �̹��� �ʱ�ȭ
        this.scoreImg.fillAmount = 0f;

        // ������ ���ʽ� �ؽ�Ʈ ����
        SetScoreTmp_Func();
        SetBonusTmp_Func();

        // �˾� �ؽ�Ʈ �ڷ�ƾ ����
        StartCoroutine(OnPopText_Cor());
    }

    // �˾� �ؽ�Ʈ �ڷ�ƾ
    private IEnumerator OnPopText_Cor()
    {
        while (true)
        {
            if (this.popTextDataList.Count > 0)
            {
                // ����Ʈ���� ù ��° �˾� �ؽ�Ʈ ������ ��������
                PopTextData _popTextData = this.popTextDataList[0];
                this.popTextDataList.RemoveAt(0);

                // �˾� �ؽ�Ʈ Ȱ��ȭ �Լ� ȣ��
                PopText_Script.ForActivate_Func(_popTextData.str, _popTextData.color, _popTextData.pos);

                // ���� �˾� �ؽ�Ʈ���� ���
                yield return new WaitForSeconds(DataBase_Manager.Instance.popTextInterval);
            }
            else
            {
                yield return null;
            }
        }
    }

    // �ؽ�Ʈ �޽� ���� �ִϸ��̼� �Լ�
    public void TmpBounc_Func(TextMeshProUGUI _tmp)
    {
        Transform _trf = _tmp.transform;
        Sequence _seq = DOTween.Sequence();
        _seq.Append(_trf.DOScale(2f, 0.1f))
           .Append(_trf.DOScale(1f, 0.1f));
    }

    // ���� �߰� �Լ�
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

    // ���� �ؽ�Ʈ ���� �Լ�
    public void SetScoreTmp_Func()
    {
        this.scoreTmp.text = this.score.ToString();

        TmpBounc_Func(this.scoreTmp);
    }

    // ���ʽ� �߰� �Լ�
    public void AddBonus_Func(Vector2 _pos, float _bonusValue)
    {
        this.bonus += _bonusValue;

        string _str = $"Bonus +{_bonusValue * 100}%";
        this.popTextDataList.Add(new PopTextData(_str, Color.yellow, _pos));

        SetBonusTmp_Func();
    }

    // ���ʽ� �ʱ�ȭ �Լ�
    public void OnResetBonus_Func(Vector2 _pos)
    {
        this.bonus = 0f;

        string _str = $"Bonus Reset";
        this.popTextDataList.Add(new PopTextData(_str, Color.grey, _pos));

        SetBonusTmp_Func();
    }

    // ���ʽ� �ؽ�Ʈ ���� �Լ�
    public void SetBonusTmp_Func()
    {
        int _bonusInt = (int)(this.bonus * 100);
        this.bonusTmp.text = $"Bonus {_bonusInt}%";

        TmpBounc_Func(this.bonusTmp);
    }

    // ��Ȱ��ȭ �Լ�
    public void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            // �ʱ�ȭ�� �ƴ� ��� �߰� �۾� ����
        }

        // �˾� �ؽ�Ʈ ������ ����Ʈ �ʱ�ȭ
        this.popTextDataList.Clear();
    }

    // �˾� �ؽ�Ʈ ������ ����ü
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