using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Map.Hex;
using UnityEngine;

namespace Map
{
    public class Sector : MonoBehaviour
    {
        #region Unity Bindings

        public Material MatGrass;
        public Material MatWater;
        public Material MatStone;
        public Material MatConcrete;
        public Material MatAlcuin;
        public Material MatConstantine;
        public Material MatDerwent;
        public Material MatGoodricke;
        public Material MatHalifax;
        public Material MatJames;
        public Material MatLangwith;
        public Material MatVanbrugh;
        public Material MatWentworth;

        #endregion

        #region Private fields

        Coord _currentCoord;
        IUnit _occupyingUnit;
        ILandmark _landmark;
        bool _traversable;

        SectorTexture[] notTraversableTextures = {
            SectorTexture.Water
        };

        #endregion

        #region Public Properties

        public IUnit OccupyingUnit
        {
            get { return _occupyingUnit; }
            set
            {
                _occupyingUnit = value;
                _occupyingUnit.Position = Layout.Default.HexToPixel(_currentCoord);
            }
        }
        public ILandmark Landmark { get; set; }
        public bool Traversable { get; private set; }

        #endregion

        #region Initialisation

        /// <summary>
        /// Initialised the Sector. <paramref name="traversable"/> will be ignored
        /// if the texture in in the 'not traversable' textures list.
        /// </summary>
        /// <returns>The init.</returns>
        /// <param name="currentCoord">The coordinate of the Sector.</param>
        /// <param name="texture">The texture of the Sector.</param>
        /// <param name="traversable">
        /// Whether the sector is traversable (ignored
        /// if texture is in the 'not traversable' list).
        /// </param>
        public void Init(Coord currentCoord, SectorTexture texture, bool traversable)
        {
            _currentCoord = currentCoord;
            gameObject.transform.position = Layout.Default.HexToPixel(currentCoord);

            // Set texture
            switch (texture)
            {
                case SectorTexture.Grass:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatGrass;
                    break;
                case SectorTexture.Water:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatWater;
                    break;
                case SectorTexture.Stones:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatStone;
                    break;
                case SectorTexture.Concrete:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatConcrete;
                    break;
                case SectorTexture.Alcuin:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatAlcuin;
                    break;
                case SectorTexture.Constantine:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatConstantine;
                    break;
                case SectorTexture.Derwent:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatDerwent;
                    break;
                case SectorTexture.Goodricke:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatGoodricke;
                    break;
                case SectorTexture.Halifax:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatHalifax;
                    break;
                case SectorTexture.James:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatJames;
                    break;
                case SectorTexture.Langwith:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatLangwith;
                    break;
                case SectorTexture.Vanbrugh:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatVanbrugh;
                    break;
                case SectorTexture.Wentworth:
                    gameObject.GetComponentInChildren<MeshRenderer>().material = MatWentworth;
                    break;
            }

            if (!notTraversableTextures.Contains(texture))
            {
                Traversable = traversable;
                if (!traversable)
                {
                    // todo: process material shadowing
                }
            }
            else
                Traversable = false;
        }

        #endregion

        #region MonoBehaviour

        // Use this for initialization
        void Start()
        {
            // dev testing
            //Init(new Hex.Coord(), SectorTexture.Grass);
        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion
    }
}
