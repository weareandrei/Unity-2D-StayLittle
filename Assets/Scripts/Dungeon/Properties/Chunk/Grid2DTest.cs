using Dungeon.Generator;
using UnityEngine;

namespace Grid2DEditor {
    public class Grid2DTest: MonoBehaviour {
        [SerializeField] public Grid2D grid;

        public Grid2DTest() {
            grid = new Grid2D(Consts.ChunkSize);
        }
    }
}