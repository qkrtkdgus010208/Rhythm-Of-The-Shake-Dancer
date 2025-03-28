using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Beat_Script : MonoBehaviour
{
    // 비트 진행 이미지와 정확도 이미지
    [SerializeField] private Image beatProgressImg = null;
    [SerializeField] private Image accuracyImg = null;
    // 팝업 텍스트 스폰 위치
    [SerializeField] private Transform spawnPos = null;
    // 정확도 이미지 숨김 코루틴
    private Coroutine hideCoroutine;

    // 초기화 함수
    public void Init_Func()
    {
        // 비활성화 함수 호출
        this.Deactivate_Func(true);
    }

    // 활성화 함수
    public void Activate_Func()
    {
        // 정확도 이미지 비활성화 및 투명도 설정
        this.accuracyImg.gameObject.SetActive(false);
        this.accuracyImg.color = new Color(this.accuracyImg.color.r, this.accuracyImg.color.g, this.accuracyImg.color.b, 0f);
    }

    // 비트 진행 설정 함수
    public void SetBeat_Func(float _beatReminder)
    {
        this.beatProgressImg.fillAmount = _beatReminder;
    }

    // 정확도 설정 함수
    public void SetAccuracy_Func(float _per, Color _color)
    {
        // 정확도 이미지 회전 및 색상 설정
        this.accuracyImg.transform.eulerAngles = Vector3.back * _per * 360f;
        this.accuracyImg.color = _color;

        // 정확도 이미지 활성화 및 페이드 인
        this.accuracyImg.gameObject.SetActive(true);
        this.accuracyImg.DOFade(1f, 0.1f);

        // 이전 코루틴이 실행 중이면 중지
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        // 정확도 이미지 숨김 코루틴 시작
        hideCoroutine = StartCoroutine(HideAccuracyImage());
    }

    // 정확도 이미지 숨김 코루틴
    public IEnumerator HideAccuracyImage()
    {
        yield return new WaitForSeconds(0.5f);

        // 정확도 이미지 페이드 아웃 및 비활성화
        this.accuracyImg.DOFade(0f, 0.2f).OnComplete(() =>
        {
            this.accuracyImg.gameObject.SetActive(false);
        });
    }

    // 정확도 팝업 텍스트 설정 함수
    public void SetAccuracyPopText(string _str, Color _color, int _fontSize)
    {
        PopText_UI_Script.ForActivate_Func(_str, _color, _fontSize, this.transform.position, spawnPos);
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