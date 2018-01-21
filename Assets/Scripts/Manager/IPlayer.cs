using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public interface IPlayer
    {
        /// <summary>
        /// The ID of the current player.
        /// </summary>
        int Id { get; }
        /// <summary>
        /// The mana of the player.
        /// </summary>
        int Mana { get; set; }
        /// <summary>
        /// The maximum available mana of the player.
        /// </summary>
        int MaxMana { get; set; }
        /// <summary>
        /// Whether the player should skip their movement phase.
        /// </summary>
        bool ShouldSkipMovePhase { get; set; }
        /// <summary>
        /// Whether the player should skip their attack phase.
        /// </summary>
        bool ShouldSkipAttackPhase { get; set; }
        /// <summary>
        /// Checks if a player is Equal to another.
        /// </summary>
        /// <param name="other">The other player to check.</param>
        /// <returns>Whether they are equal.</returns>
        bool Equals(IPlayer other);
    }
}
