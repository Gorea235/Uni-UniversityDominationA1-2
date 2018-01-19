using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Map.Hex;
using UnityEngine;

namespace Map
{
    public class Sector : MonoBehaviour
    {
        #region Constants

        public static readonly SectorTexture[] notTraversableTextures = {
            SectorTexture.Water
        };

        #endregion

        #region Private fields

        MeshRenderer _renderer;
        SectorMaterials _sectorMaterials;
        Material _defaultMaterial;
        Material _highlightBrightMaterial;
        Material _highlightDimmedMaterial;
        HighlightLevel _highlightLevel;

        IUnit _occupyingUnit;
        ILandmark _landmark;

        #endregion

        #region Public Properties

        public IUnit OccupyingUnit
        {
            get { return _occupyingUnit; }
            set
            {
                _occupyingUnit = value;
                _occupyingUnit.Transform.parent = gameObject.transform;
                _occupyingUnit.Transform.localPosition = _occupyingUnit.DefaultOffset;
            }
        }
        public ILandmark Landmark
        {
            get { return _landmark; }
            set
            {
                _landmark = value;
                _landmark.Transform.parent = gameObject.transform;
            }
        }
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
        public void Init(SectorMaterials sectorMaterials, Coord currentCoord, SectorTexture texture, bool traversable)
        {
            // setup base vars
            _renderer = gameObject.GetComponentInChildren<MeshRenderer>();

            gameObject.transform.position = Layout.Default.HexToPixel(currentCoord);

            if (!notTraversableTextures.Contains(texture))
                Traversable = traversable;
            else
                Traversable = false;

            // Setup materials
            _sectorMaterials = sectorMaterials;
            _defaultMaterial = _sectorMaterials.GetMaterial(texture, Traversable ? SectorMaterialType.Normal : SectorMaterialType.Dark);
            _highlightBrightMaterial = _sectorMaterials.GetMaterial(texture, SectorMaterialType.Bright);
            _highlightDimmedMaterial = _sectorMaterials.GetMaterial(texture, SectorMaterialType.Dimmed);
            ApplyMaterial(_defaultMaterial);
        }

        #endregion

        #region Material Modifications

        public HighlightLevel Highlight
        {
            get { return _highlightLevel; }
            set
            {
                _highlightLevel = value;
                switch (value)
                {
                    case HighlightLevel.Bright:
                        ApplyMaterial(_highlightBrightMaterial);
                        break;
                    case HighlightLevel.Dimmed:
                        ApplyMaterial(_highlightDimmedMaterial);
                        break;
                    case HighlightLevel.None:
                        ApplyMaterial(_defaultMaterial);
                        break;
                }
            }
        }

        void ApplyMaterial(Material material)
        {
            _renderer.material = material;
        }

        #endregion
    }
}
