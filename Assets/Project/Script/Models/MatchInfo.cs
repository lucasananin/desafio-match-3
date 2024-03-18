using Gazeus.DesafioMatch3.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Models
{
    public class MatchInfo
    {
        public Tile Tile { get; private set; } = null;
        public bool IsMatch { get; private set; } = false;

        public void Setup(Tile _tile, bool _isMatch)
        {
            Tile = _tile;

            if (Tile.IsDeadZoneType())
            {
                IsMatch = false;
                return;
            }
            else
            {
                IsMatch = _isMatch;
            }
        }
    }
}
