﻿using UnityEngine;

namespace HexCardGame.Runtime.GameBoard
{
    public interface IBoardDataStorage<T> where T : class
    {
        T GetDataFrom(Vector2Int vPosition);
        void AddDataAt(T data, Vector2Int position);
        bool RemoveDataAt(Vector2Int vPosition);
        bool HasDataAt(Vector2Int position);
        bool RemoveData(T data);
        bool HasData(T data);
        void Clear();
    }

    public partial class Board<T>
    {
        public T GetDataFrom(Vector2Int vPosition) => GetPosition(vPosition).Data;

        public void AddDataAt(T data, Vector2Int position) => GetPosition(position).SetData(data);

        public bool RemoveDataAt(Vector2Int vPosition)
        {
            var position = GetPosition(vPosition);
            var data = position.Data;
            position.SetData(null);
            return data != null;
        }

        public bool HasDataAt(Vector2Int position) => GetPosition(position).HasData;

        public bool RemoveData(T data)
        {
            foreach (var i in Positions)
            {
                if (!i.HasData) continue;
                if (i.Data != data) continue;

                i.SetData(null);
                return true;
            }

            return false;
        }

        public bool HasData(T data)
        {
            foreach (var i in Positions)
            {
                if (!i.HasData) continue;
                if (i.Data != data) continue;
                return true;
            }

            return false;
        }
    }
}