using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Map.Unit
{
    public class DefenceUnit : MonoBehaviour, IUnit
    {
        //arbitrary (for now, tweak after balance testing) values for
        //defence units stats
        int _health = 160;
        int _baseAttack = 15;
        int _baseMove = 2;
        int _baseDefence = 40;
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

        //instantiation of a single DefenceUnit
        public void Init(IPlayer owner, College college)
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
