using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Meta.EventArgs;
using Model.Deity;
using UnityEngine;

namespace Player.Data
{
    public class DeityFactory
    {
        private static DeityFactory _factory;

        public static DeityFactory GetInstance(string path)
        {
            return _factory ?? (_factory = new DeityFactory(path));
        }

        private const string FileName = "deities.json";
        private const string DirectoryName = "deities";

        private int NextDeityIdentifier => _collection?.deities?.Count + 1 ?? 1;

        private readonly string _saveFile;
        private readonly DeityCollection _collection;

        private Deity _currentDeity;
        public Deity CurrentDeity
        {
            get => _currentDeity;
            set
            {
                if (_currentDeity == value) return;
                OnCurrentDeityChanged(value);
                _currentDeity = value;
            }
        }
        public event EventHandler<ChangedCurrentDeityEventUpdate> OnCurrentDeityChange;
        protected virtual void OnCurrentDeityChanged([CanBeNull] Deity deity)
        {
            OnCurrentDeityChange?.Invoke(this, new ChangedCurrentDeityEventUpdate(deity));
        }
        public event EventHandler<EventArgs> OnDeityListChange;
        protected virtual void OnDeityListChanged()
        {
            OnDeityListChange?.Invoke(this, new EventArgs());
        }

        private DeityFactory(string basePath)
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

        public void CreateDeity(string name)
        {
            var deity = new Deity(NextDeityIdentifier, name);
            if (_collection.deities == null) _collection.deities = new List<Deity>();
            _collection.deities.Add(deity);
            SaveDeities();
            OnDeityListChanged();
        }

        public bool DeleteDeity(int identifier)
        {
            if (identifier >= NextDeityIdentifier) return false;
            if (!_collection.deities.Remove(new Deity(identifier))) return false;
            SaveDeities();
            OnDeityListChanged();
            return true;
        }

        public void UpdateDeity(Deity deity)
        {
            _collection.deities.Remove(deity);
            _collection.deities.Add(deity);
            SaveDeities();
            OnDeityListChanged();
        }

        [CanBeNull]
        public Deity GetDeity(int identifier) => identifier >= NextDeityIdentifier
            ? null
            : _collection.deities.Find(deity => deity.identifier == identifier);

        public IEnumerable<Deity> GetDeities()
        {
            return _collection.deities;
        }
        
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