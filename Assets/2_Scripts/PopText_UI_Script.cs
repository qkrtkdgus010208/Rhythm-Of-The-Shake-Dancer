using TMPro;
using UnityEngine;
using static ScoreSystem_Manager;

public class PopText_UI_Script : MonoBehaviour
{
    // �ؽ�Ʈ �޽� ���� UI ���
    [SerializeField] private TextMeshProUGUI tmp = null;

    // �ʱ�ȭ �Լ�
    public void Init_Func()
    {
        // ��Ȱ��ȭ �Լ� ȣ��
        this.Deactivate_Func(true);
    }

    // Ȱ��ȭ �Լ�
    public void Activate_Func(string _str, Color _color, int _fontSize, Vector2 _pos)
    {
        // �ؽ�Ʈ, ����, ��Ʈ ũ��, ��ġ ����
        this.tmp.text = _str;
        this.tmp.color = _color;
        this.tmp.fontSize = _fontSize;
        this.transform.position = _pos;
    }

    // ��Ȱ��ȭ �Լ�
    public void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            // ���� ������Ʈ �ı�
            Destroy(this.gameObject);
        }
    }

    // �ִϸ��̼� ȣ�� �Լ�
    public void CallAni_Func()
    {
        // ��Ȱ��ȭ �Լ� ȣ��
        this.Deactivate_Func();
    }

    // ���� Ȱ��ȭ �Լ�
    public static void ForActivate_Func(string _str, Color _color, int _fontSize, Vector2 _pos, Transform spawnTrf)
    {
        // �θ� Ʈ�������� ��� �ڽ� ������Ʈ �ı�
        foreach (Transform child in spawnTrf)
        {
            Destroy(child.gameObject);
        }

        // �⺻ �˾� �ؽ�Ʈ UI �ν��Ͻ� ���� �� ����
        PopText_UI_Script _basePopText = DataBase_Manager.Instance.basePopTextUI;
        PopText_UI_Script _popText = Instantiate(_basePopText);
        _popText.transform.SetParent(spawnTrf);
        _popText.Activate_Func(_str, _color, _fontSize, _pos);
    }
}