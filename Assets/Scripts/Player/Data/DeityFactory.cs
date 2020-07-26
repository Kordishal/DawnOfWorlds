using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Model.Deity;
using UnityEngine;

namespace Player.Data
{
    public class DeityFactory
    {
        private const string FileName = "deities.json";
        private const string DirectoryName = "deities";

        private int NextDeityIdentifier => _collection?.deities?.Count + 1 ?? 1;

        private readonly string _saveFile;
        private readonly DeityCollection _collection;

        public DeityFactory(string basePath)
        {
            var directory = Path.Combine(basePath, DirectoryName);
            _saveFile = Path.Combine(directory, FileName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (!File.Exists(_saveFile))
            {
                SaveDeities();
            }
            _collection = JsonUtility.FromJson<DeityCollection>(File.ReadAllText(_saveFile)) ?? new DeityCollection();
        }

        public Deity CreateDeity(string name)
        {
            var deity = new Deity(NextDeityIdentifier, name);
            if (_collection.deities == null) _collection.deities = new List<Deity>();
            _collection.deities.Add(deity);
            SaveDeities();
            return deity;
        }

        public bool DeleteDeity(int identifier)
        {
            if (identifier >= NextDeityIdentifier) return false;
            if (!_collection.deities.Remove(new Deity(identifier))) return false;
            SaveDeities();
            return true;
        }

        public void UpdateDeity(Deity deity)
        {
            _collection.deities.Remove(deity);
            _collection.deities.Add(deity);
            SaveDeities();
        }

        [CanBeNull]
        public Deity GetDeity(int identifier) => identifier >= NextDeityIdentifier
            ? null
            : _collection.deities.Find(deity => deity.identifier == identifier);

        private void SaveDeities()
        {
            File.WriteAllText(_saveFile, JsonUtility.ToJson(_collection));
        }

        [Serializable]
        private class DeityCollection
        {
            public List<Deity> deities;
        }
    }
}