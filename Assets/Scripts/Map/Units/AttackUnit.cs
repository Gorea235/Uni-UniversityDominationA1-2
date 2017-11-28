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
        int hp = 5;
        int att = 3;
        int mov = 4;
        int def = 2;
        IPlayer owner;
        College college;

        //iplemented methods from IUnit
        public int Health
        {
            get
            {
                return hp;
            }

            set
            {
                hp = value;
            }
        }

        public int Attack { get { return att; } }

        public int MaxMove { get { return mov; } }

        public int Defence { get { return def; } }

        public IPlayer Owner { get { return owner; } }

        public College College { get { return College; } }

        //instantiation of a single AttackUnit
        public AttackUnit Init(IPlayer ownedby, College incollege) {
            /*TO DO: add a case statement to handle model rendering dependent on College similar to Sector's textures*/

            AttackUnit attacker = new AttackUnit();
            attacker.college = incollege;
            attacker.owner = ownedby;
            return attacker;
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
