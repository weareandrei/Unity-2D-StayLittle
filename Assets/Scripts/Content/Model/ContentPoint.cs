using System;
using UnityEngine;

namespace Content {
    public class ContentPoint : MonoBehaviour {
        [SerializeField]
        public ContentType type;
        public ContentPayload payload;
        // public ContentSpawner spawner;
        // todo: I've decided to use this as a static class and pass
        // todo:   all the required data to it every time
    }
}