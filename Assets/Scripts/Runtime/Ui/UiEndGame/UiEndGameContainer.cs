﻿using System.Collections;
using Game.Ui;
using HexCardGame.Runtime.Game;
using UnityEngine;

namespace HexCardGame.UI
{
    public interface IRestartGameHandler
    {
        void RestartGame();
    }

    /// <summary> End game HUD. Solves model dependencies accessing the game controller via Singleton. </summary>
    [RequireComponent(typeof(IUiUserInput))]
    public class UiEndGameContainer : UiEventListener, IRestartGameHandler, IFinishGame, IStartGame
    {
        private const float DelayToEnable = 1f;
        private GameData GameData { get; set; }
        private IUiUserInput UserInput { get; set; }

        void IFinishGame.OnFinishGame(IPlayer winner)
        {
            StartCoroutine(EnableInput());
        }

        void IRestartGameHandler.RestartGame()
        {
            GameData.CurrentGameInstance.BattleFsm.Controller.RestartGameImmediately();
        }

        void IStartGame.OnStartGame(IPlayer starter)
        {
            UserInput.Disable();
        }

        protected override void Awake()
        {
            base.Awake();
            GameData = GameData.Load();

            //user input
            UserInput = gameObject.AddComponent<UiUserInput>();

            //HUD end game
            gameObject.AddComponent<UiButtonsEndGame>();
        }

        private IEnumerator EnableInput()
        {
            yield return new WaitForSeconds(DelayToEnable);
            UserInput.Enable();
        }

        public void RestartGame()
        {
        }
    }
}