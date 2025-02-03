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

        public override void Start(ICoreAPI api)
        {
            api.RegisterItemClass($"{Mod.Info.ModID}.helveaxe", typeof(HelveAxe));
            
            if (!Harmony.HasAnyPatches(Mod.Info.ModID))
            {
                harmony = new Harmony(Mod.Info.ModID);
                harmony.PatchAllUncategorized();
            }
        }

        public override void Dispose() => harmony?.UnpatchAll(Mod.Info.ModID);
    }
}