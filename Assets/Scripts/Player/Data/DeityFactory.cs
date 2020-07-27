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
        private static readonly object LockCurrentDeity = new object();
        private static readonly object LockDeityCollection = new object();

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
            get
            {
                lock (LockCurrentDeity)
                {
                    return _currentDeity;
                }
            }
            set
            {
                if (_currentDeity == value) return;
                lock (LockCurrentDeity)
                {
                    _currentDeity = value;
                    OnCurrentDeityChanged(value);
                }
            }
        }

        public event EventHandler<DeityEventArgs> OnCurrentDeityChange;

        private void OnCurrentDeityChanged([CanBeNull] Deity deity)
        {
            OnCurrentDeityChange?.Invoke(this, new DeityEventArgs(deity));
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
            lock (LockDeityCollection)
            {
                var deity = new Deity(NextDeityIdentifier, name);
                if (!_deities.TryAdd(deity.identifier, deity)) return;
                _currentMaxIdentifier += 1;
                CurrentDeity = deity;
                SaveDeities();
            }
        }

        public bool DeleteDeity()
        {
            if (CurrentDeity == null) return false;
            lock (LockCurrentDeity)
            {
                lock (LockDeityCollection)
                {
                    if (!_deities.TryRemove(CurrentDeity.identifier, out _)) return false;
                    CurrentDeity = null;
                    SaveDeities();
                    return true;
                }
            }
        }

        public void UpdateDeity(string newName)
        {
            lock (LockCurrentDeity)
            {
                lock (LockDeityCollection)
                {
                    var newDeity = new Deity(CurrentDeity.identifier, newName, CurrentDeity.currentPowerPoints);
                    if (!_deities.TryUpdate(CurrentDeity.identifier, newDeity,  CurrentDeity)) return;
                    CurrentDeity = newDeity;
                    SaveDeities();
                }
            }
        }

        public Deity GetDeity(int identifier)
        {
            lock (LockDeityCollection)
            {
                return _deities[identifier];
            }
        }

        public IEnumerable<Deity> GetDeities()
        {
            lock (LockDeityCollection)
            {
                return _deities.Values;
            }
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
            [CanBeNull] public List<Deity> deities;

            public ConcurrentDictionary<int, Deity> ToConcurrentDictionary()
            {
                if (deities != null)
                    return new ConcurrentDictionary<int, Deity>(deities.Select(deity =>
                        new KeyValuePair<int, Deity>(deity.identifier, deity)));
                else
                    return new ConcurrentDictionary<int, Deity>();
            }
        }
    }
}