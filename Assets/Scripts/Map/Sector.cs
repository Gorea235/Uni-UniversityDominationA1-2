using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Map.Hex;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// The sector class that manages all the data in a given hexagonal sector.
    /// </summary>
    public class Sector : MonoBehaviour
    {
        #region Constants

        /// <summary>
        /// The textures that are said to be non-traversable by default.
        /// </summary>
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

        /// <summary>
        /// The currently occupying unit on the sector.
        /// </summary>
        public IUnit OccupyingUnit
        {
            get { return _occupyingUnit; }
            set
            {
                _occupyingUnit = value;
                if (value != null)
                {
                    _occupyingUnit.Transform.parent = gameObject.transform;
                    _occupyingUnit.Transform.localPosition = _occupyingUnit.DefaultOffset;
                }
            }
        }
        /// <summary>
        /// The current landmark on this sector.
        /// </summary>
        public ILandmark Landmark
        {
            get { return _landmark; }
            set
            {
                _landmark = value;
                if (value != null)
                    _landmark.Transform.parent = gameObject.transform;
            }
        }
        /// <summary>
        /// Whether the current sector is traversable by units.
        /// </summary>
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

            gameObject.transform.position = Layout.Default.HexToPixel(currentCoord); // get the position the sector should be in
            Traversable = traversable && !notTraversableTextures.Contains(texture); // work out whether the sector is traversable or not

            // Setup materials
            _sectorMaterials = sectorMaterials;
            _defaultMaterial = _sectorMaterials.GetMaterial(texture, Traversable ? SectorMaterialType.Normal : SectorMaterialType.Dark);
            _highlightBrightMaterial = _sectorMaterials.GetMaterial(texture, SectorMaterialType.Bright);
            _highlightDimmedMaterial = _sectorMaterials.GetMaterial(texture, SectorMaterialType.Dimmed);
            ApplyMaterial(_defaultMaterial);
        }

        #endregion

        #region Material Modifications

        /// <summary>
        /// Gets and sets the current highlight of the sector.
        /// </summary>
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

        /// <summary>
        /// Applies the given material to the sector.
        /// </summary>
        /// <param name="material"></param>
        void ApplyMaterial(Material material)
        {
            _renderer.material = material;
        }

        #endregion
    }
}
