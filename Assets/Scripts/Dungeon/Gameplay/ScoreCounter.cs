using System;
using UnityEngine;

namespace Dungeon.Gameplay {
    [Serializable]
    public class ScoreCounter {
        [SerializeField] public int pointsEarned = 0;
        [SerializeField] public int mobsKilled = 0;
    }
}