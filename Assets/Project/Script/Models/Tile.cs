using UnityEngine;

namespace Gazeus.DesafioMatch3.Models
{
    public class Tile
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public Vector2Int GridPosition { get; set; }

        public bool IsStripe { get; set; } = false;
        public bool IsWrapper { get; set; } = false;
        public bool IsBomb { get; set; } = false;
        public bool StripeX { get; set; } = true;

        public const int DEAD_ZONE_TYPE_INDEX = 6;

        public void SetToDeadzone()
        {
            Type = DEAD_ZONE_TYPE_INDEX;
        }

        public bool IsDeadZoneType()
        {
            return Type == DEAD_ZONE_TYPE_INDEX;
        }

        public bool IsSpecial()
        {
            return IsStripe || IsWrapper || IsBomb;
        }

        public bool CanMatch(Tile _otherTile)
        {
            if (IsDeadZoneType() || _otherTile.IsDeadZoneType())
            {
                return false;
            }

            return Type == _otherTile.Type;
        }
    }
}
