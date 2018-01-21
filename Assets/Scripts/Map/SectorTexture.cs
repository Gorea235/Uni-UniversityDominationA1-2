using UnityEngine;
using System.Collections;

namespace Map
{
    /// <summary>
    /// The textures available to sectors.
    /// </summary>
    public enum SectorTexture
    {
        // ground types
        Grass,
        Water, // set up to be non traversable by default
        Stones,
        Concrete,
        // college types
        Alcuin,
        Constantine,
        Derwent,
        Goodricke,
        Halifax,
        James,
        Langwith,
        Vanbrugh,
        Wentworth
    }
}
