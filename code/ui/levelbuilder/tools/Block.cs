using Sandbox.UI;
using Sandbox.UI.Construct;

using TerryBros.LevelElements;

namespace TerryBros.UI.LevelBuilder.Tools
{
    [UseTemplate]
    public class Block : Panel
    {
        public Panel Wrapper { get; set; }

        public Panel ImageWrapper { get; set; }
        public Image Image { get; set; }
        public Label TextLabel { get; set; }

        public BlockAsset Asset { get; set; }

        public Block(Panel parent = null, BlockAsset asset = null) : base(parent)
        {
            Asset = asset;

            Image = ImageWrapper.Add.Image(Asset.IconPath, "image");
            TextLabel.Text = Asset.DisplayName;
            TextLabel.Style.Set("opacity", "0");

            AddEventListener("onclick", (e) =>
            {
                BlockSelector.Instance.Select(Asset.Name);
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
