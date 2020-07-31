using System;
using Model.Geo.Features.Climate;

namespace SaveSystem.Model
{
    [Serializable]
    public class WorldAreaData
    {
        public int id;
        public string name;
        public Climate climate;
        public int region;
    }
}