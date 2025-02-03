using HarmonyLib;
using MechanicalWoodSplitter.Code.EntityBehaviors;
using MechanicalWoodSplitter.Code.Items;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.GameContent.Mechanics;

namespace MechanicalWoodSplitter.Code
{
    public class MechanicalWoodSplitterModSystem : ModSystem
    {
        private Harmony harmony;

        public override void Start(ICoreAPI api)
        {
            api.RegisterItemClass($"{Mod.Info.ModID}.helveaxe", typeof(HelveAxe));
            api.RegisterBlockEntityBehaviorClass($"{Mod.Info.ModID}.HelveAxeBaseBlockEntityBehavior", typeof(HelveAxeBaseBlockEntitybehavior));
            if (!Harmony.HasAnyPatches(Mod.Info.ModID))
            {
                harmony = new Harmony(Mod.Info.ModID);
                harmony.PatchAllUncategorized();
            }
        }

        public override void AssetsFinalize(ICoreAPI api)
        {
            foreach(var helveHammerBase in api.World.Blocks.OfType<BlockHelveHammer>())
            {
                helveHammerBase.BlockEntityBehaviors = helveHammerBase.BlockEntityBehaviors.Append(new BlockEntityBehaviorType()
                {
                    Name = $"{Mod.Info.ModID}.HelveAxeBaseBlockEntityBehavior"
                });
            }
        }

        public override void Dispose() => harmony?.UnpatchAll(Mod.Info.ModID);
    }
}