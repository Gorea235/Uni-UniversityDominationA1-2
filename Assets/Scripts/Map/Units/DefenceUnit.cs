using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Map.Unit
{
    public class DefenceUnit : MonoBehaviour, IUnit
    {

        public Material MatAlcuin;
        public Material MatConstantine;
        public Material MatDerwent;
        public Material MatGoodricke;
        public Material MatHalifax;
        public Material MatJames;
        public Material MatLangwith;
        public Material MatVanbrugh;
        public Material MatWentworth;

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
                //As GetComponentsInChildren returns an array of the matching type components from BOTH parent and child
                //the indexing is for referencing the only child (as we are using a hierarchy of Unit > torso for all units)
                case College.Alcuin:
                    gameObject.GetComponentsInChildren<MeshRenderer>()[1].material = MatAlcuin;
                    break;
                case College.Constantine:
                    gameObject.GetComponentsInChildren<MeshRenderer>()[1].material = MatConstantine;
                    break;
                case College.Derwent:
                    gameObject.GetComponentsInChildren<MeshRenderer>()[1].material = MatDerwent;
                    break;
                case College.Goodricke:
                    gameObject.GetComponentsInChildren<MeshRenderer>()[1].material = MatGoodricke;
                    break;
                case College.Halifax:
                    gameObject.GetComponentsInChildren<MeshRenderer>()[1].material = MatHalifax;
                    break;
                case College.James:
                    gameObject.GetComponentsInChildren<MeshRenderer>()[1].material = MatJames;
                    break;
                case College.Langwith:
                    gameObject.GetComponentsInChildren<MeshRenderer>()[1].material = MatLangwith;
                    break;
                case College.Vanbrugh:
                    gameObject.GetComponentsInChildren<MeshRenderer>()[1].material = MatVanbrugh;
                    break;
                case College.Wentworth:
                    gameObject.GetComponentsInChildren<MeshRenderer>()[1].material = MatWentworth;
                    break;
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
