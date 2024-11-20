using HarmonyLib;
using MechanicalWoodSplitter.HarmonyPatches;
using MechanicalWoodSplitter.Items;
using System.Security.Cryptography.X509Certificates;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.GameContent.Mechanics;

namespace MechanicalWoodSplitter
{
    public class MechanicalWoodSplitterModSystem : ModSystem
    {
        private Harmony harmony;

        // Called on server and client
        // Useful for registering block/entity classes on both sides
        public override void Start(ICoreAPI api)
        {
            if (!Harmony.HasAnyPatches(Mod.Info.ModID))
            {
                harmony = new Harmony(Mod.Info.ModID);
                harmony.PatchAll(); // Applies all harmony patches
                var choppingBlockType = AccessTools.TypeByName("InDappledGroves.BlockEntities.IDGBEChoppingBlock");
                var choppingBlockConstructor = AccessTools.Constructor(choppingBlockType);
                harmony.Patch(choppingBlockConstructor, postfix: new HarmonyMethod(typeof(FixItemStackSizeOfChoppingBlock).GetMethod(nameof(FixItemStackSizeOfChoppingBlock.PostFix))));
            }
            api.RegisterItemClass($"{Mod.Info.ModID}.helveaxe", typeof(HelveAxe));
        }

        public override void Dispose() => harmony?.UnpatchAll(Mod.Info.ModID);
    }
}