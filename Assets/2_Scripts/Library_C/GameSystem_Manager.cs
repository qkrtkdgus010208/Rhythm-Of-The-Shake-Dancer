using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Library_C
{
    public abstract class GameSystem_Manager : MonoBehaviour
    {
        [SerializeField] private SoundSystem_Manager soundSystem_Manager = null; // 사운드 시스템 매니저
        [SerializeField] private DataBase_Manager dataBase_Manager = null; // 데이터베이스 시스템 매니저

        private void Awake()
        {
            // 타겟 프레임 레이트 설정
            Application.targetFrameRate = 60;
            // V-Sync 끄기 (프레임 제한을 정확하게 적용하려면 필요)
            QualitySettings.vSyncCount = 0;

            // 초기화 함수 호출
            this.Init_Func();
        }

        // 초기화 함수
        public virtual void Init_Func()
        {
            // 사운드 시스템 초기화
            this.soundSystem_Manager.Init_Func();
            // 데이터베이스 시스템 초기화
            this.dataBase_Manager.Init_Func();

            // 비활성화 함수 호출 (초기화 시)
            this.Deactivate_Func(true);

            // 활성화 함수 호출 (초기화 시)
            this.Activate_Func();
        }

        // 활성화 함수 (상속받은 클래스에서 구현)
        public virtual void Activate_Func()
        {

        }

        // 비활성화 함수
        public virtual void Deactivate_Func(bool _isInit = false)
        {
            // 초기화 시가 아닌 경우에만 실행
            if (_isInit == false)
            {

            }
        }
    }
}