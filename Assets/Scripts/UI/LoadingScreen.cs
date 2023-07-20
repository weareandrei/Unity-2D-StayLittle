using System;
using UnityEngine;

namespace UI {
    public class LoadingScreen : MonoBehaviour {
        private void Awake() {
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}