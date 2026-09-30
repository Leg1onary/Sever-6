using UnityEngine;

namespace Sever6.Prototype01
{
    public sealed class GameFlowController : MonoBehaviour
    {
        [Header("Screen Roots")]
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject controlRoomScreen;
        [SerializeField] private GameObject stationMapScreen;
        [SerializeField] private GameObject transitScreen;
        [SerializeField] private GameObject generatorRoomScreen;

        private enum Location
        {
            ControlRoom,
            GeneratorRoom
        }

        private Location currentLocation = Location.ControlRoom;
        private Location transitDestination = Location.ControlRoom;

        private void Awake()
        {
            ShowStartScreen();
        }

        public void BeginShift()
        {
            currentLocation = Location.ControlRoom;
            ShowControlRoom();
        }

        public void OpenStationMap()
        {
            ShowOnly(stationMapScreen);
        }

        public void CloseStationMap()
        {
            ShowCurrentLocation();
        }

        public void TravelToControlRoom()
        {
            BeginTransit(Location.ControlRoom);
        }

        public void TravelToGeneratorRoom()
        {
            BeginTransit(Location.GeneratorRoom);
        }

        public void ContinueTransit()
        {
            currentLocation = transitDestination;
            ShowCurrentLocation();
        }

        private void BeginTransit(Location destination)
        {
            if (destination == currentLocation)
            {
                ShowCurrentLocation();
                return;
            }

            transitDestination = destination;
            ShowOnly(transitScreen);
        }

        private void ShowCurrentLocation()
        {
            switch (currentLocation)
            {
                case Location.ControlRoom:
                    ShowControlRoom();
                    break;

                case Location.GeneratorRoom:
                    ShowGeneratorRoom();
                    break;
            }
        }

        private void ShowStartScreen()
        {
            ShowOnly(startScreen);
        }

        private void ShowControlRoom()
        {
            ShowOnly(controlRoomScreen);
        }

        private void ShowGeneratorRoom()
        {
            ShowOnly(generatorRoomScreen);
        }

        private void ShowOnly(GameObject activeScreen)
        {
            startScreen.SetActive(activeScreen == startScreen);
            controlRoomScreen.SetActive(activeScreen == controlRoomScreen);
            stationMapScreen.SetActive(activeScreen == stationMapScreen);
            transitScreen.SetActive(activeScreen == transitScreen);
            generatorRoomScreen.SetActive(activeScreen == generatorRoomScreen);
        }
    }
}