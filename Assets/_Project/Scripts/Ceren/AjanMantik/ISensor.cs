using System.Collections.Generic;
using UnityEngine;

namespace MuzeSoygunu.Ceren
{
    public interface ISensor
    {
        IReadOnlyList<SensorOlcumu> Olc(Vector2 yon);
    }
}