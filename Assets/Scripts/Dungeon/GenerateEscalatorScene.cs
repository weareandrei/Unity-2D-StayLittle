using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEscalatorScene : MonoBehaviour
{

    [SerializeField] private float maxDistanceElevatorTravel = 500f;
    private void Start() {
        // select randomly
        int randomInt = Random.Range(0, DungeonsData.countDungeons()-1);
        DungeonsData._dungeonData dungeon = DungeonsData.getDungeonDataByIndex(randomInt);
        Debug.Log("Chosen : " + dungeon.id);
        int stopPositionY = dungeon.depthLevel;

        List<DungeonsData._dungeonData> dungeonsNearTargetDungeon = new List<DungeonsData._dungeonData>();

        if (randomInt < DungeonsData.countDungeons()-1) {
            dungeonsNearTargetDungeon.Add(DungeonsData.getDungeonDataByIndex(randomInt+1));
        }

        for (int i = randomInt; i >= 0; i--) {
            if (dungeonsNearTargetDungeon.Count < 5) {
                if ((dungeon.depthLevel - DungeonsData.getDungeonDataByIndex(randomInt+1).depthLevel) < maxDistanceElevatorTravel) {
                    dungeonsNearTargetDungeon.Add(DungeonsData.getDungeonDataByIndex(i));
                }
            }
        }

        printList(dungeonsNearTargetDungeon);

        // Now we have a list of dungeons from the target.

        // Now we have them as Scenes (should have generated scenes in GenerateDungeons for each Dungeon)
        //  so we additively load each scene to make it visible on it's own position.
        //  Or no? Maybe they all already have their positions. and I just need to load and thats it.
        // THEN start the elevator movement until the point.
    }

    private void printList(List<DungeonsData._dungeonData> dungeonList) {
        foreach(DungeonsData._dungeonData dungeonData in dungeonList) {
            Debug.Log(dungeonData.id);
        }
    }
}
