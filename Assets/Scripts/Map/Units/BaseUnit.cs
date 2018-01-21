using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Map.Unit
{
    public class BaseUnit : MonoBehaviour, IUnit
    {
        #region Unity Bindings

        public Sprite icon;

        #endregion

        #region Private Fields

        const string _name = "Base";
        int _health = 1000;
        const int _baseAttack = 0;
        const int _baseAttackRange = 0;
        bool _hasAttacked = false;
        const int _baseMove = 0;
        int _availableMove;
        const int _baseDefence = 15;
        const int _buildRange = 2;
        const bool _buildable = false;
        const int _baseCost = int.MaxValue;
        const int _baseManaMoveRatio = int.MaxValue;
        const int _baseManaAttackCost = int.MaxValue;
        readonly List<IEffect> _activeEffects = new List<IEffect>();
        IPlayer _owner;
        College _college;
        readonly Vector3 _defaultOffset = new Vector3(0, 0, 0.673f);

        #endregion

        #region Public Properties

        public string Name { get { return _name; } }
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public int Attack { get { return _baseAttack; } }
        public int AttackRange { get { return _baseAttackRange; } }
        public bool HasAttacked
        {
            get { return _hasAttacked; }
            set { _hasAttacked = value; }
        }
        public int MaxMove { get { return _baseMove; } }
        public int AvailableMove
        {
            get { return _availableMove; }
            set { _availableMove = value; }
        }
        public int Defence { get { return _baseDefence; } }
        public int BuildRange { get { return _buildRange; } }
        public bool Buildable { get { return _buildable; } }
        public int Cost { get { return _baseCost; } }
        public int ManaMoveRatio { get { return _baseManaMoveRatio; } }
        public int ManaAttackCost { get { return _baseManaAttackCost; } }
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

        #region Public Methods

        public void Kill() => Destroy(gameObject);

        #endregion
    }
}
