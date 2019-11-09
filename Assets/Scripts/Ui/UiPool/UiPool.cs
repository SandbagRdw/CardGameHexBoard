﻿using Game.Ui;
using HexCardGame.Runtime;
using HexCardGame.Runtime.Game;
using HexCardGame.Runtime.GamePool;
using UnityEngine;
using Logger = Tools.Logger;

namespace HexCardGame.UI
{
    public class UiPool : UiEventListener, IRestartGame, ISelectPoolPosition, ICreatePool<CardPool>,
        IPickCard, IReturnCard,
        IRevealCard, IRevealPool
    {
        UiPoolPositioning _positioning;
        [SerializeField] Transform deckPosition;
        [SerializeField] UiPoolParameters parameters;
        [SerializeField] UiPoolPosition[] poolCardPositions;
        IPool<CardPool> CurrentPool { get; set; }

        void ICreatePool<CardPool>.OnCreatePool(IPool<CardPool> pool) => CurrentPool = pool;

        void IPickCard.OnPickCard(PlayerId id, CardHand card, PositionId positionId)
        {
            Logger.Log<UiPool>("pick Card Received", Color.blue);
            var position = GetPosition(positionId);
            if (!position.HasData)
                return;
            position.Clear();
        }

        void IRestartGame.OnRestart() => Clear();

        void IReturnCard.OnReturnCard(PlayerId id, CardHand cardHand, PositionId positionId) =>
            Logger.Log<UiPool>("Return Card Received", Color.blue);

        void IRevealCard.OnRevealCard(PlayerId id, CardPool cardPool, PositionId positionId)
        {
            Logger.Log<UiPool>("Reveal Card received", Color.blue);
            AddCard(cardPool, positionId, CurrentPool.IsPositionLocked(positionId));
        }

        void IRevealPool.OnRevealPool(IPool<CardPool> pool)
        {
            Logger.Log<UiPool>("On Reveal Pool received", Color.blue);

            var positions = PoolPositionUtility.GetAllIndices();
            foreach (var i in positions)
            {
                var cardPool = pool.GetCardAt(i);
                var isLocked = pool.IsPositionLocked(i);
                AddCard(cardPool, i, isLocked);
            }
        }

        void ISelectPoolPosition.OnSelectPoolPosition(PlayerId playerId, PositionId positionId) =>
            GameData.CurrentGameInstance.PickCardFromPosition(PlayerId.User, positionId);

        void Clear()
        {
            CurrentPool = null;
            foreach (var i in poolCardPositions)
                i.Clear();
        }

        void AddCard(CardPool cardPool, PositionId positionId, bool isLocked)
        {
            var uiPosition = GetPosition(positionId);
            var template = parameters.UiCardPoolTemplate.gameObject;
            var uiCard = ObjectPooler.Instance.Get<UiCardPool>(template);
            if (isLocked)
                uiCard.SetColor(parameters.Locked);
            uiCard.transform.position = deckPosition.transform.position;
            uiCard.transform.SetParent(uiPosition.transform);
            uiPosition.SetData(uiCard);
        }

        public UiPoolPosition GetPosition(PositionId positionId)
        {
            foreach (var i in poolCardPositions)
                if (i.Id == positionId)
                    return i;
            return null;
        }

        protected override void Awake()
        {
            base.Awake();
            _positioning = new UiPoolPositioning(this, parameters);
            UpdatePositions();
        }

        void UpdatePositions()
        {
            var positions = PoolPositionUtility.GetAllIndices();
            foreach (var i in positions)
            {
                var position = GetPosition(i);
                position.transform.position = _positioning.GetPositionFor(i);
            }
        }

        void Update()
        {
            _positioning.Update();
            UpdatePositions();
        }
    }
}