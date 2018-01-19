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
        /// Gets the attack range of the unit.
        /// </summary>
        /// <value>The attack range.</value>
        int AttackRange { get; }
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
        /// The active effects that have been applied to the unit.
        /// Currently, effects are planned to all take effect at the start of
        /// each turn. However, if some effects should be processed at specific
        /// points in the phases or are passive (i.e. change how damage is
        /// taken etc.), IEffect should specify when the effect should be applied,
        /// and what it should do. If effects are to be applied only at specified
        /// points in the turn cycle, then this should move to a
        /// <c>Dictionary&lt;EffectApplyEnum, IEffect&gt;</c>, where
        /// <c>EffectApplyEnum</c> is the enum that will specify the apply point.
        /// </summary>
        /// <value>The active effects.</value>
        List<IEffect> ActiveEffects { get; }
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
        /// The default value to offset the GameObject by.
        /// </summary>
        Vector3 DefaultOffset { get; }
        /// <summary>
        /// Initialises the unit with the given colour, player and college.
        /// </summary>
        /// <param name="materials">The SectorMaterials GameObject.</param>
        /// <param name="player">The owning player.</param>
        /// <param name="college">The college the unit is from.</param>
        void Init(SectorMaterials materials, Manager.IPlayer player, College college);
    }
}
