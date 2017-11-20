using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Context : MonoBehaviour
    {

        public IPlayer[] Players { get; }
        public Gui.Gui Gui { get; }
        public Map.Map Map { get; }
        public AudioManager Audio { get; }

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
