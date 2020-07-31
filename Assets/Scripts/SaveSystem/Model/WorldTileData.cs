using System;
using System.Collections.Generic;
using Model.Geo.Features.Climate;
using Model.Geo.Features.Terrain;
using Model.Geo.Support;

namespace SaveSystem.Model
{
    [Serializable]
    public class WorldTileData
    {
        public int id;
        public Position position;

        public int area;
        
        public Biome biome;
        public List<WeatherEffect> weatherEffects;



    }
}