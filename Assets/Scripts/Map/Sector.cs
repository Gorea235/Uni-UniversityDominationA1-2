using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Sector : MonoBehaviour
    {
        #region Unity Bindings

        public Texture testTexture;

        #endregion

        #region Private fields

        Hex.Coord _currentCoord;

        #endregion

        #region Public Properties

        public IUnit OccupyingUnit { get; set; }
        public ILandmark Landmark { get; set; }

        #endregion

        #region Initialisation

        public void Init(Hex.Coord currentCoord, SectorTexture texture)
        {
            _currentCoord = currentCoord;

            // Set texture
            switch (texture)
            {
                case SectorTexture.Grass:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/grass", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
                case SectorTexture.Water:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/water", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
                case SectorTexture.Stone:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/stone", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
                case SectorTexture.Concrete:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/concrete", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
                case SectorTexture.Alcuin:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/alcuin", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
                case SectorTexture.Constantine:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/constantine", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
                case SectorTexture.Derwent:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/derwent", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
                case SectorTexture.Goodricke:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/goodricke", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
                case SectorTexture.Halifax:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/halifax", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
                case SectorTexture.James:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/james", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
                case SectorTexture.Langwith:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/langwith", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
                case SectorTexture.Vanbrugh:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/vanbrugh", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
                case SectorTexture.Wentworth:
                    gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load(path: "Assets/Prefabs/Textures/wentworth", systemTypeInstance: typeof(Texture)) as Texture;
                    break;
            }
        }

        #endregion

        #region MonoBehaviour

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion
    }
}
