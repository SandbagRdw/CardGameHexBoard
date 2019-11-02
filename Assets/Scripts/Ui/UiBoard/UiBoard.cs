﻿using Game.Ui;
using HexCardGame.Runtime.GameBoard;
using UnityEngine;
using UnityEngine.Tilemaps;
using Logger = Tools.Logger;

namespace HexCardGame.UI
{
    public class UiBoard : UiEventListener, ICreateBoard
    {
        GameObject[] positions;
        [SerializeField] TileBase test;
        IBoard CurrentBoard { get; set; }
        Tilemap TileMap { get; set; }

        void ICreateBoard.OnCreateBoard(IBoard board)
        {
            CurrentBoard = board;
            DrawBoardUi();
        }

        protected override void Awake()
        {
            base.Awake();
            TileMap = GetComponentInChildren<Tilemap>();
        }

        [Button]
        void DrawBoardUi()
        {
            Logger.Log<UiBoard>("Board View Created");
            foreach (var p in CurrentBoard.Positions)
                TileMap.SetTile(p, test);
        }

        [Button]
        void ClearBoardUi() => TileMap.ClearAllTiles();
    }
}