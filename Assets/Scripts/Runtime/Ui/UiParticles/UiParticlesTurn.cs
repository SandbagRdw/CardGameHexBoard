﻿using HexCardGame.Runtime.Game;
using UnityEngine;

namespace HexCardGame.UI
{
    public class UiParticlesTurn : UiParticles, IStartPlayerTurn
    {
        [SerializeField] private SeatType id;

        void IStartPlayerTurn.OnStartPlayerTurn(IPlayer player)
        {
            if (player.Seat == id)
                StartCoroutine(Play());
        }
    }
}