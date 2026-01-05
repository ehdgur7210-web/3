using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterFactory
{
    //반환값: 생성된 몬스터의 GameObject
    public abstract GameObject CreateMonster(Vector3 position);
}

public class SlimeFactory : MonsterFactory
{
    private GameObject slimePrefab;

    public SlimeFactory(GameObject monsterPrefab)
    {
        this.slimePrefab = monsterPrefab;
    }
    public override GameObject CreateMonster(Vector3 position)
    {
        if (slimePrefab == null)
        {
            Debug.Log("생성불가");
            return null;
        }

        //복사할때 원본 , 위치 , 회전값은고정
        GameObject slime = Object.Instantiate(slimePrefab, position, Quaternion.identity);
        slime.name = "슬라임"; // 오브젝트 이름 설정

        Debug.Log("슬라임이 생성되었습니다!");
        return slime;
    }
}

public class DragonFactory : MonsterFactory
{
    private GameObject DragonPrefabs;

    public DragonFactory(GameObject monsterPrefab)
    {
        this.DragonPrefabs = monsterPrefab;
    }
    public override GameObject CreateMonster(Vector3 position)
    {
        if (DragonPrefabs == null)
        {
            Debug.Log("생성불가");
            return null;
        }

        //복사할때 원본 , 위치 , 회전값은고정
        GameObject Dragon = Object.Instantiate(DragonPrefabs, position, Quaternion.identity);
        Dragon.name = "드래곤"; // 오브젝트 이름 설정

        Debug.Log("드래곤이 생성되었습니다!");
        return Dragon;
    }

}
