using Model.TileFeatures;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Model.Tiles
{
    public class ColourTile : TileBase
    {
        public Sprite sprite;
        public Color color;
        public GameObject gameObject;
        public WeatherEffect weatherEffect;
        public Biome biome;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = sprite;
            tileData.color = color;
            tileData.gameObject = gameObject;
        }

#if UNITY_EDITOR
// The following is a helper that adds a menu item to create a RoadTile Asset
        [MenuItem("Assets/Create/ColourTile")]
        public static void CreateRoadTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Colour Tile", "New Colour Tile", "asset", "Save Colour Tile", "Assets/StreamingAssets/tiles/colour");
            if (path == "")
                return;

            var instance = CreateInstance<ColourTile>();
            instance.sprite = Resources.Load<Sprite>("TileSprite");
            instance.gameObject = Resources.Load<GameObject>("TileGameObjectPrefab");
            AssetDatabase.CreateAsset(instance, path);
        }
#endif
    }
}