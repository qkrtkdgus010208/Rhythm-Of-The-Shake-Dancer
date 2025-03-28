using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BeatSystem_Manager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static BeatSystem_Manager Instance;

    // UI ��Ʈ Ŭ����
    [SerializeField] private UI_Beat_Script uiBeatClass = null;
    // ���� ��Ʈ�� ���� ���� ���� ����
    private float currentBeat = 0f;
    private bool isGameOver = false;

    // �ʱ�ȭ �Լ�
    public void Init_Func()
    {
        // �̱��� �ν��Ͻ� ����
        Instance = this;

        // UI ��Ʈ Ŭ���� �ʱ�ȭ
        this.uiBeatClass.Init_Func();

        // ��Ȱ��ȭ �Լ� ȣ��
        this.Deactivate_Func(true);
    }

    // Ȱ��ȭ �Լ�
    public void Activate_Func()
    {
        // UI ��Ʈ Ŭ���� Ȱ��ȭ
        this.uiBeatClass.Activate_Func();

        // ��Ʈ �ڷ�ƾ ����
        StartCoroutine(OnBeat_Cor());
    }

    // ��Ʈ �ڷ�ƾ
    private IEnumerator OnBeat_Cor()
    {
        while (true)
        {
            // ���� ��Ʈ ����
            this.currentBeat += Time.deltaTime;

            // ��Ʈ �����δ� ��� �� ����
            float _beatReminder = this.GetBeatReminder_Func();
            this.uiBeatClass.SetBeat_Func(_beatReminder);

            // ��Ʈ ������ �ʰ��ϸ� �� �ý����� ��Ʈ �Ϸ� �Լ� ȣ��
            if (this.currentBeat > DataBase_Manager.Instance.beatInterval)
            {
                BlockSystem_Manager.Instance.OnBeatDone_Func();

                // ���� ��Ʈ �ʱ�ȭ
                this.currentBeat = 0f;
            }

            yield return null;
        }
    }

    // ��Ʈ �����δ� ��� �Լ�
    public float GetBeatReminder_Func()
    {
        float _beatPer = this.currentBeat / DataBase_Manager.Instance.beatInterval;
        return _beatPer % 1f;
    }

    // �� �����Ӹ��� ȣ��Ǵ� ������Ʈ �Լ�
    private void Update()
    {
        if (this.isGameOver)
            return;

        // ���� �Ǵ� ������ Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _OnTryCheckBeat_Func(false);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _OnTryCheckBeat_Func(true);
        }

        // ��Ʈ ��Ȯ�� üũ �Լ�
        void _OnTryCheckBeat_Func(bool _isRight)
        {
            float _reminder = this.GetBeatReminder_Func();
            Color _color = Color.clear;

            string _popTextStr = string.Empty;
            Color _popTextColor = Color.clear;
            int _fontSize = 0;

            bool _isBeatCorrect = false;

            // ��Ʈ ��Ȯ�� ������ �迭�� ��ȸ�ϸ� ��Ȯ�� üũ
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

                    // ĳ���Ͱ� �ٴڿ� ���� ���� �̵�
                    if (CharacterSystem_Manager.Instance.transform.position.y == 0)
                        CharacterSystem_Manager.Instance.OnMove_Func(_isRight);

                    // ���ʽ� �߰�
                    ScoreSystem_Manager.Instance.AddBonus_Func(CharacterSystem_Manager.Instance.transform.position, _beatAccuracyData.bonusValue);

                    break;
                }
            }

            // ��Ʈ�� ��Ȯ���� ���� ���
            if (!_isBeatCorrect)
            {
                _color = DataBase_Manager.Instance.beatFailColor;

                _popTextStr = DataBase_Manager.Instance.beatFailPopTextStr;
                _popTextColor = DataBase_Manager.Instance.beatFailPopTextColor;
                _fontSize = DataBase_Manager.Instance.beatFailPoptextFontSize;

                // ���ʽ� �ʱ�ȭ
                ScoreSystem_Manager.Instance.OnResetBonus_Func(CharacterSystem_Manager.Instance.transform.position);

                // �߸��� ��Ʈ ���� ���
                SoundSystem_Manager.Instance.PlaySfx_Func(SfxType.Wrong);
            }
            else
            {
                // ��Ȯ�� ��Ʈ ���� ���
                SoundSystem_Manager.Instance.PlaySfx_Func(SfxType.Correct);
            }

            // UI ��Ʈ Ŭ������ ��Ȯ�� ����
            this.uiBeatClass.SetAccuracy_Func(_reminder, _color);
            this.uiBeatClass.SetAccuracyPopText(_popTextStr, _popTextColor, _fontSize);
        }
    }

    // ��Ȱ��ȭ �Լ�
    public void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            // ���� ���� ���� ����
            this.isGameOver = true;

            // UI ��Ʈ Ŭ���� ��Ȱ��ȭ
            this.uiBeatClass.Deactivate_Func();
        }
    }
}