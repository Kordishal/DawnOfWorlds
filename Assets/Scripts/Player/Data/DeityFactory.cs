using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Meta.EventArgs;
using Model.Deity;
using UnityEngine;

namespace Player.Data
{
    public sealed class DeityFactory
    {
        private static DeityFactory _factory;

        public static DeityFactory GetInstance(string path)
        {
            return _factory ?? (_factory = new DeityFactory(path));
        }

        private const string FileName = "deities.json";
        private const string DirectoryName = "deities";

        private int _currentMaxIdentifier = 0;

        private int NextDeityIdentifier => _currentMaxIdentifier + 1;

        private readonly string _saveFile;
        private readonly ConcurrentDictionary<int, Deity> _deities;

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

        private void OnCurrentDeityChanged([CanBeNull] Deity deity)
        {
            OnCurrentDeityChange?.Invoke(this, new ChangedCurrentDeityEventUpdate(deity));
        }

        public event EventHandler<EventArgs> OnDeityListChange;

        private void OnDeityListChanged()
        {
            OnDeityListChange?.Invoke(this, new EventArgs());
        }

        private DeityFactory(string basePath)
        {
            var directory = Path.Combine(basePath, DirectoryName);
            _saveFile = Path.Combine(directory, FileName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            if (!File.Exists(_saveFile))
                File.Create(_saveFile).Close();
            var deityCollection = JsonUtility.FromJson<DeityCollection>(File.ReadAllText(_saveFile)) ??
                                  new DeityCollection();
            _deities = deityCollection.ToConcurrentDictionary();
            foreach (var deity in GetDeities())
            {
                if (deity.identifier > _currentMaxIdentifier)
                {
                    _currentMaxIdentifier = deity.identifier;
                }
            }
        }

        public void CreateDeity(string name)
        {
            var deity = new Deity(NextDeityIdentifier, name);
            if (_deities.TryAdd(deity.identifier, deity))
            {
                _currentMaxIdentifier += 1;
                CurrentDeity = deity;
                SaveDeities();
            }
        }

        public bool DeleteDeity(int identifier)
        {
            if (!_deities.TryRemove(identifier, out _)) return false;
            CurrentDeity = null;
            SaveDeities();
            return true;
        }

        public void UpdateDeity(Deity deity)
        { 
            if (_deities.TryUpdate(deity.identifier, deity, _deities[deity.identifier]))
            {
                CurrentDeity = deity;
                SaveDeities();
            }
        }
        public Deity GetDeity(int identifier)
        {
            return _deities[identifier];
        }

        public IEnumerable<Deity> GetDeities()
        {
            return _deities.Values;
        }

        private void SaveDeities()
        {
            var collection = new DeityCollection
            {
                deities = _deities.Values.ToList()
            };

            File.WriteAllText(_saveFile, JsonUtility.ToJson(collection));
            OnDeityListChanged();
        }

        [Serializable]
        private class DeityCollection
        {
            public List<Deity> deities;

            public ConcurrentDictionary<int, Deity> ToConcurrentDictionary()
            {
                return new ConcurrentDictionary<int, Deity>(deities.Cast<KeyValuePair<int, Deity>>());
            }
        }
    }
}