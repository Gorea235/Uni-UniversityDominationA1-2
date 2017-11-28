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
        int hp = 8;
        int att = 1;
        int mov = 1;
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
        public void Init(IPlayer ownedby, College incollege) {
            /*TO DO: add a switch statement to handle model rendering dependent on College similar to Sector's textures*/
            switch (incollege)
            {
                /*case College.Alcuin:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = ;
                    break;
                case College.Constantine:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = ;
                    break;
                case College.Derwent:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = ;
                    break;
                case College.Goodricke:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = ;
                    break;
                case College.Halifax:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = ;
                    break;
                case College.James:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = ;
                    break;
                case College.Langwith:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = ;
                    break;
                case College.Vanbrugh:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = ;
                    break;
                case College.Wentworth:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = ;
                    break;*/
            }

            college = incollege;
            owner = ownedby;
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
