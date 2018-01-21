using UnityEngine;
using System.Collections;

namespace Map
{
    /// <summary>
    /// The types of materials available to sectors.
    /// </summary>
    public enum SectorMaterialType
    {
        /// <summary>
        /// The standard material.
        /// </summary>
        Normal,
        /// <summary>
        /// A dark version of the material.
        /// </summary>
        Dark,
        /// <summary>
        /// A bright version of the material.
        /// Includes a glowing effect.
        /// </summary>
        Bright,
        /// <summary>
        /// A dimmed version of the material.
        /// Includes a glowing effect.
        /// </summary>
        Dimmed
    }
}
