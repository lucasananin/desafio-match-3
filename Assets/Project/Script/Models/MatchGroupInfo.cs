using Gazeus.DesafioMatch3.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class MatchGroupInfo
    {
        public List<List<Tile>> MatchGroups { get; set; } = new List<List<Tile>>();

        public void AddGroup(List<Tile> _tiles)
        {
            MatchGroups.Add(_tiles);
        }

        public bool Contains(Tile _tile)
        {
            int _countY = MatchGroups.Count;

            for (int y = 0; y < _countY; y++)
            {
                int _countX = MatchGroups[y].Count;

                for (int x = 0; x < _countX; x++)
                {
                    if (MatchGroups[y].Contains(_tile))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
