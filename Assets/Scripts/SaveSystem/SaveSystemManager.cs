using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Meta;
using Model.Geo.Organization;
using Model.Geo.Support;
using SaveSystem.Model;
using UnityEngine;
using UnityEngine.UI;

namespace SaveSystem
{
    public class SaveSystemManager : MonoBehaviour
    {
        private static int _saveCount = 0;
        private static int _tileCount = 1;
        private static int _areaCount = 1;
        private static int _regionCount = 1;

        public Dropdown savesDropdown;

        private Dictionary<string, int> _savedAreasIdMap;
        private Dictionary<string, int> _savedRegionsIdMap;

        private Dictionary<int, WorldArea> _loadedWorldAreas;
        private Dictionary<int, WorldRegion> _loadedWorldRegions;

        private Dictionary<int, WorldRegionData> _regionDict;

        private List<WorldTile> _tiles;

        private GameObject _gameWorld;

        private string _path;

        public GameObject worldAreaPrefab;
        public GameObject worldRegionPrefab;

        private void Start()
        {
            _path = Path.Combine(Application.persistentDataPath, "saves");
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            _savedAreasIdMap = new Dictionary<string, int>();
            _savedRegionsIdMap = new Dictionary<string, int>();
            _gameWorld = GameObject.FindWithTag(Tags.World);

            foreach (var file in Directory.EnumerateFiles(_path))
            {
                savesDropdown.options.Add(new Dropdown.OptionData(file));
                var regex = new Regex("saveFile(\\d+).json");
                var match = regex.Match(file);
                var num = int.Parse(match.Groups[1].Value);
                if (num > _saveCount)
                    _saveCount = num;
            }

            _saveCount += 1;
            if (savesDropdown != null) savesDropdown.value = 1;
        }

        public void Save()
        {
            if (_tiles == null || _tiles.Count == 0)
                _tiles = GameObject.FindGameObjectsWithTag(Tags.Tile).Select(go => go.GetComponent<WorldTile>())
                    .ToList();
            var text = JsonUtility.ToJson(BuildSave());
            var newFileName = "saveFile" + _saveCount + ".json";
            File.WriteAllText(Path.Combine(_path, newFileName), text);
            savesDropdown.options.Add(new Dropdown.OptionData(newFileName));
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
                    type = worldTile.type,
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

        public void LoadGame()
        {
            foreach (var areaGo in GameObject.FindGameObjectsWithTag(Tags.Area))
            {
                DestroyImmediate(areaGo);
            }

            foreach (var regionGo in GameObject.FindGameObjectsWithTag(Tags.Region))
            {
                DestroyImmediate(regionGo);
            }

            var json = File.ReadAllText(Path.Combine(_path, savesDropdown.options[savesDropdown.value].text));
            var saveData = JsonUtility.FromJson<SaveData>(json);
            _loadedWorldAreas = new Dictionary<int, WorldArea>();
            _loadedWorldRegions = new Dictionary<int, WorldRegion>();

            var tileDict = new Dictionary<Position, WorldTileData>();
            foreach (var tileData in saveData.worldData.tileData)
            {
                tileDict[tileData.position] = tileData;
            }

            var areaDict = new Dictionary<int, WorldAreaData>();
            foreach (var areaData in saveData.worldData.areaData)
            {
                areaDict[areaData.id] = areaData;
            }

            _regionDict = new Dictionary<int, WorldRegionData>();
            foreach (var regionData in saveData.worldData.regionData)
            {
                _regionDict[regionData.id] = regionData;
            }

            if (_tiles == null || _tiles.Count == 0)
                _tiles = GameObject.FindGameObjectsWithTag(Tags.Tile).Select(go => go.GetComponent<WorldTile>())
                    .ToList();

            foreach (var tile in _tiles)
            {
                var tileData = tileDict[tile.position];
                tile.type = tileData.type;
                tile.biome = tileData.biome;
                tile.weatherEffects = tileData.weatherEffects;
                if (tileData.area != -1)
                    tile.worldArea = InstantiateWorldArea(tile, areaDict[tileData.area]);
            }

            _regionDict = null;
            _loadedWorldAreas = null;
            _loadedWorldRegions = null;
            _gameWorld.GetComponent<WorldMap>().UpdateAllSprites();
        }


        private WorldArea InstantiateWorldArea(WorldTile tile, WorldAreaData data)
        {
            if (!_loadedWorldAreas.ContainsKey(data.id))
            {
                var go = Instantiate(worldAreaPrefab, _gameWorld.transform);
                var worldArea = go.GetComponent<WorldArea>();
                worldArea.areaName = data.name;
                worldArea.climate = data.climate;
                worldArea.tiles = new List<WorldTile> {tile};
                WorldRegion region = null;
                if (data.region != -1)
                    region = InstantiateWorldRegion(worldArea, _regionDict[data.region]);
                worldArea.worldRegion = region;
                _loadedWorldAreas[data.id] = worldArea;
                return worldArea;
            }

            var area = _loadedWorldAreas[data.id];
            area.tiles.Add(tile);
            return area;
        }


        private WorldRegion InstantiateWorldRegion(WorldArea area, WorldRegionData data)
        {
            if (_loadedWorldRegions.ContainsKey(data.id))
            {
                var region = _loadedWorldRegions[data.id];
                region.areas.Add(area);
                return region;
            }

            var go = Instantiate(worldRegionPrefab, _gameWorld.transform);
            var worldRegion = go.GetComponent<WorldRegion>();
            worldRegion.regionName = data.name;
            worldRegion.areas = new List<WorldArea> {area};
            _loadedWorldRegions[data.id] = worldRegion;
            return worldRegion;
        }
    }
}