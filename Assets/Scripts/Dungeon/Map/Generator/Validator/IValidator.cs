using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.MapGenerator
{
    public interface IMapValidator
    {
        public bool Check(MapLayout layout);
    }
}
