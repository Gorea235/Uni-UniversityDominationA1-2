using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Map.Unit
{
    public class AttackUnit : MonoBehaviour, IUnit
    {

        //arbitrary (for now, tweak after balance testing) values for
        //attack units stats
        int _health = 100;
        int _baseAttack = 30;
        int _baseMove = 3;
        int _baseDefence = 20;
        IPlayer _owner;
        College _college;

        //iplemented methods from IUnit
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public int Attack { get { return _baseAttack; } }
        public int MaxMove { get { return _baseMove; } }
        public int Defence { get { return _baseDefence; } }
        public IPlayer Owner { get { return _owner; } }
        public College College { get { return _college; } }
        public Transform Transform { get { return gameObject.transform; } }

        //instantiation of a single AttackUnit
        public void Init(SectorMaterials materials, IPlayer owner, College college)
        {
            _owner = owner;
            _college = college;

            gameObject.GetComponentsInChildren<MeshRenderer>()[1].material = materials.GetMaterial(college);
        }

        public void ApplyEffect(IEffect effect)
        {

        }

        public void ProcessEffects()
        {

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
