using UnityEngine;

public partial class DataBase_Manager : DB_Manager
{
    // 맵의 최소, 최대 X 좌표와 최대 Y 좌표
    public int mapSpaceMinX = -2;
    public int mapSpaceMaxX = 2;
    public int mapSpaceMaxY = 7;
    // 게임 플레이 시간과 목표 점수
    public float playTime = 60f;
    public int goalScore = 10000;

    [Header("텍스트 출력")]
    // 팝업 텍스트와 텍스트 출력 간격
    public PopText_Script basePopText = null;
    public float popTextInterval = 0.1f;

    [Header("텍스트 출력(UI)")]
    // UI 팝업 텍스트
    public PopText_UI_Script basePopTextUI = null;

    [Header("비트")]
    // 비트 간격과 실패 시 색상 및 텍스트 설정
    public float beatInterval = 1f;
    public Color beatFailColor = Color.clear;
    public Color beatFailPopTextColor = Color.clear;
    public string beatFailPopTextStr = string.Empty;
    public int beatFailPoptextFontSize = 0;
    // 비트 정확도 데이터 배열
    public BeatAccuracyData[] beatAccuracyDataArr = null;

    [Header("캐릭터")]
    // 캐릭터 점프 파워와 지속 시간
    public float JumpPower = 1f;
    public float JumpDuration = 0.5f;

    [Header("블럭")]
    // 블럭 드롭 지속 시간과 기본 블럭, 아이템 블럭, 장애물 블럭 스프라이트
    public float blockDropDuration = 0.5f;
    public Block_Script baseBlock = null;
    public Sprite itemBlockSprite = null;
    public Sprite obstacleBlockSprite = null;
    // 아이템과 장애물 스폰 확률, 아이템 점수
    public float itemSpawnPer = 0.5f;
    public float obstacleSpawnPer = 0.5f;
    public int itemScore = 100;

    // 블럭 타입에 따른 스프라이트 반환 함수
    public Sprite GetBlockSprite_Func(Block_Script.BlockType _blockType)
    {
        switch (_blockType)
        {
            case Block_Script.BlockType.Item: return this.itemBlockSprite;
            case Block_Script.BlockType.Obstacle: return this.obstacleBlockSprite;

            default:
                Debug.LogError("_blockType : " + _blockType);
                return null;
        }
    }

    // 비트 정확도 데이터 구조체
    [System.Serializable]
    public struct BeatAccuracyData
    {
        public float accuracyValue;
        public float bonusValue;
        public Color AccuracyImgColor;

        public Color popTextColor;
        public string popTextStr;
        public int fontSize;
    }
}