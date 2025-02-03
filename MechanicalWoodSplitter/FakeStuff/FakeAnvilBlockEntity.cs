using HarmonyLib;
using InDappledGroves.BlockEntities;
using MechanicalWoodSplitter.Items;
using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace MechanicalWoodSplitter.FakeStuff
{
    public class FakeBlockEntityAnvil : BlockEntityAnvil
    {
        public IDGBEWorkstation ChoppingBlock { get; internal set; }
        
        public readonly HelveAxe HelveAxe;

        //caching this since someone made their config class internal -_-
        public float? baseWorkstationMiningSpdMult_cache;

        public FakeBlockEntityAnvil(HelveAxe helveAxe) => HelveAxe = helveAxe;

        public override void OnHelveHammerHit()
        {
            if (Api.Side != EnumAppSide.Server || ChoppingBlock == null) return;
            ChoppingBlock.recipecomplete = false;
            if (ChoppingBlock.InputSlot.Empty) return;
            
            Api.World.PlaySoundAt(new AssetLocation("sounds/block/chop2"), ChoppingBlock.Pos.X, ChoppingBlock.Pos.Y, ChoppingBlock.Pos.Z, null);

            ChoppingBlock.recipeHandler.GetMatchingRecipes(
                Api.World,
                ChoppingBlock.InputSlot,
                "chopping",
                ChoppingBlock.Block.Attributes["inventoryclass"].ToString(),
                ChoppingBlock.Block.Attributes["workstationproperties"]["workstationtype"].ToString(),
                out var recipe
            );

            ChoppingBlock.recipeHandler.recipe = recipe;
            if (recipe == null) return;


            ProgressRecipe();
            ChoppingBlock.updateMeshes();
            ChoppingBlock.MarkDirty(true);
        }

        public bool ProgressRecipe()
        {
            if (Api.Side != EnumAppSide.Server) return false;
            baseWorkstationMiningSpdMult_cache ??= Traverse.Create(AccessTools.TypeByName("InDappledGroves.Util.Config.IDGToolConfig")).Property("Current").Property<float>("baseWorkstationResistanceMult").Value;
            
            ItemStack inputStack = ChoppingBlock.InputSlot.Itemstack;
            ChoppingBlock.recipeHandler.resistance = ((inputStack.Block != null) ? inputStack.Block.Resistance : inputStack.Item.Attributes["resistance"].AsFloat(0f)) * baseWorkstationMiningSpdMult_cache.Value;

            ChoppingBlock.recipeHandler.curDmgFromMiningSpeed = HelveAxe.HelveAxeDamage * (1f + baseWorkstationMiningSpdMult_cache.Value);
            ChoppingBlock.recipeHandler.currentMiningDamage += 0.5f * ChoppingBlock.recipeHandler.curDmgFromMiningSpeed;
            ChoppingBlock.recipeHandler.recipeProgress = ChoppingBlock.recipeHandler.currentMiningDamage / ChoppingBlock.recipeHandler.resistance;
            
            if (ChoppingBlock.recipeHandler.currentMiningDamage >= ChoppingBlock.recipeHandler.resistance)
            {
                FinishRecipe();
                return true;
            }

            return false;
        }

        public void FinishRecipe()
        {
            ChoppingBlock.recipecomplete = true;
            ItemStack resolvedItemstack = ChoppingBlock.recipeHandler.recipe.ReturnStack.ResolvedItemstack;
            var craftedStack = ChoppingBlock.recipeHandler.recipe.TryCraftNow(Api, ChoppingBlock.InputSlot);
            ChoppingBlock.InputSlot.Itemstack = null;
            
            if (resolvedItemstack.Collectible.FirstCodePart(0) != "air")
            {
                if(resolvedItemstack.Collectible.FirstCodePart(0) == craftedStack.Collectible.FirstCodePart(0))
                {
                    craftedStack.StackSize += 1;
                }
                else ChoppingBlock.InputSlot.Itemstack = resolvedItemstack.Clone();
            }
            
            SpawnOutput(craftedStack);

            ChoppingBlock.recipeHandler.clearRecipe(true);
        }

        public void SpawnOutput(ItemStack output)
		{
			for (int i = output.StackSize; i > 0; i--)
            {
				Api.World.SpawnItemEntity(new ItemStack(output.Collectible, 1), ChoppingBlock.Pos.ToVec3d(), new Vec3d(0.05000000074505806, 0.10000000149011612, 0.05000000074505806));
			}
		}
    }
}