using System;
using UnityEngine;

namespace Content {
    public class ContentPoint : MonoBehaviour {
        [SerializeField]
        public ContentType type;
        public ContentPayload payload;

        // public ContentPointData GetContentPointData() {
        //     return data;
        // }
        
    }
}