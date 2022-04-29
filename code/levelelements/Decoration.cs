using Sandbox;

namespace TerryBros.LevelElements
{
    [Library("stb_decoration"), Hammer.Skip]
    public partial class Decoration : BlockEntity
    {
        public Decoration() : base()
        {
            CollisionGroup = CollisionGroup.Never;
        }
    }
}
