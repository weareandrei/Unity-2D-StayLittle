using Global;
using UnityEngine;

namespace Dungeon.Renderer {
    
    public static class ShaftRenderer {
        
        public static void Generate() {
            string resourcesDirectory = GlobalVariables.ResourcesDirectory;
            string path = resourcesDirectory + "Dungeon/Shaft/Prefab/ShaftPrefab";
            GameObject shaftPrefab = Resources.Load<GameObject>(path);

            Vector2 startCoordinates = new Vector2(0, 0);
            
            // Get the size of the prefab
            Vector3 prefabSize = GetPrefabSize(shaftPrefab);

            // Check if the prefabSize is valid
            if (prefabSize == Vector3.zero) {
                Debug.LogError("ShaftPrefab size cannot be determined. Make sure it has a valid mesh renderer.");
                return;
            }

            // Calculate the distance between prefabs based on the prefab's height
            float distanceBetweenPrefabs = prefabSize.y;

            int numberOfPrefabs = 200; // Adjust this value to change the number of prefabs you want to create

            for (int i = 0; i < numberOfPrefabs; i++) {
                Vector2 position = startCoordinates + Vector2.down * i * distanceBetweenPrefabs;
                InstantiateShaftPrefab(shaftPrefab, position);
            }
        }
        
        private static Vector2 GetPrefabSize(GameObject prefab) {
            Vector2 size = Vector2.zero;
            SpriteRenderer spriteRenderer = prefab.GetComponentInChildren<SpriteRenderer>();

            if (spriteRenderer != null) {
                size = spriteRenderer.bounds.size;
            } else {
                Debug.LogWarning("Prefab size cannot be determined. Make sure it has a child GameObject with a valid Sprite Renderer component.");
            }

            return size * (float) 0.9f;
        }

        private static void InstantiateShaftPrefab(GameObject prefab, Vector2 position) {
            // Make sure the prefab is not null before instantiating it
            if (prefab == null) {
                Debug.LogError("ShaftPrefab is null. Make sure it is assigned correctly in the Resources folder.");
                return;
            }

            // Instantiate the prefab at the given position
            GameObject.Instantiate(prefab, position, Quaternion.identity);
        }


    }
    
}