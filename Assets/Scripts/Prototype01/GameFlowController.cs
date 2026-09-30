using TMPro;
using UnityEngine;

namespace Sever6.Prototype01
{
    public sealed class GameFlowController : MonoBehaviour
    {
        private const int MinutesPerTransit = 15;

        [Header("Screen Roots")]
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject controlRoomScreen;
        [SerializeField] private GameObject stationMapScreen;
        [SerializeField] private GameObject transitScreen;
        [SerializeField] private GameObject generatorRoomScreen;

        [Header("Runtime UI")]
        [SerializeField] private TMP_Text controlRoomClockText;
        [SerializeField] private TMP_Text stationMapLocationText;
        [SerializeField] private TMP_Text transitDetailText;

        private enum Location
        {
            ControlRoom,
            GeneratorRoom
        }

        private Location currentLocation = Location.ControlRoom;
        private Location transitDestination = Location.ControlRoom;
        private int elapsedMinutes;

        private void Awake()
        {
            ResetShift();
            ShowStartScreen();
        }

        public void BeginShift()
        {
            ResetShift();
            ShowControlRoom();
        }

        public void OpenStationMap()
        {
            RefreshUi();
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
            elapsedMinutes += MinutesPerTransit;
            currentLocation = transitDestination;
            RefreshUi();
            ShowCurrentLocation();
        }

        private void ResetShift()
        {
            currentLocation = Location.ControlRoom;
            transitDestination = Location.ControlRoom;
            elapsedMinutes = 0;
            RefreshUi();
        }

        private void BeginTransit(Location destination)
        {
            if (destination == currentLocation)
            {
                ShowCurrentLocation();
                return;
            }

            transitDestination = destination;
            RefreshUi();
            ShowOnly(transitScreen);
        }

        private void ShowCurrentLocation()
        {
            RefreshUi();

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
            RefreshUi();
            ShowOnly(controlRoomScreen);
        }

        private void ShowGeneratorRoom()
        {
            RefreshUi();
            ShowOnly(generatorRoomScreen);
        }

        private void RefreshUi()
        {
            if (controlRoomClockText != null)
            {
                controlRoomClockText.text = $"SHIFT TIME: {FormatTime(elapsedMinutes)}";
            }

            if (stationMapLocationText != null)
            {
                stationMapLocationText.text =
                    $"CURRENT LOCATION: {FormatLocation(currentLocation)}";
            }

            if (transitDetailText != null)
            {
                transitDetailText.text =
                    $"REMOTE SYSTEMS UNAVAILABLE.\nTIME ADVANCED: +{MinutesPerTransit:00} MIN";
            }
        }

        private static string FormatTime(int totalMinutes)
        {
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;
            return $"{hours:00}:{minutes:00}";
        }

        private static string FormatLocation(Location location)
        {
            return location switch
            {
                Location.ControlRoom => "CONTROL ROOM",
                Location.GeneratorRoom => "GENERATOR ROOM",
                _ => "UNKNOWN"
            };
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