using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BlockSystem_Manager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static BlockSystem_Manager Instance;

    // 블럭 리스트
    private List<Block_Script> blockList;

    // 초기화 함수
    public void Init_Func()
    {
        // 싱글톤 인스턴스 설정
        Instance = this;

        // 블럭 리스트 초기화
        this.blockList = new List<Block_Script>();

        // 비활성화 함수 호출
        this.Deactivate_Func(true);
    }

    // 활성화 함수
    public void Activate_Func()
    {
        // 활성화 시 필요한 작업 추가 가능
    }

    // 비트 완료 시 호출되는 함수
    public void OnBeatDone_Func()
    {
        // 모든 블럭에 대해 비트 완료 함수 호출
        for (int i = this.blockList.Count - 1; i >= 0; i--)
        {
            Block_Script _block = this.blockList[i];
            _block.OnBeatDone_Func();
        }

        // 아이템과 장애물 스폰 여부 결정
        bool shouldSpawnItem = Random.value < DataBase_Manager.Instance.itemSpawnPer;
        bool shouldSpawnObstacle = Random.value < DataBase_Manager.Instance.obstacleSpawnPer;

        // 스폰 위치 결정
        int _xPos = Random.Range(DataBase_Manager.Instance.mapSpaceMinX, DataBase_Manager.Instance.mapSpaceMaxX + 1);

        // 아이템과 장애물이 모두 스폰되지 않는 경우 아이템을 스폰
        if (!shouldSpawnItem && !shouldSpawnObstacle)
        {
            shouldSpawnItem = true;
        }

        // 아이템 스폰
        if (shouldSpawnItem)
        {
            Block_Script _baseBlock = DataBase_Manager.Instance.baseBlock;
            Block_Script _spawnblock = Instantiate(_baseBlock);

            _spawnblock.Activate_Func(Block_Script.BlockType.Item, _xPos);

            this.blockList.Add(_spawnblock);
        }

        // 장애물 스폰
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

    // 블럭 착지 시 호출되는 함수
    public void OnLanding_Func(Block_Script _block)
    {
        // 블럭 리스트에서 제거
        this.blockList.Remove(_block);
    }

    // 비활성화 함수
    public void Deactivate_Func(bool _isInit = false)
    {
        if (!_isInit)
        {
            // 모든 블럭에 대해 비활성화 함수 호출
            for (int i = this.blockList.Count - 1; i >= 0; i--)
            {
                Block_Script _block = this.blockList[i];
                _block.Deactivate_Func();
            }
        }

        // 블럭 리스트 초기화
        this.blockList.Clear();
    }
}