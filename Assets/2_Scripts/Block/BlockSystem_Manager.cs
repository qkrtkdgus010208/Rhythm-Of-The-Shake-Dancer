using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BlockSystem_Manager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static BlockSystem_Manager Instance;

    // �� ����Ʈ
    private List<Block_Script> blockList;

    // �ʱ�ȭ �Լ�
    public void Init_Func()
    {
        // �̱��� �ν��Ͻ� ����
        Instance = this;

        // �� ����Ʈ �ʱ�ȭ
        this.blockList = new List<Block_Script>();

        // ��Ȱ��ȭ �Լ� ȣ��
        this.Deactivate_Func(true);
    }

    // Ȱ��ȭ �Լ�
    public void Activate_Func()
    {
        // Ȱ��ȭ �� �ʿ��� �۾� �߰� ����
    }

    // ��Ʈ �Ϸ� �� ȣ��Ǵ� �Լ�
    public void OnBeatDone_Func()
    {
        // ��� ���� ���� ��Ʈ �Ϸ� �Լ� ȣ��
        for (int i = this.blockList.Count - 1; i >= 0; i--)
        {
            Block_Script _block = this.blockList[i];
            _block.OnBeatDone_Func();
        }

        // �����۰� ��ֹ� ���� ���� ����
        bool shouldSpawnItem = Random.value < DataBase_Manager.Instance.itemSpawnPer;
        bool shouldSpawnObstacle = Random.value < DataBase_Manager.Instance.obstacleSpawnPer;

        // ���� ��ġ ����
        int _xPos = Random.Range(DataBase_Manager.Instance.mapSpaceMinX, DataBase_Manager.Instance.mapSpaceMaxX + 1);

        // �����۰� ��ֹ��� ��� �������� �ʴ� ��� �������� ����
        if (!shouldSpawnItem && !shouldSpawnObstacle)
        {
            shouldSpawnItem = true;
        }

        // ������ ����
        if (shouldSpawnItem)
        {
            Block_Script _baseBlock = DataBase_Manager.Instance.baseBlock;
            Block_Script _spawnblock = Instantiate(_baseBlock);

            _spawnblock.Activate_Func(Block_Script.BlockType.Item, _xPos);

            this.blockList.Add(_spawnblock);
        }

        // ��ֹ� ����
        if (shouldSpawnObstacle)
        {
            Block_Script _baseBlock = DataBase_Manager.Instance.baseBlock;
            Block_Script _spawnblock = Instantiate(_baseBlock);

            _xPos += Random.value > 0.5f ? 1 : -1;
            _xPos %= DataBase_Manager.Instance.mapSpaceMaxX + 1;

            _spawnblock.Activate_Func(Block_Script.BlockType.Obstacle, _xPos);

            this.blockList.Add(_spawnblock);
        }
    }

    // �� ���� �� ȣ��Ǵ� �Լ�
    public void OnLanding_Func(Block_Script _block)
    {
        // �� ����Ʈ���� ����
        this.blockList.Remove(_block);
    }

    // ��Ȱ��ȭ �Լ�
    public void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            // ��� ���� ���� ��Ȱ��ȭ �Լ� ȣ��
            for (int i = this.blockList.Count - 1; i >= 0; i--)
            {
                Block_Script _block = this.blockList[i];
                _block.Deactivate_Func();
            }
        }

        // �� ����Ʈ �ʱ�ȭ
        this.blockList.Clear();
    }
}