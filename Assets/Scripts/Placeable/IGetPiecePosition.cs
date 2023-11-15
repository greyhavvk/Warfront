using System.Collections.Generic;
using UnityEngine;

namespace Placeable
{
    public interface IGetPiecePosition
    {
        List<Transform> Pieces { get;}
    }
}