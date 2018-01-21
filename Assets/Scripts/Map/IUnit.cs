using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public interface IUnit
    {
        /// <summary>
        /// Gets the name of the unit.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets or sets the health of the unit.
        /// </summary>
        int Health { get; set; }
        /// <summary>
        /// Gets the attack value of the unit.
        /// If 0, then it cannot attack.
        /// </summary>
        /// <remarks>The is the damage the unit can cause to other units.</remarks>
        int Attack { get; }
        /// <summary>
        /// Gets the attack range of the unit.
        /// </summary>
        int AttackRange { get; }
        /// <summary>
        /// Whether this unit has attacked during the current turn;
        /// </summary>
        bool HasAttacked { get; set; }
        /// <summary>
        /// Gets the max move range of the unit.
        /// </summary>
        int MaxMove { get; }
        /// <summary>
        /// Gets or sets the available move range of the unit.
        /// </summary>
        int AvailableMove { get; set; }
        /// <summary>
        /// Gets the defence value of the unit.
        /// Currently the percentage reduction in damage received.
        /// </summary>
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
        int BuildRange { get; }
        /// <summary>
        /// Gets a value indicating whether the unit is buildable via builder
        /// units.
        /// </summary>
        bool Buildable { get; }
        /// <summary>
        /// The amount of mana this unit requires to build.
        /// </summary>
        int Cost { get; }
        /// <summary>
        /// The ratio of move amount to mana cost this unit has.
        /// </summary>
        int ManaMoveRatio { get; }
        /// <summary>
        /// The amount of mana it costs for this unit to attack.
        /// </summary>
        int ManaAttackCost { get; }
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
        List<IEffect> ActiveEffects { get; }
        /// <summary>
        /// The build menu icon of the unit.
        /// </summary>
        Sprite Icon { get; }
        /// <summary>
        /// The player that owns the unit.
        /// </summary>
        Manager.IPlayer Owner { get; }
        /// <summary>
        /// The college of the unit.
        /// </summary>
        College College { get; }
        /// <summary>
        /// Returns the GameObject transform property.
        /// </summary>
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
        /// <summary>
        /// Initialises the unit with the given player and college.
        /// WARNING: This function should only be used in <see cref="Manager.MainManager.Awake"/>.
        /// </summary>
        /// <param name="player">The owning player.</param>
        /// <param name="college">The college the unit is from.</param>
        void Init(Manager.IPlayer player, College college);
        /// <summary>
        /// Kills the unit. This involves removing it from the scene.
        /// </summary>
        void Kill();
    }
}
