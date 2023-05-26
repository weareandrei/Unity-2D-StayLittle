using UnityEngine;

public class TestGeneration : MonoBehaviour
{
    void Start()
    {
        Managers.DungeonManager.Initialize("3962420980463");
        Managers.DungeonManager.RenderDungeonsAll();
    }
}
