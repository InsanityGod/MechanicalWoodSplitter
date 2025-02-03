using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using Vintagestory.GameContent.Mechanics;

namespace MechanicalWoodSplitter.Code.HarmonyPatches
{
    [HarmonyPatch(typeof(BlockHelveHammer), "OnBlockInteractStart")]
    public static class PlaceAxeOnHelveBasePatch
    {
        public static bool Prefix(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, BlockHelveHammer __instance, ref bool __result)
        {
            if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BEHelveHammer bEHelveHammer && bEHelveHammer.HammerStack == null && !byPlayer.InventoryManager.ActiveHotbarSlot.Empty && byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack.Collectible.FirstCodePart().Equals("helveaxe"))
            {
                bEHelveHammer.HammerStack = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack.Clone();
                bEHelveHammer.MarkDirty();
                if (byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative)
                {
                    byPlayer.InventoryManager.ActiveHotbarSlot.TakeOut(1);
                }

                var api = Traverse.Create(__instance).Field("api").GetValue<ICoreAPI>();
                byPlayer.InventoryManager.ActiveHotbarSlot.MarkDirty();
                api.World.PlaySoundAt(new AssetLocation("sounds/player/build"), blockSel.Position.X + 0.5, blockSel.Position.Y + 0.5, blockSel.Position.Z + 0.5, null, 0.88f + (float)api.World.Rand.NextDouble() * 0.24f, 16f);
                __result = true;
                return false;
            }

            return true;
        }
    }
}