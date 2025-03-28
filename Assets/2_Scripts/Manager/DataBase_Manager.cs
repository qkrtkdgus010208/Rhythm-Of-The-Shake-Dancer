using UnityEngine;

public partial class DataBase_Manager : DB_Manager
{
    // ���� �ּ�, �ִ� X ��ǥ�� �ִ� Y ��ǥ
    public int mapSpaceMinX = -2;
    public int mapSpaceMaxX = 2;
    public int mapSpaceMaxY = 7;
    // ���� �÷��� �ð��� ��ǥ ����
    public float playTime = 60f;
    public int goalScore = 10000;

    [Header("�ؽ�Ʈ ���")]
    // �˾� �ؽ�Ʈ�� �ؽ�Ʈ ��� ����
    public PopText_Script basePopText = null;
    public float popTextInterval = 0.1f;

    [Header("�ؽ�Ʈ ���(UI)")]
    // UI �˾� �ؽ�Ʈ
    public PopText_UI_Script basePopTextUI = null;

    [Header("��Ʈ")]
    // ��Ʈ ���ݰ� ���� �� ���� �� �ؽ�Ʈ ����
    public float beatInterval = 1f;
    public Color beatFailColor = Color.clear;
    public Color beatFailPopTextColor = Color.clear;
    public string beatFailPopTextStr = string.Empty;
    public int beatFailPoptextFontSize = 0;
    // ��Ʈ ��Ȯ�� ������ �迭
    public BeatAccuracyData[] beatAccuracyDataArr = null;

    [Header("ĳ����")]
    // ĳ���� ���� �Ŀ��� ���� �ð�
    public float JumpPower = 1f;
    public float JumpDuration = 0.5f;

    [Header("��")]
    // �� ��� ���� �ð��� �⺻ ��, ������ ��, ��ֹ� �� ��������Ʈ
    public float blockDropDuration = 0.5f;
    public Block_Script baseBlock = null;
    public Sprite itemBlockSprite = null;
    public Sprite obstacleBlockSprite = null;
    // �����۰� ��ֹ� ���� Ȯ��, ������ ����
    public float itemSpawnPer = 0.5f;
    public float obstacleSpawnPer = 0.5f;
    public int itemScore = 100;

    // �� Ÿ�Կ� ���� ��������Ʈ ��ȯ �Լ�
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

    // ��Ʈ ��Ȯ�� ������ ����ü
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