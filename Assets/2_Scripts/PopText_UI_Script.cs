using TMPro;
using UnityEngine;
using static ScoreSystem_Manager;

public class PopText_UI_Script : MonoBehaviour
{
    // 텍스트 메시 프로 UI 요소
    [SerializeField] private TextMeshProUGUI tmp = null;

    // 초기화 함수
    public void Init_Func()
    {
        // 비활성화 함수 호출
        this.Deactivate_Func(true);
    }

    // 활성화 함수
    public void Activate_Func(string _str, Color _color, int _fontSize, Vector2 _pos)
    {
        // 텍스트, 색상, 폰트 크기, 위치 설정
        this.tmp.text = _str;
        this.tmp.color = _color;
        this.tmp.fontSize = _fontSize;
        this.transform.position = _pos;
    }

    // 비활성화 함수
    public void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            // 게임 오브젝트 파괴
            Destroy(this.gameObject);
        }
    }

    // 애니메이션 호출 함수
    public void CallAni_Func()
    {
        // 비활성화 함수 호출
        this.Deactivate_Func();
    }

    // 정적 활성화 함수
    public static void ForActivate_Func(string _str, Color _color, int _fontSize, Vector2 _pos, Transform spawnTrf)
    {
        // 부모 트랜스폼의 모든 자식 오브젝트 파괴
        foreach (Transform child in spawnTrf)
        {
            Destroy(child.gameObject);
        }

        // 기본 팝업 텍스트 UI 인스턴스 생성 및 설정
        PopText_UI_Script _basePopText = DataBase_Manager.Instance.basePopTextUI;
        PopText_UI_Script _popText = Instantiate(_basePopText);
        _popText.transform.SetParent(spawnTrf);
        _popText.Activate_Func(_str, _color, _fontSize, _pos);
    }
}