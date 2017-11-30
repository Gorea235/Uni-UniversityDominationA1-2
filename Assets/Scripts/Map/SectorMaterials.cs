using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
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
        float _emissionTo = 0.3f;
        float _emissionFrom = 0.5f;
        float _emissionCurrentTime = 0;

        #endregion

        #region Initialisation

        public void Init()
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
            AddDefaultMat(SectorTexture.Wentworth, MatAlcuin);
            AddDefaultMat(SectorTexture.Concrete, MatConcrete);
            AddDefaultMat(SectorTexture.Grass, MatGrass);
            AddDefaultMat(SectorTexture.Stones, MatStones);
            AddDefaultMat(SectorTexture.Water, MatWater);
            // add modified materials to dictionary
            Material tmpMat;
            foreach (SectorTexture texture in Enum.GetValues(typeof(SectorTexture)))
            {
                // add bright texture
                tmpMat = GetFromDefaultMat(texture);
                tmpMat.SetColor("_EmissionColor", GetGrey(_emissionFrom));
                tmpMat.EnableKeyword("_EMISSION");
                _materials[texture].Add(SectorMaterialType.Bright, tmpMat);
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

        void AddDefaultMat(SectorTexture texture, Material material)
        {
            _materials.Add(texture, new Dictionary<SectorMaterialType, Material> {
                { SectorMaterialType.Normal, material }
            });
        }

        Material GetFromDefaultMat(SectorTexture texture)
        {
            return new Material(_materials[texture][SectorMaterialType.Normal]);
        }

        Color GetGrey(float amountWhite)
        {
            return Color.Lerp(Color.white, Color.black, amountWhite);
        }

        #endregion

        #region Material Fetching

        public Material GetMaterial(SectorTexture texture, SectorMaterialType type)
        {
            return _materials[texture][type];
        }

        #endregion

        #region MonoBehaviour

        void Update()
        {
            // process highlight glow
            _emissionCurrentTime += Time.deltaTime;
            Color currentEmission = GetGrey(Mathf.Lerp(_emissionFrom,
                                                       _emissionTo,
                                                       _emissionCurrentTime / _emissionChangeTime));
            foreach (var mat in _materials.Values)
            {
                mat[SectorMaterialType.Bright].SetColor("_EmissionColor", currentEmission);
            }
            if (_emissionCurrentTime >= _emissionChangeTime)
            {
                float tmp = _emissionTo;
                _emissionTo = _emissionFrom;
                _emissionFrom = tmp;
                _emissionCurrentTime = 0;
            }
        }

        #endregion
    }
}
