using System.Collections.Generic;
using System.Linq;
using Content;
using Content.Base;
using Content.Util;
using UnityEngine;

namespace Manager.SubManager {
    
    public static class ContentManager {
        
        public static List<string> GetAvailableContentIDs(ContentType type) {
            return  GetContent.GetPrefabsByType(type)
                .Select(prefab => prefab.GetComponent<BaseContent>().id).ToList();
        }
        
    }
    
}