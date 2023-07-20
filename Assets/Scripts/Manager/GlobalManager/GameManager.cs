using System;
using UI;
using UnityEngine;
using Util;

namespace Manager {
    public static class GameManager/* : Singleton<GameManager>*/ {
        
        [SerializeField] public static GameState State { get; private set; }
        
        public static event Action<GameState> OnBeforeStateChanged;
        public static event Action<GameState> OnAfterStateChanged;

        public static void ChangeState(GameState newState) {
            OnBeforeStateChanged?.Invoke(newState);

            State = newState;
            switch (newState) {
                case GameState.Loading:
                    HandleLoading();
                    break;
                case GameState.Playing:
                    // HandlePlaying();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            OnAfterStateChanged?.Invoke(newState);
        }

        private static void HandleLoading() {
            // LoadingManager.StartLoading();
        }
    }
}

public enum GameState {
    Loading = 0,
    Playing = 1
}