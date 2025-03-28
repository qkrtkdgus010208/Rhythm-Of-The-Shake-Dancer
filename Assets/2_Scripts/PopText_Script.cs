using TMPro;
using UnityEngine;

public class PopText_Script : MonoBehaviour
{
    // �ؽ�Ʈ �޽� ���� UI ���
    [SerializeField] private TextMeshPro tmp = null;

    // �ʱ�ȭ �Լ�
    public void Init_Func()
    {
        // ��Ȱ��ȭ �Լ� ȣ��
        this.Deactivate_Func(true);
    }

    // Ȱ��ȭ �Լ�
    public void Activate_Func(string _str, Color _color, Vector2 _pos)
    {
        // �ؽ�Ʈ, ����, ��ġ ����
        this.tmp.text = _str;
        this.tmp.color = _color;
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
    public static void ForActivate_Func(string _str, Color _color, Vector2 _pos)
    {
        // �⺻ �˾� �ؽ�Ʈ �ν��Ͻ� ���� �� ����
        PopText_Script _basePopText = DataBase_Manager.Instance.basePopText;
        PopText_Script _popText = Instantiate(_basePopText);
        _popText.Activate_Func(_str, _color, _pos);
    }
}