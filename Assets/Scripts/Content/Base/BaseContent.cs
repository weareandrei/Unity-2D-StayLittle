using System;
using UnityEngine;

namespace Content.Base {
    
    [Serializable]
    public class BaseContent : MonoBehaviour {
        [SerializeField]
        public string id;
        [SerializeField]
        public ContentType type;
        // Some data about the Content as well as Base methods the Content can use. 
    }
}