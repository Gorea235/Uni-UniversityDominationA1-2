using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Map.Hex;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// A landmark that is on the current sector.
    /// </summary>
    public interface ILandmark
    {
        /// <summary>
        /// The bonus to give when the landmark is landed on.
        /// </summary>
        /// <param name="player">The player to give the bonus to.</param>
        /// <param name="coord">The coordinate of the landmark.</param>
        /// <param name="context">The current <see cref="Context"/> object.</param>
        void StartBonus(IPlayer player, Coord coord, Context context);
        /// <summary>
        /// The bonus to give on the start of every turm.
        /// </summary>
        /// <param name="player">The player to give the bonus to.</param>
        /// <param name="coord">The coordinate of the landmark.</param>
        /// <param name="context">The current <see cref="Context"/> object.</param>
        void TurnBonus(IPlayer player, Coord coord, Context context);
        /// <summary>
        /// The current <see cref="GameObject.transform"/> object.
        /// </summary>
        Transform Transform { get; }
    }
}
