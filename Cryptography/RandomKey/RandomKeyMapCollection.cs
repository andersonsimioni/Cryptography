using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hasbots.Classes.Cryptographic.RandomKeyMap
{
    public static class RandomKeyMapCollection
    {
        private static readonly string MapsDirectory = "RandomKeyMaps";

        private static Dictionary<string, Map> Maps = null;

        /// <summary>
        /// Load all maps from saved maps in files of server
        /// </summary>
        /// <returns>old saved maps</returns>
        private static void loadAllMapsFromServerPath() 
        {
            if (Maps != null)
                return;

            //ServerDirectory.Directories.createDirectory(MapsDirectory);

            var mapsLoaded = new Dictionary<string, Map>();
            //var mapsFiles = ServerDirectory.Directories.getFiles(MapsDirectory);

           // foreach (var item in mapsFiles)
               // mapsLoaded.Add(item, Map.loadFromFile($"{MapsDirectory}\\{item}"));

            Maps = mapsLoaded;
        }

        /// <summary>
        /// Save all maps in file on server path
        /// </summary>
        private static void saveAllMapsInServerPath() 
        {
            foreach (var item in Maps)
                item.Value.save($"{MapsDirectory}\\{item.Key}");        
        }

        /// <summary>
        /// Create a new map
        /// </summary>
        /// <param name="name"></param>
        public static void createMap(string name) 
        {
            loadAllMapsFromServerPath();

            if (Maps.ContainsKey(name))
                return;

            Maps.Add(name, Map.createNew());

            //saveAllMapsInServerPath();
        }

        /// <summary>
        /// Call creation key function in map
        /// </summary>
        /// <param name="map">name of map</param>
        /// <param name="data">data to be associated with a key</param>
        /// <returns>new key name in map</returns>
        public static string addKeyData(string map, string data) 
        {
            createMap(map);

            if (!Maps.ContainsKey(map))
                throw new Exception("Map not exist!");

            var newKeyName = Maps[map].addKey(data);
            //saveAllMapsInServerPath();

            return newKeyName;
        }

        /// <summary>
        /// Return the data of key
        /// </summary>
        /// <param name="map">map of key</param>
        /// <param name="key">key name</param>
        public static string getData(string map, string key) 
        {
            createMap(map);

            if (!Maps.ContainsKey(map))
                throw new Exception("Map not exist!");

            return Maps[map].getData(key);
        }
    }
}
