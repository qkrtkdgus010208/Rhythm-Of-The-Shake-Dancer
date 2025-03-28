using DG.Tweening;
using UnityEngine;

public class Block_Script : MonoBehaviour
{
    // 블럭 타입과 X 위치 ID
    private BlockType blockType;
    private int xPosID;
    // 블럭의 스프라이트 렌더러
    [SerializeField] private SpriteRenderer bodySrdr;

    // X 위치 ID를 반환하는 프로퍼티
    public int GetXPosID => this.xPosID;

    // 초기화 함수
    public void Init_Func()
    {
        // 비활성화 함수 호출
        this.Deactivate_Func(true);
    }

    // 블럭 활성화 함수
    public void Activate_Func(BlockType _blockType, int _xPos)
    {
        // 블럭 타입과 위치 설정
        this.blockType = _blockType;
        this.transform.position = new Vector2(_xPos, DataBase_Manager.Instance.mapSpaceMaxY);

        // 블럭 스프라이트 설정
        this.bodySrdr.sprite = DataBase_Manager.Instance.GetBlockSprite_Func(this.blockType);

        // X 위치 ID 설정
        this.xPosID = _xPos;

        // 블럭 스케일 애니메이션
        this.transform.localScale = Vector2.one * 0.001f;
        Sequence _seq = DOTween.Sequence();
        _seq.Append(this.transform.DOScale(1.2f, 0.3f))
           .Append(this.transform.DOScale(1f, 0.1f));
    }

    // 비트 완료 시 호출되는 함수
    public void OnBeatDone_Func()
    {
        if (this.transform.position.y > 0f)
        {
            // 블럭이 한 칸 아래로 이동
            float _arriveHeight = this.transform.position.y - 1f;
            this.transform.DOMoveY(_arriveHeight, DataBase_Manager.Instance.blockDropDuration).OnComplete(() =>
            {
                // 블럭이 바닥에 도착하고 캐릭터와 충돌한 경우
                if (_arriveHeight <= 0 && CharacterSystem_Manager.Instance.IsCollideCharacter_Func(this))
                {
                    switch (this.blockType)
                    {
                        case Block_Script.BlockType.Item:
                            {
                                // 아이템 블럭인 경우 아이템 획득 사운드 재생 및 점수 추가
                                SoundSystem_Manager.Instance.PlaySfx_Func(SfxType.GetItem);
                                ScoreSystem_Manager.Instance.AddScore_Func(this.transform.position);
                            }
                            break;

                        case Block_Script.BlockType.Obstacle:
                            {
                                // 장애물 블럭인 경우 게임 오버 처리
                                GameSystem_Manager.Instance.OnGameOver_Func();
                            }
                            break;

                        default:
                            Debug.LogError("_blockType : " + this.blockType);
                            break;
                    }

                    // 블럭 비활성화
                    this.Deactivate_Func();
                }
            });
        }
        else
        {
            // 블럭이 바닥에 도착한 경우 비활성화
            this.Deactivate_Func();
        }
    }

    // 블럭 비활성화 함수
    public void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            // 블럭 시스템 매니저에 착지 함수 호출
            BlockSystem_Manager.Instance.OnLanding_Func(this);

            // 블럭 스케일 애니메이션 후 파괴
            Sequence _seq = DOTween.Sequence();
            _seq.Append(this.transform.DOScale(1.2f, 0.1f))
               .Append(this.transform.DOScale(0.001f, 0.3f))
               .OnComplete(() => Destroy(this.gameObject));
        }
    }

    // 블럭 타입 열거형
    public enum BlockType
    {
        None = 0,
        Item = 10,
        Obstacle = 20,
    }
}