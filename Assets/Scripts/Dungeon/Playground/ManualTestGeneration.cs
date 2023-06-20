using Global;
using UnityEngine;
using Manager.SubManager;

public class ManualTestGeneration : MonoBehaviour
{
    void Start() {
        GlobalVariables.environment = "DEV";
        DungeonManager.Initialize("3962420980463");
        DungeonManager.RenderDungeonsAll();
    }
}
