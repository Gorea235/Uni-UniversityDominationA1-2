using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Map.Unit
{
    public class BaseUnit : MonoBehaviour, IUnit
    {
        //arbitrary (for now, tweak after balance testing) values for
        //base units stats
        int _health = 1000;
        const int _baseAttack = 0;
        const int _baseMove = 0;
        int _availableMove;
        const int _baseDefence = 15;
        const int _buildRange = 2;
        const bool _buildable = false;
        readonly List<IEffect> _activeEffects = new List<IEffect>();
        IPlayer _owner;
        College _college;

        // implemented methods from IUnit
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public int Attack { get { return _baseAttack; } }
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
        public IPlayer Owner { get { return _owner; } }
        public College College { get { return _college; } }
        public Transform Transform { get { return gameObject.transform; } }

        //instantiation of a single BaseUnit
        public void Init(SectorMaterials materials, IPlayer owner, College college)
        {
            _owner = owner;
            _college = college;

            gameObject.GetComponentsInChildren<MeshRenderer>()[1].material = materials.GetMaterial(college);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
