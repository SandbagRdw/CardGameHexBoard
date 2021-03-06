﻿using HexCardGame.Runtime.Game;
using Tools.Patterns.Observer;
using UnityEngine;

namespace HexCardGame
{
    /// <summary>  Game data. </summary>
    [CreateAssetMenu(menuName = "Variables/GameData")]
    public class GameData : ScriptableObject
    {
        public const string Path = "Variables/GameData";
        private IDispatcher _dispatcher;
        [SerializeField] private GameParameters gameParameters;

        public IGame CurrentGameInstance { get; private set; }

        public static GameData Load()
        {
            return Resources.Load<GameData>(Path);
        }

        /// <summary>  Clears the game data. </summary>
        public void Clear()
        {
            CurrentGameInstance = null;
        }

        public void CreateGame(IGameController controller, IPlayer localPlayer, IPlayer remotePlayer)
        {
            var turnBasedArgs = new RuntimeGame.GameArgs
            {
                Controller = controller,
                Dispatcher = _dispatcher,
                GameParameters = gameParameters,
                LocalPlayer = localPlayer,
                RemotePlayer = remotePlayer
            };
            CurrentGameInstance = new RuntimeGame(turnBasedArgs);
        }

        private void OnEnable()
        {
            if (_dispatcher == null)
                _dispatcher = EventsDispatcher.Load();
        }
    }
}