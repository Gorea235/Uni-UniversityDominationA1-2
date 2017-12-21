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
        int _baseDefence = 15;
        int _baseAttack = 0;
        int _baseMove = 0;
        IPlayer _owner;
        College _college;

        //iplemented methods from IUnit
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public int Defence { get { return _baseDefence; } }
        public IPlayer Owner { get { return _owner; } }
        public College College { get { return _college; } }
        public Transform Transform { get { return gameObject.transform; } }

        public int Attack { get { return _baseAttack; } }
        public int MaxMove { get { return _baseMove; } }

        //instantiation of a single BaseUnit
        public void Init(SectorMaterials torsoMaterials, IPlayer owner, College college)
        {
            _owner = owner;
            _college = college;

            switch (college)
            {
                // setup model application
            }
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
