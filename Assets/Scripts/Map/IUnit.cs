using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public interface IUnit
    {
        /// <summary>
        /// Gets or sets the health of the unit.
        /// </summary>
        /// <value>The health of the unit.</value>
        int Health { get; set; }
        /// <summary>
        /// Gets the attack value of the unit.
        /// If 0, then it cannot attack.
        /// </summary>
        /// <value>The attack value.</value>
        /// <remarks>The is the damage the unit can cause to other units.</remarks>
        int Attack { get; }
        /// <summary>
        /// Gets the max move range of the unit.
        /// </summary>
        /// <value>The max move range.</value>
        int MaxMove { get; }
        /// <summary>
        /// Gets or sets the available move range of the unit.
        /// </summary>
        /// <value>The available move range.</value>
        int AvailableMove { get; set; }
        /// <summary>
        /// Gets the defence value of the unit.
        /// </summary>
        /// <value>The defence value.</value>
        /// <remarks>
        /// This is the amount of damage mitigation that the unit has, the
        /// higher the defence, the less damage the unit will take for a given
        /// attack.
        /// </remarks>
        int Defence { get; }
        /// <summary>
        /// Gets the build range of the unit.
        /// If 0, then it cannot build.
        /// </summary>
        /// <value>The build range.</value>
        int BuildRange { get; }
        /// <summary>
        /// Gets a value indicating whether the unit is buildable via builder
        /// units.
        /// </summary>
        /// <value><c>true</c> if buildable; otherwise, <c>false</c>.</value>
        bool Buildable { get; }
        /// <summary>
        /// The player that owns the unit.
        /// </summary>
        /// <value>The owner.</value>
        Manager.IPlayer Owner { get; }
        /// <summary>
        /// The college of the unit.
        /// </summary>
        /// <value>The college.</value>
        College College { get; }
        /// <summary>
        /// Returns the GameObject transform property.
        /// </summary>
        /// <value>The transform property.</value>
        Transform Transform { get; }
        /// <summary>
        /// Initialises the unit with the given colour, player and college.
        /// </summary>
        /// <param name="unitColor">The unit colour.</param>
        /// <param name="player">The owning player.</param>
        /// <param name="college">The college the unit is from.</param>
        void Init(SectorMaterials unitColor, Manager.IPlayer player, College college);
        /// <summary>
        /// Applies the given effect to the unit.
        /// </summary>
        /// <param name="effect">Effect to apply.</param>
        void ApplyEffect(IEffect effect);
        /// <summary>
        /// Process all effects currently applied to the unit.
        /// This should also handle finishing of effects.
        /// Called on all units of a player on turn start.
        /// </summary>
        void ProcessEffects();
    }
}
