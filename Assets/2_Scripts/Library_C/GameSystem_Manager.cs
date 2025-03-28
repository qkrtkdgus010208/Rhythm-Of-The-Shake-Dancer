using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Library_C
{
    public abstract class GameSystem_Manager : MonoBehaviour
    {
        [SerializeField] private SoundSystem_Manager soundSystem_Manager = null; // ���� �ý��� �Ŵ���
        [SerializeField] private DataBase_Manager dataBase_Manager = null; // �����ͺ��̽� �ý��� �Ŵ���

        private void Awake()
        {
            // Ÿ�� ������ ����Ʈ ����
            Application.targetFrameRate = 60;
            // V-Sync ���� (������ ������ ��Ȯ�ϰ� �����Ϸ��� �ʿ�)
            QualitySettings.vSyncCount = 0;

            // �ʱ�ȭ �Լ� ȣ��
            this.Init_Func();
        }

        // �ʱ�ȭ �Լ�
        public virtual void Init_Func()
        {
            // ���� �ý��� �ʱ�ȭ
            this.soundSystem_Manager.Init_Func();
            // �����ͺ��̽� �ý��� �ʱ�ȭ
            this.dataBase_Manager.Init_Func();

            // ��Ȱ��ȭ �Լ� ȣ�� (�ʱ�ȭ ��)
            this.Deactivate_Func(true);

            // Ȱ��ȭ �Լ� ȣ�� (�ʱ�ȭ ��)
            this.Activate_Func();
        }

        // Ȱ��ȭ �Լ� (��ӹ��� Ŭ�������� ����)
        public virtual void Activate_Func()
        {

        }

        // ��Ȱ��ȭ �Լ�
        public virtual void Deactivate_Func(bool _isInit = false)
        {
            // �ʱ�ȭ �ð� �ƴ� ��쿡�� ����
            if (_isInit == false)
            {

            }
        }
    }
}