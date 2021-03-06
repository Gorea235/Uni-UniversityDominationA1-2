using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// A class used to manage all of the materials used by sectors and units of the game.
    /// </summary>
    public class SectorMaterials : MonoBehaviour
    {
        #region Unity Bindings

        public Material MatAlcuin;
        public Material MatConstantine;
        public Material MatDerwent;
        public Material MatGoodricke;
        public Material MatHalifax;
        public Material MatJames;
        public Material MatLangwith;
        public Material MatVanbrugh;
        public Material MatWentworth;
        public Material MatConcrete;
        public Material MatGrass;
        public Material MatStones;
        public Material MatWater;

        #endregion

        #region Private Fields

        Dictionary<SectorTexture, Dictionary<SectorMaterialType, Material>> _materials;

        const float _emissionChangeTime = 1f;
        float _brightEmissionFrom = 0.5f;
        float _brightEmissionTo = 0.3f;
        float _dimmedEmissionFrom = 0.75f;
        float _dimmedEmissionTo = 0.6f;
        float _emissionCurrentTime = 0;

        #endregion

        #region Initialisation

        void Awake() // initialise all of the materials we need
        {
            // init material dictionary
            _materials = new Dictionary<SectorTexture, Dictionary<SectorMaterialType, Material>>();
            // add default materials to dictionary
            AddDefaultMat(SectorTexture.Alcuin, MatAlcuin);
            AddDefaultMat(SectorTexture.Constantine, MatConstantine);
            AddDefaultMat(SectorTexture.Derwent, MatDerwent);
            AddDefaultMat(SectorTexture.Goodricke, MatGoodricke);
            AddDefaultMat(SectorTexture.Halifax, MatHalifax);
            AddDefaultMat(SectorTexture.James, MatJames);
            AddDefaultMat(SectorTexture.Langwith, MatLangwith);
            AddDefaultMat(SectorTexture.Vanbrugh, MatVanbrugh);
            AddDefaultMat(SectorTexture.Wentworth, MatWentworth);
            AddDefaultMat(SectorTexture.Concrete, MatConcrete);
            AddDefaultMat(SectorTexture.Grass, MatGrass);
            AddDefaultMat(SectorTexture.Stones, MatStones);
            AddDefaultMat(SectorTexture.Water, MatWater);
            // add modified materials to dictionary
            Material tmpMat;
            // loop through SectorTexture as it provides all of the materials we will use as the base
            foreach (SectorTexture texture in Enum.GetValues(typeof(SectorTexture)))
            {
                // add bright texture
                tmpMat = GetFromDefaultMat(texture);
                tmpMat.SetColor("_EmissionColor", GetGrey(_brightEmissionFrom));
                tmpMat.EnableKeyword("_EMISSION");
                _materials[texture].Add(SectorMaterialType.Bright, tmpMat);
                // add dimmed texture
                tmpMat = GetFromDefaultMat(texture);
                tmpMat.SetColor("_EmissionColor", GetGrey(_dimmedEmissionFrom));
                tmpMat.EnableKeyword("_EMISSION");
                _materials[texture].Add(SectorMaterialType.Dimmed, tmpMat);
                // add dark texture without change if sector type is traversable by default
                if (Sector.notTraversableTextures.Contains(texture))
                    _materials[texture].Add(SectorMaterialType.Dark, GetFromDefaultMat(texture));
                else
                {
                    // add dark texture
                    tmpMat = GetFromDefaultMat(texture);
                    tmpMat.SetColor("_Color", GetGrey(0.5f));
                    _materials[texture].Add(SectorMaterialType.Dark, tmpMat);
                }
            }
        }

        /// <summary>
        /// Adds the default material to the material dictionary.
        /// </summary>
        /// <param name="texture">The enum that will reference it.</param>
        /// <param name="material">The normal material.</param>
        void AddDefaultMat(SectorTexture texture, Material material)
        {
            _materials.Add(texture, new Dictionary<SectorMaterialType, Material> {
                { SectorMaterialType.Normal, material }
            });
        }

        /// <summary>
        /// Creates a new material using the normal one of the given SectorTexture.
        /// </summary>
        /// <param name="texture">The texture to use.</param>
        /// <returns>The new material copied from the base.</returns>
        Material GetFromDefaultMat(SectorTexture texture) => new Material(_materials[texture][SectorMaterialType.Normal]);

        /// <summary>
        /// Performs a linear interpolation between white and black by the given percentage.
        /// </summary>
        /// <param name="amountWhite">The percentage between white and black to get.</param>
        /// <returns>The grey colour.</returns>
        Color GetGrey(float amountWhite) => Color.Lerp(Color.white, Color.black, amountWhite);

        #endregion

        #region Material Fetching

        /// <summary>
        /// Gets the material for the SectorTexture and SectorMaterialType.
        /// </summary>
        /// <returns>The material.</returns>
        /// <param name="texture">The texture to get.</param>
        /// <param name="type">The version of the texture to get.</param>
        public Material GetMaterial(SectorTexture texture, SectorMaterialType type) => _materials[texture][type];

        /// <summary>
        /// Gets the material for the given college. It will use
        /// <see cref="SectorMaterialType.Normal"/> for the material type.
        /// </summary>
        /// <returns>The material.</returns>
        /// <param name="college">The college texture to get.</param>
        public Material GetMaterial(College college) => GetMaterial((SectorTexture)college, SectorMaterialType.Normal);

        #endregion

        #region MonoBehaviour

        void Update() // used for global material change calculations
        {
            // === process highlight glow ===
            _emissionCurrentTime += Time.deltaTime; // the current time through the transition
            Color currentEmission;

            // set the emission color for all the bright materials
            currentEmission = GetEmissionLevel(_brightEmissionFrom, _brightEmissionTo);
            foreach (var mat in _materials.Values)
                mat[SectorMaterialType.Bright].SetColor("_EmissionColor", currentEmission);

            // set the emission color for all the dimmed materials
            currentEmission = GetEmissionLevel(_dimmedEmissionFrom, _dimmedEmissionTo);
            foreach (var mat in _materials.Values)
                mat[SectorMaterialType.Dimmed].SetColor("_EmissionColor", currentEmission);

            // if the transition has completed, reverse the direction
            if (_emissionCurrentTime >= _emissionChangeTime)
            {
                float tmp = _brightEmissionTo;
                _brightEmissionTo = _brightEmissionFrom;
                _brightEmissionFrom = tmp;
                tmp = _dimmedEmissionTo;
                _dimmedEmissionTo = _dimmedEmissionFrom;
                _dimmedEmissionFrom = tmp;
                _emissionCurrentTime = 0;
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Gets the emission level using the start and end position using the
        /// current emission time.
        /// </summary>
        /// <param name="from">The emission level to start at.</param>
        /// <param name="to">The emission level to go to.</param>
        /// <returns>The current emission level for the current time.</returns>
        /// <remarks>
        /// The following code for this function is a little odd, but I'm gonna walk through it
        /// here (mostly so I know what's going on).
        /// </remarks>
        Color GetEmissionLevel(float from, float to) => GetGrey( // GetGrey will get the colour grey that's t percent
                                                                 //(between 0-1) between black and white
                                                                 Mathf.Lerp(from,  // the emission that is being transitioned from
                                                                     to, // the emission that is being transitioned to
                                                                     MathHelpers.EaseInOutPolynomial( // ease the transition
                                                                                                      // the percent of the transition we are through
                                                                         _emissionCurrentTime / _emissionChangeTime,
                                                                         2))); // sets easing to quadratic

        /// <summary>
        /// Gets the highlight tint.
        /// </summary>
        /// <returns>The highlight tint.</returns>
        /// <param name="level">Highlight level.</param>
        public Color GetHighlightTint(HighlightLevel level)
        {
            switch (level)
            {
                case HighlightLevel.Bright:
                    return _materials[SectorTexture.Alcuin][SectorMaterialType.Bright].GetColor("_Color");
                case HighlightLevel.Dimmed:
                    return _materials[SectorTexture.Alcuin][SectorMaterialType.Dimmed].GetColor("_Color");
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Sets the highlight tint.
        /// </summary>
        /// <param name="level">Highlight level.</param>
        /// <param name="tint">The tint.</param>
        public void SetHighlightTint(HighlightLevel level, Color tint)
        {
            switch (level)
            {
                case HighlightLevel.Bright:
                    foreach (var mat in _materials.Values)
                        mat[SectorMaterialType.Bright].SetColor("_Color", tint);
                    break;
                case HighlightLevel.Dimmed:
                    foreach (var mat in _materials.Values)
                        mat[SectorMaterialType.Dimmed].SetColor("_Color", tint);
                    break;
            }
        }

        #endregion
    }
}
