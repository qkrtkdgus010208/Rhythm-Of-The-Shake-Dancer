using DG.Tweening;
using UnityEngine;

public class Block_Script : MonoBehaviour
{
    // �� Ÿ�԰� X ��ġ ID
    private BlockType blockType;
    private int xPosID;
    // ���� ��������Ʈ ������
    [SerializeField] private SpriteRenderer bodySrdr;

    // X ��ġ ID�� ��ȯ�ϴ� ������Ƽ
    public int GetXPosID => this.xPosID;

    // �ʱ�ȭ �Լ�
    public void Init_Func()
    {
        // ��Ȱ��ȭ �Լ� ȣ��
        this.Deactivate_Func(true);
    }

    // �� Ȱ��ȭ �Լ�
    public void Activate_Func(BlockType _blockType, int _xPos)
    {
        // �� Ÿ�԰� ��ġ ����
        this.blockType = _blockType;
        this.transform.position = new Vector2(_xPos, DataBase_Manager.Instance.mapSpaceMaxY);

        // �� ��������Ʈ ����
        this.bodySrdr.sprite = DataBase_Manager.Instance.GetBlockSprite_Func(this.blockType);

        // X ��ġ ID ����
        this.xPosID = _xPos;

        // �� ������ �ִϸ��̼�
        this.transform.localScale = Vector2.one * 0.001f;
        Sequence _seq = DOTween.Sequence();
        _seq.Append(this.transform.DOScale(1.2f, 0.3f))
           .Append(this.transform.DOScale(1f, 0.1f));
    }

    // ��Ʈ �Ϸ� �� ȣ��Ǵ� �Լ�
    public void OnBeatDone_Func()
    {
        if (this.transform.position.y > 0f)
        {
            // ���� �� ĭ �Ʒ��� �̵�
            float _arriveHeight = this.transform.position.y - 1f;
            this.transform.DOMoveY(_arriveHeight, DataBase_Manager.Instance.blockDropDuration).OnComplete(() =>
            {
                // ���� �ٴڿ� �����ϰ� ĳ���Ϳ� �浹�� ���
                if (_arriveHeight <= 0 && CharacterSystem_Manager.Instance.IsCollideCharacter_Func(this))
                {
                    switch (this.blockType)
                    {
                        case Block_Script.BlockType.Item:
                            {
                                // ������ ���� ��� ������ ȹ�� ���� ��� �� ���� �߰�
                                SoundSystem_Manager.Instance.PlaySfx_Func(SfxType.GetItem);
                                ScoreSystem_Manager.Instance.AddScore_Func(this.transform.position);
                            }
                            break;

                        case Block_Script.BlockType.Obstacle:
                            {
                                // ��ֹ� ���� ��� ���� ���� ó��
                                GameSystem_Manager.Instance.OnGameOver_Func();
                            }
                            break;

                        default:
                            Debug.LogError("_blockType : " + this.blockType);
                            break;
                    }

                    // �� ��Ȱ��ȭ
                    this.Deactivate_Func();
                }
            });
        }
        else
        {
            // ���� �ٴڿ� ������ ��� ��Ȱ��ȭ
            this.Deactivate_Func();
        }
    }

    // �� ��Ȱ��ȭ �Լ�
    public void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            // �� �ý��� �Ŵ����� ���� �Լ� ȣ��
            BlockSystem_Manager.Instance.OnLanding_Func(this);

            // �� ������ �ִϸ��̼� �� �ı�
            Sequence _seq = DOTween.Sequence();
            _seq.Append(this.transform.DOScale(1.2f, 0.1f))
               .Append(this.transform.DOScale(0.001f, 0.3f))
               .OnComplete(() => Destroy(this.gameObject));
        }
    }

    // �� Ÿ�� ������
    public enum BlockType
    {
        None = 0,
        Item = 10,
        Obstacle = 20,
    }
}