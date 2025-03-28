using UnityEngine;
using DG.Tweening;

public class CharacterSystem_Manager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static CharacterSystem_Manager Instance;

    // ĳ������ ��������Ʈ �������� �ִϸ�����
    [SerializeField] private SpriteRenderer bodySrdr = null;
    [SerializeField] private Animator anim = null;

    // ĳ������ ���� X ��ġ ID
    private int xPosID;

    // �ʱ�ȭ �Լ�
    public void Init_Func()
    {
        // �̱��� �ν��Ͻ� ����
        Instance = this;

        // ��Ȱ��ȭ �Լ� ȣ��
        this.Deactivate_Func(true);
    }

    // Ȱ��ȭ �Լ�
    public void Activate_Func()
    {
        // X ��ġ ID �ʱ�ȭ
        this.xPosID = 0;
    }

    // ĳ���� �̵� �Լ�
    public void OnMove_Func(bool _isRight)
    {
        bool _isFlip = false;
        bool _isMove = false;
        Vector2 _movePos = this.transform.position;

        // ���������� �̵�
        if (_isRight)
        {
            if (_movePos.x < 2f)
            {
                _isFlip = true;
                _isMove = true;
                _movePos += Vector2.right;
            }
        }
        // �������� �̵�
        else
        {
            if (_movePos.x > -2f)
            {
                _isMove = true;
                _movePos += Vector2.left;
            }
        }

        // �̵��� ������ ���
        if (_isMove)
        {
            // ��������Ʈ ���� ����
            this.bodySrdr.flipX = _isFlip;

            // ���� �ִϸ��̼� Ʈ����
            this.anim.SetTrigger("Jump");

            // ���� �ִϸ��̼� ����
            this.transform.DOJump(_movePos, DataBase_Manager.Instance.JumpPower, 1, DataBase_Manager.Instance.JumpDuration).OnComplete(this.SetIdle_Func);

            // X ��ġ ID ������Ʈ
            this.xPosID += _isRight ? 1 : -1;
        }
    }

    // ĳ���͸� Idle ���·� �����ϴ� �Լ�
    private void SetIdle_Func()
    {
        this.anim.SetTrigger("Idle");
    }

    // ĳ���Ϳ� ���� �浹�ߴ��� Ȯ���ϴ� �Լ�
    public bool IsCollideCharacter_Func(Block_Script _block)
    {
        return this.xPosID == _block.GetXPosID;
    }

    // ��Ȱ��ȭ �Լ�
    public void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            // �ʱ�ȭ�� �ƴ� ��� �߰� �۾� ����
        }
    }
}