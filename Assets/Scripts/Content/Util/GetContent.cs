using System.Collections.Generic;
using Content.Base;
using Global;
using UnityEngine;

namespace Content.Util {
    
    public static class GetContent {
        
        public static GameObject GetPrefabByID(string id) {
            string resourcesDirectory = GlobalVariables.ResourcesDirectory;
            string path = resourcesDirectory + "Content/Prefab";
            
            GameObject[] contentPrefabs = Resources.LoadAll<GameObject>(path);
            foreach (GameObject contentPrefab in contentPrefabs) {
                BaseContent contentComponent = contentPrefab.GetComponent<BaseContent>();

                if (contentComponent != null && contentComponent.id == id) {
                    return contentPrefab;
                }
            }

            return null;
        }
        
        public static List<GameObject> GetPrefabsByType(ContentType type) {
            string resourcesDirectory = GlobalVariables.ResourcesDirectory;
            string path = resourcesDirectory + "Content/Prefab";
            
            GameObject[] contentPrefabs = Resources.LoadAll<GameObject>(path);
            List<GameObject> contentPrefabsAvailable = new List<GameObject>();
            foreach (GameObject contentPrefab in contentPrefabs) {
                BaseContent contentComponent = contentPrefab.GetComponent<BaseContent>();

                if (contentComponent != null && contentComponent.type == type) {
                    contentPrefabsAvailable.Add(contentPrefab);
                }
            }

            return contentPrefabsAvailable;
        }
        
    }
}