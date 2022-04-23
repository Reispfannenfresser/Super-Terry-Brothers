using System;
using System.Collections.Generic;
using System.Text.Json;

using Sandbox;

using TerryBros.Gamemode;

namespace TerryBros.Levels
{
    public abstract partial class Level
    {
        private static int _currentPacketHash = -1;
        private static int _packetCount;
        private static string[] _packetData;

        public static void ServerSendData(Dictionary<string, List<Vector2>> dict)
        {
            string levelDataJson = JsonSerializer.Serialize(dict);
            int splitLength = 150;
            int splitCount = (int) MathF.Ceiling((float) levelDataJson.Length / splitLength);

            for (int i = 0; i < splitCount; i++)
            {
                ServerSendPartialData(levelDataJson.GetHashCode(), i, splitCount, levelDataJson.Substring(splitLength * i, splitLength + Math.Min(0, levelDataJson.Length - splitLength * (i + 1))));
            }
        }

        [ServerCmd(Name = "stb_send_partialleveldata")]
        public static void ServerSendPartialData(int packetHash, int packetNum, int maxPackets, string partialLevelData)
        {
            if (!ConsoleSystem.Caller?.HasPermission("import") ?? true)
            {
                return;
            }

            ProceedPartialData(packetHash, packetNum, maxPackets, partialLevelData);
            ClientSendPartialData(packetHash, packetNum, maxPackets, partialLevelData);
        }

        [ClientRpc]
        public static void ClientSendPartialData(int packetHash, int packetNum, int maxPackets, string partialLevelData)
        {
            ProceedPartialData(packetHash, packetNum, maxPackets, partialLevelData);
        }

        public static void ProceedPartialData(int packetHash, int packetNum, int maxPackets, string partialLevelData)
        {
            if (_currentPacketHash != packetHash)
            {
                _packetCount = 0;
                _packetData = new string[maxPackets];

                _currentPacketHash = packetHash;
            }

            _packetData[packetNum] = partialLevelData;
            _packetCount++;

            if (_packetCount == maxPackets)
            {
                _currentPacketHash = -1;

                Clear();
                Import(JsonSerializer.Deserialize<Dictionary<string, List<Vector2>>>(string.Join("", _packetData)));

                if (Host.IsServer)
                {
                    STBGame.CurrentLevel?.Restart();
                    STBGame.ClientRestartLevel();

                    foreach (Client client in Client.All)
                    {
                        if (client.Pawn is Player player)
                        {
                            player.Respawn();
                        }
                    }
                }
            }
        }

        [ServerCmd(Name = "stb_clear")]
        public static void ServerClear()
        {
            Clear();
            ClientClear();
        }

        [ClientRpc]
        public static void ClientClear()
        {
            Clear();
        }
    }
}
