using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public partial class DataBase_Manager : DB_Manager
{
    public static DataBase_Manager Instance;

    // 초기화 함수
    public override void Init_Func()
    {
        // 싱글톤 인스턴스 설정
        Instance = this;

        // 부모 클래스의 초기화 함수 호출
        base.Init_Func();
    }
}