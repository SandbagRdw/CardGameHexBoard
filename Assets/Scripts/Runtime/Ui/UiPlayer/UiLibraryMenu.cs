﻿using Game.Ui;
using UnityEngine;

namespace HexCardGame.UI
{
    public class UiLibraryMenu : UiEventListener, UiButtonReveal.IPressReveal, UiButtonDraw.IPressDraw,
        UiButtonCloseLibrary.IPressCloseLibrary
    {
        [SerializeField] private GameObject buttonClose;
        [SerializeField] private GameObject content;

        void UiButtonCloseLibrary.IPressCloseLibrary.CloseLibrary()
        {
            Hide();
        }

        void UiButtonDraw.IPressDraw.Draw()
        {
            GameData.CurrentGameInstance.DrawCardFromLibrary(SeatType.Bottom);
            Hide();
        }

        void UiButtonReveal.IPressReveal.Reveal()
        {
            GameData.CurrentGameInstance.RevealCardHigherPosition(SeatType.Bottom);
            Hide();
        }

        public void Show()
        {
            content.SetActive(true);
            buttonClose.SetActive(true);
        }

        public void Hide()
        {
            content.SetActive(false);
            buttonClose.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();
            var buttons = GetComponentsInChildren<UiButton>();
            foreach (var i in buttons)
                i.SetHandler(this);
            Hide();
        }
    }
}