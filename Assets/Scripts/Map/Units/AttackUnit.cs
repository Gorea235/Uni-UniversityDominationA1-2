﻿using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Map.Unit
{
    public class AttackUnit : MonoBehaviour, IUnit
    {
        #region Unity Bindings

        public Sprite icon;

        #endregion

        #region Private Fields

        const string _name = "Attack";
        int _health = 100;
        const int _baseAttack = 30;
        const int _baseAttackRange = 3;
        const int _baseMove = 3;
        int _availableMove;
        const int _baseDefence = 20;
        const int _buildRange = 0;
        const bool _buildable = true;
        readonly List<IEffect> _activeEffects = new List<IEffect>();
        IPlayer _owner;
        College _college;
        readonly Vector3 _defaultOffset = new Vector3(0, 0, 0.767f);

        #endregion

        #region Public Properties

        // implemented properties from IUnit
        public string Name { get { return _name; } }
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public int Attack { get { return _baseAttack; } }
        public int AttackRange { get { return _baseAttackRange; } }
        public int MaxMove { get { return _baseMove; } }
        public int AvailableMove
        {
            get { return _availableMove; }
            set { _availableMove = value; }
        }
        public int Defence { get { return _baseDefence; } }
        public int BuildRange { get { return _buildRange; } }
        public bool Buildable { get { return _buildable; } }
        public List<IEffect> ActiveEffects { get { return _activeEffects; } }
        public Sprite Icon { get { return icon; } }
        public IPlayer Owner { get { return _owner; } }
        public College College { get { return _college; } }
        public Transform Transform { get { return gameObject.transform; } }
        public Vector3 DefaultOffset { get { return _defaultOffset; } }

        #endregion

        #region Initialisation

        public void Init(SectorMaterials materials, IPlayer owner, College college)
        {
            Init(owner, college);
            gameObject.GetComponentsInChildren<MeshRenderer>()[1].material = materials.GetMaterial(college);
        }

        public void Init(IPlayer owner, College college)
        {
            _owner = owner;
            _college = college;
        }

        #endregion

        #region MonoBehaviour

        void Start()
        {

        }

        void Update()
        {

        }

        #endregion
    }
}
