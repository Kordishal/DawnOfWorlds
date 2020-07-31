using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Meta;
using Model.Geo.Organization;
using SaveSystem.Model;
using UnityEngine;

namespace SaveSystem
{
    public class SaveSystemManager : MonoBehaviour
    {
        private static int _saveCount = 0;
        private static int _tileCount = 1;
        private static int _areaCount = 1;
        private static int _regionCount = 1;

        private Dictionary<string, int> _savedAreasIdMap;
        private Dictionary<string, int> _savedRegionsIdMap;

        private List<WorldTile> _tiles;

        private string _path;

        private void Start()
        {
            _path = Path.Combine(Application.persistentDataPath, "saves");
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            _savedAreasIdMap = new Dictionary<string, int>();
            _savedRegionsIdMap = new Dictionary<string, int>();
        }

        public void Save()
        {
            if (_tiles == null || _tiles.Count == 0)
                _tiles = GameObject.FindGameObjectsWithTag(Tags.Tile).Select(go => go.GetComponent<WorldTile>())
                    .ToList();
            var text = JsonUtility.ToJson(BuildSave());
            File.WriteAllText(Path.Combine(_path, "saveFile" + _saveCount + ".json"), text);
            _saveCount += 1;
        }


        private SaveData BuildSave()
        {
            var save = new SaveData
            {
                worldData = new WorldData()
                {
                    tileData = new List<WorldTileData>(),
                    areaData = new List<WorldAreaData>(),
                    regionData = new List<WorldRegionData>(),
                }
            };
            _tileCount = 1;
            _areaCount = 1;
            _regionCount = 1;
            foreach (var worldTile in _tiles)
            {
                var (areaId, area, region) = SaveArea(worldTile.worldArea);
                var tileData = new WorldTileData()
                {
                    id = _tileCount,
                    position = worldTile.position,
                    biome = worldTile.biome,
                    weatherEffects = worldTile.weatherEffects,
                    area = areaId
                };
                save.worldData.tileData.Add(tileData);
                if (area != null)
                    save.worldData.areaData.Add(area);
                if (region != null)
                    save.worldData.regionData.Add(region);

                _tileCount += 1;
            }
            _savedAreasIdMap.Clear();
            _savedRegionsIdMap.Clear();
            return save;
        }


        private Tuple<int, WorldAreaData, WorldRegionData> SaveArea([CanBeNull] WorldArea area)
        {
            if (area == null)
                return new Tuple<int, WorldAreaData, WorldRegionData>(-1, null, null);

            if (_savedAreasIdMap.ContainsKey(area.areaName))
                return new Tuple<int, WorldAreaData, WorldRegionData>(_savedAreasIdMap[area.areaName], null, null);
            
            var (id, regionData) = SaveRegion(area.worldRegion);
            var areaData = new WorldAreaData()
            {
                id = _areaCount,
                name = area.areaName,
                climate = area.climate,
                region = id
            };

            _savedAreasIdMap[area.areaName] = _areaCount;

            _areaCount += 1;
            return new Tuple<int, WorldAreaData, WorldRegionData>(areaData.id, areaData, regionData);
        }

        private Tuple<int, WorldRegionData> SaveRegion([CanBeNull] WorldRegion region)
        {
            if (region == null)
                return new Tuple<int, WorldRegionData>(-1, null);

            if (_savedRegionsIdMap.ContainsKey(region.regionName))
                return new Tuple<int, WorldRegionData>(_savedRegionsIdMap[region.regionName], null);

            var regionData = new WorldRegionData()
            {
                id = _regionCount,
                name = region.regionName
            };
            
            _savedRegionsIdMap[region.regionName] = _regionCount;
            _regionCount += 1;
            return new Tuple<int, WorldRegionData>(regionData.id, regionData);
        }
    }
}