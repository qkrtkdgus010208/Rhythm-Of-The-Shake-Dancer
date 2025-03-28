using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Beat_Script : MonoBehaviour
{
    // ��Ʈ ���� �̹����� ��Ȯ�� �̹���
    [SerializeField] private Image beatProgressImg = null;
    [SerializeField] private Image accuracyImg = null;
    // �˾� �ؽ�Ʈ ���� ��ġ
    [SerializeField] private Transform spawnPos = null;
    // ��Ȯ�� �̹��� ���� �ڷ�ƾ
    private Coroutine hideCoroutine;

    // �ʱ�ȭ �Լ�
    public void Init_Func()
    {
        // ��Ȱ��ȭ �Լ� ȣ��
        this.Deactivate_Func(true);
    }

    // Ȱ��ȭ �Լ�
    public void Activate_Func()
    {
        // ��Ȯ�� �̹��� ��Ȱ��ȭ �� ���� ����
        this.accuracyImg.gameObject.SetActive(false);
        this.accuracyImg.color = new Color(this.accuracyImg.color.r, this.accuracyImg.color.g, this.accuracyImg.color.b, 0f);
    }

    // ��Ʈ ���� ���� �Լ�
    public void SetBeat_Func(float _beatReminder)
    {
        this.beatProgressImg.fillAmount = _beatReminder;
    }

    // ��Ȯ�� ���� �Լ�
    public void SetAccuracy_Func(float _per, Color _color)
    {
        // ��Ȯ�� �̹��� ȸ�� �� ���� ����
        this.accuracyImg.transform.eulerAngles = Vector3.back * _per * 360f;
        this.accuracyImg.color = _color;

        // ��Ȯ�� �̹��� Ȱ��ȭ �� ���̵� ��
        this.accuracyImg.gameObject.SetActive(true);
        this.accuracyImg.DOFade(1f, 0.1f);

        // ���� �ڷ�ƾ�� ���� ���̸� ����
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        // ��Ȯ�� �̹��� ���� �ڷ�ƾ ����
        hideCoroutine = StartCoroutine(HideAccuracyImage());
    }

    // ��Ȯ�� �̹��� ���� �ڷ�ƾ
    public IEnumerator HideAccuracyImage()
    {
        yield return new WaitForSeconds(0.5f);

        // ��Ȯ�� �̹��� ���̵� �ƿ� �� ��Ȱ��ȭ
        this.accuracyImg.DOFade(0f, 0.2f).OnComplete(() =>
        {
            this.accuracyImg.gameObject.SetActive(false);
        });
    }

    // ��Ȯ�� �˾� �ؽ�Ʈ ���� �Լ�
    public void SetAccuracyPopText(string _str, Color _color, int _fontSize)
    {
        PopText_UI_Script.ForActivate_Func(_str, _color, _fontSize, this.transform.position, spawnPos);
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