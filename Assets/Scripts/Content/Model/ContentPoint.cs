using System;
using UnityEngine;

namespace Content {
    public class ContentPoint : MonoBehaviour {
        [SerializeField]
        public ContentPointData data;

        public ContentPointData GetContentPointData() {
            return data;
        }
        
    }
}