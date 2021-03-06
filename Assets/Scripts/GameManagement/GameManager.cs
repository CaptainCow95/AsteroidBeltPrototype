﻿using AsteroidBelt.ShipComponents;
using AsteroidBelt.StationComponents;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsteroidBelt
{
    public class GameManager : Singleton<GameManager>
    {
        public List<AudioClip> backgroundAudioClips;
        public GameObject[] shipComponentPrefabs;
        public GameObject shipPrefab;
        public List<ShipPart> ShipToLoad;
        public List<ShipPart> ShipToLoadDefault;
        public GameObject[] stationComponentPrefabs;
        public GameObject stationPrefab;
        public int totalCredits;
        public GameObject WaypointPrefab;
        private List<GameObject> persistingObjects = new List<GameObject>();
        private Queue<AudioClip> playedAudioClips = new Queue<AudioClip>();

        public GameObject CreateShip(Vector2 position, Vector2[] componentPositions, ShipComponent.Direction[] componentDirections, ShipComponentType[] shipComponents, bool playerControlled)
        {
            GameObject shipObject = (GameObject)Instantiate(shipPrefab, position, Quaternion.identity);
            Ship ship = shipObject.GetComponent<Ship>();
            ship.playerControlled = playerControlled;
            for (int i = 0; i < shipComponents.Length; ++i)
            {
                GameObject newShipComponent = (GameObject)Instantiate(shipComponentPrefabs[(int)shipComponents[i]], componentPositions[i], Quaternion.identity);
                ShipComponent comp = newShipComponent.GetComponent<ShipComponent>();
                comp.ParentShip = shipObject;
                comp.ComponentDirection = componentDirections[i];
                ship.AddShipComponent(comp);
            }

            return shipObject;
        }

        public GameObject CreateShip(Vector2 position, List<ShipPart> shipParts, bool playerControlled)
        {
            Vector2[] componentPositions = new Vector2[shipParts.Count];
            ShipComponent.Direction[] componentDirections = new ShipComponent.Direction[shipParts.Count];
            ShipComponentType[] shipComponents = new ShipComponentType[shipParts.Count];

            for (int i = 0; i < shipParts.Count; ++i)
            {
                componentPositions[i] = shipParts[i].Location;
                componentDirections[i] = shipParts[i].Direction;
                shipComponents[i] = shipParts[i].ShipComponent;
            }
            return CreateShip(position, componentPositions, componentDirections, shipComponents, playerControlled);
        }

        public GameObject CreateStation(Vector2 position, Vector2[] componentPositions, ShipComponent.Direction[] componentDirections, ShipComponentType[] stationComponents)
        {
            GameObject stationObject = (GameObject)Instantiate(stationPrefab, position, Quaternion.identity);
            Station station = stationObject.GetComponent<Station>();
            for (int i = 0; i < stationComponents.Length; ++i)
            {
                GameObject newStationComponent = (GameObject)Instantiate(stationComponentPrefabs[(int)stationComponents[i]], componentPositions[i], Quaternion.identity);
                StationComponent component = newStationComponent.GetComponent<StationComponent>();
                component.ParentStation = stationObject;
                component.ComponentDirection = componentDirections[i];
                station.AddStationComponent(component);
            }

            DontDestroyOnLoad(stationObject);
            persistingObjects.Add(stationObject);
            return stationObject;
        }

        public void CreateWaypoint(Vector2 location)
        {
            GameObject waypointObject = Instantiate(WaypointPrefab);
            waypointObject.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
            Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
            waypoint.Location = location;
            persistingObjects.Add(waypointObject);
            DontDestroyOnLoad(waypointObject);
        }

        public void SetShipToLoad(List<ShipPart> shipToLoad)
        {
            ShipToLoad = shipToLoad;
        }

        public void SpawnDefaultPlayerShip()
        {
            CreateShip(new Vector2(0, 0), ShipToLoadDefault, true);
        }

        private void Awake()
        {
            if (instance == null || instance == this)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Initialize()
        {
            PlayBackgroundMusic();
        }

        private void OnLevelWasLoaded(int level)
        {
            if (level == 0 || level == 2)
            {
                return;
            }

            if (ShipToLoad != null && ShipToLoad.Any())
            {
                CreateShip(new Vector2(0, 0), ShipToLoad, true);
                ShipToLoad = null;
            }
        }

        private void PlayBackgroundMusic()
        {
            int index = Random.Range(0, backgroundAudioClips.Count);
            while (playedAudioClips.Contains(backgroundAudioClips[index]))
            {
                index = Random.Range(0, backgroundAudioClips.Count);
            }

            GetComponent<AudioSource>().clip = backgroundAudioClips[index];
            GetComponent<AudioSource>().Play();
            playedAudioClips.Enqueue(backgroundAudioClips[index]);
            while (playedAudioClips.Count > backgroundAudioClips.Count / 2)
            {
                playedAudioClips.Dequeue();
            }
        }

        private void Update()
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                PlayBackgroundMusic();
            }
        }
    }
}