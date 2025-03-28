using UnityEngine;
using DG.Tweening;

public class CharacterSystem_Manager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static CharacterSystem_Manager Instance;

    // 캐릭터의 스프라이트 렌더러와 애니메이터
    [SerializeField] private SpriteRenderer bodySrdr = null;
    [SerializeField] private Animator anim = null;

    // 캐릭터의 현재 X 위치 ID
    private int xPosID;

    // 초기화 함수
    public void Init_Func()
    {
        // 싱글톤 인스턴스 설정
        Instance = this;

        // 비활성화 함수 호출
        this.Deactivate_Func(true);
    }

    // 활성화 함수
    public void Activate_Func()
    {
        // X 위치 ID 초기화
        this.xPosID = 0;
    }

    // 캐릭터 이동 함수
    public void OnMove_Func(bool _isRight)
    {
        bool _isFlip = false;
        bool _isMove = false;
        Vector2 _movePos = this.transform.position;

        // 오른쪽으로 이동
        if (_isRight)
        {
            if (_movePos.x < 2f)
            {
                _isFlip = true;
                _isMove = true;
                _movePos += Vector2.right;
            }
        }
        // 왼쪽으로 이동
        else
        {
            if (_movePos.x > -2f)
            {
                _isMove = true;
                _movePos += Vector2.left;
            }
        }

        // 이동이 가능한 경우
        if (_isMove)
        {
            // 스프라이트 방향 설정
            this.bodySrdr.flipX = _isFlip;

            // 점프 애니메이션 트리거
            this.anim.SetTrigger("Jump");

            // 점프 애니메이션 실행
            this.transform.DOJump(_movePos, DataBase_Manager.Instance.JumpPower, 1, DataBase_Manager.Instance.JumpDuration).OnComplete(this.SetIdle_Func);

            // X 위치 ID 업데이트
            this.xPosID += _isRight ? 1 : -1;
        }
    }

    // 캐릭터를 Idle 상태로 설정하는 함수
    private void SetIdle_Func()
    {
        this.anim.SetTrigger("Idle");
    }

    // 캐릭터와 블럭이 충돌했는지 확인하는 함수
    public bool IsCollideCharacter_Func(Block_Script _block)
    {
        return this.xPosID == _block.GetXPosID;
    }

    // 비활성화 함수
    public void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            // 초기화가 아닌 경우 추가 작업 가능
        }
    }
}