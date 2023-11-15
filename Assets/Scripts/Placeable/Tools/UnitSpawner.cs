using System.Linq;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Placeable.Tools
{
    public static class UnitSpawner
    {
        public static void SpawnUnit(Vector2[] spawnPoints , int lvl)
        {
            var spawnPointList = spawnPoints.ToList();
            var possibleSpawnPoints = GridManager.GetPart.FindEmptyPoints(spawnPointList);

            if (possibleSpawnPoints==null)
            {
                return;
            }
            if (possibleSpawnPoints.Count<= 0)
                return;

            var spawnPoint = possibleSpawnPoints[Random.Range(0,possibleSpawnPoints.Count-1)];
            if (spawnPoint==null)
            {
                return;
            }

            var unit = PlaceableFactory.Instance.InstantPopUnit();
            unit.transform.position = spawnPoint.Transform.position;
            unit.SetLevel(lvl);
            unit.Placed();
            var position = unit.transform.position;
                var gridPart = GridManager.GetPart.GetGridPart(position);
                gridPart.Empty = false;
                gridPart.Unit = unit.transform;
                gridPart.PiecePosition = unit;
        }
    }
}