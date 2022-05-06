using Sandbox.UI;
using Sandbox.UI.Construct;

using TerryBros.LevelElements;

namespace TerryBros.UI.LevelBuilder
{
    public class Block : Panel
    {
        public Label TextLabel;
        public Image Image;

        public BlockAsset Asset { get; set; }

        public Block(Panel parent = null, BlockAsset asset = null) : base(parent)
        {
            Asset = asset;

            Image = Add.Image(Asset.IconPath, "image");
            TextLabel = Add.Label(Asset.DisplayName, "name");
            TextLabel.Style.Set("opacity", "0");

            AddEventListener("onclick", (e) =>
            {
                BuildPanel.Instance.BlockSelection.Select(Asset.Name);
            });
            AddEventListener("onmouseover", (e) =>
            {
                TextLabel.Style.Set("opacity", "1");
            });
            AddEventListener("onmouseout", (e) =>
            {
                TextLabel.Style.Set("opacity", "0");
            });
        }
    }
}
