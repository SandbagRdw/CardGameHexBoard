﻿using System.Collections.Generic;
using Game.Ui;
using HexCardGame.Runtime;
using HexCardGame.Runtime.Game;
using HexCardGame.Runtime.GamePool;
using Tools.UI.Card;
using UnityEngine;
using Logger = Tools.Logger;

namespace HexCardGame.UI
{
    public class UiHand : UiEventListener, IDrawCard, ICreateBoardElement, ISelectBoardPosition, IRestartGame, ISelectPoolPosition
    {
        readonly Dictionary<IUiCard, CardHand> _cards = new Dictionary<IUiCard, CardHand>();
        [SerializeField] UiCardHand cardHand;
        [SerializeField] [Tooltip("Prefab of the Card")]
        GameObject cardPrefab;

        [SerializeField] [Tooltip("World point where the deck is positioned")]
        Transform deckPosition;

        [SerializeField] PlayerId id;
        public CardHand SelectedCard { get; private set; }

        void ICreateBoardElement.OnCreateBoardElement(PlayerId id, BoardElement boardElement, Vector3Int position, CardHand card)
        {
            if (id != this.id) 
                return;
            cardHand.PlaySelected();
            RemoveCard(card);
        }

        void IDrawCard.OnDrawCard(PlayerId id, CardHand card)
        {
            if (this.id == id)
                _cards.Add(GetCard(), card);
        }

        void ISelectBoardPosition.OnSelectPosition(Vector3Int position)
        {
            if (SelectedCard == null)
                return;
            
            GameData.CurrentGameInstance.PlayElementAt(id, SelectedCard, position);
            SelectedCard = null;
        }

        protected override void Awake()
        {
            base.Awake();
            cardHand.OnCardSelected += SelectCard;
            cardHand.OnCardUnSelect += Unselect;
        }


        [Button]
        public IUiCard GetCard()
        {
            var uiCard = ObjectPooler.Instance.Get<IUiCard>(cardPrefab);
            uiCard.transform.SetParent(cardHand.transform);
            uiCard.transform.position = deckPosition.position;
            uiCard.Initialize();
            cardHand.AddCard(uiCard);
            return uiCard;
        }


        void RemoveCard(CardHand card)
        {
            IUiCard removed = null;
            foreach (var key in _cards.Keys)
                if (_cards[key] == card)
                    removed = key;

            if (removed != null)
                _cards.Remove(removed);

            ObjectPooler.Instance.Release(removed?.gameObject);
        }

        void SelectCard(IUiCard uiCard) => SelectedCard = _cards[uiCard];
        void Unselect() => SelectedCard = null;
        void IRestartGame.OnRestart() => Clear();

        void Clear()
        {
            _cards.Clear();
            SelectedCard = null;
        }

        void ISelectPoolPosition.OnSelectPoolPosition(PlayerId playerId, PositionId positionId)
        {
        }
    }
}
