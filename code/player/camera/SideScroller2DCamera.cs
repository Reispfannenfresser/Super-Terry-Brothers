using System;

using Sandbox;

using TerryBros.Gamemode;
using TerryBros.Settings;

namespace TerryBros.Player.Camera
{
    public partial class SideScroller2DCamera : Sandbox.Camera
    {
        public float FreeCameraSpeed = 500f;
        public Vector3 Position
        {
            get => GlobalSettings.ConvertGlobalToLocalCoordinates(Pos);
            set { Pos = GlobalSettings.ConvertLocalToGlobalCoordinates(value); }
        }

        private int _distanceInBlocks = 10;
        private float _orthoSize = 0.3f;
        private int _visibleGroundBlocks = 3;

        public override void Update()
        {
            if (Local.Pawn is not TerryBrosPlayer player)
            {
                return;
            }

            if (!player.IsInLevelBuilder)
            {
                BBox bBox = STBGame.CurrentLevel.LevelBoundsLocal;
                OrthoSize = Math.Min((bBox.Maxs.x - bBox.Mins.x) / Screen.Width, (bBox.Maxs.y - bBox.Mins.y) / Screen.Height);
                OrthoSize = Math.Min(_orthoSize, OrthoSize);

                //TODO: Use Local for player and ground directly
                Vector3 newPos = GlobalSettings.ConvertGlobalToLocalCoordinates(new(player.Position.x, player.Position.y, GlobalSettings.GroundPos.z));
                newPos.y -= GlobalSettings.BlockSize * _visibleGroundBlocks;
                newPos.y += Screen.Height / 2 * OrthoSize;
                newPos.z -= GlobalSettings.BlockSize * _distanceInBlocks;

                // horizontal camera movement
                newPos.x = Math.Clamp(newPos.x, bBox.Mins.x + Screen.Width / 2 * OrthoSize, bBox.Maxs.x - Screen.Width / 2 * OrthoSize);

                // vertical camera movement
                newPos.y = Math.Clamp(newPos.y, bBox.Mins.y + Screen.Height / 2 * OrthoSize, bBox.Maxs.y - Screen.Height / 2 * OrthoSize);
                
                Position = newPos;
            }

            Rot = Rotation.LookAt(GlobalSettings.LookDir, GlobalSettings.UpwardDir);

            Ortho = true;

            Viewer = null;
        }
    }
}
