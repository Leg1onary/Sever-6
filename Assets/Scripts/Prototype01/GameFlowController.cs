using TMPro;
using UnityEngine;

namespace Sever6.Prototype01
{
    public sealed class GameFlowController : MonoBehaviour
    {
        private const int MinutesPerTransit = 15;
        private const int MaxPower = 5;
        private const int StartingPower = 4;

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
        [SerializeField] private TMP_Text powerText;
        [SerializeField] private TMP_Text commsText;
        [SerializeField] private TMP_Text camerasText;
        [SerializeField] private TMP_Text systemMessageText;

        private enum Location
        {
            ControlRoom,
            GeneratorRoom
        }

        private enum CommsStatus
        {
            Online,
            Degrading
        }

        private enum CameraStatus
        {
            Online
        }

        private Location currentLocation = Location.ControlRoom;
        private Location transitDestination = Location.ControlRoom;
        private CommsStatus commsStatus = CommsStatus.Online;
        private CameraStatus cameraStatus = CameraStatus.Online;

        private int elapsedMinutes;
        private int availablePower;
        private int completedTransitCount;
        private string systemMessage;

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
            completedTransitCount++;
            elapsedMinutes += MinutesPerTransit;
            availablePower = Mathf.Max(0, availablePower - 1);
            currentLocation = transitDestination;

            ApplyTransitConsequences();
            RefreshUi();
            ShowCurrentLocation();
        }

        private void ResetShift()
        {
            currentLocation = Location.ControlRoom;
            transitDestination = Location.ControlRoom;
            commsStatus = CommsStatus.Online;
            cameraStatus = CameraStatus.Online;

            elapsedMinutes = 0;
            availablePower = StartingPower;
            completedTransitCount = 0;
            systemMessage = "SYSTEM READY. AWAITING OPERATOR INPUT.";

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

        private void ApplyTransitConsequences()
        {
            if (completedTransitCount == 1 &&
                currentLocation == Location.GeneratorRoom)
            {
                commsStatus = CommsStatus.Degrading;
                systemMessage = "COMMS WARNING: SIGNAL DEGRADING.";
                return;
            }

            if (currentLocation == Location.ControlRoom &&
                commsStatus == CommsStatus.Degrading)
            {
                systemMessage = "COMMS REQUIRES MANUAL SERVICE IN GENERATOR ROOM.";
            }
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

            if (powerText != null)
            {
                powerText.text = $"POWER: {availablePower} / {MaxPower}";
            }

            if (commsText != null)
            {
                commsText.text = $"COMMS: {FormatCommsStatus(commsStatus)}";
            }

            if (camerasText != null)
            {
                camerasText.text = $"CAMERAS: {FormatCameraStatus(cameraStatus)}";
            }

            if (systemMessageText != null)
            {
                systemMessageText.text = systemMessage;
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

        private static string FormatCommsStatus(CommsStatus status)
        {
            return status switch
            {
                CommsStatus.Online => "ONLINE",
                CommsStatus.Degrading => "DEGRADING",
                _ => "UNKNOWN"
            };
        }

        private static string FormatCameraStatus(CameraStatus status)
        {
            return status switch
            {
                CameraStatus.Online => "ONLINE",
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