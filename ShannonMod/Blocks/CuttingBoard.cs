using ShannonMod.BlockEntities;
using ShannonMod.CollectibleBehaviors;
using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace ShannonMod.Blocks
{
    class CuttingBoard : Block
    {

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            BECuttingBoard becuttingBoard = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BECuttingBoard;

            if (becuttingBoard == null) return base.OnBlockInteractStart(world, byPlayer, blockSel);

            if (byPlayer.InventoryManager.ActiveHotbarSlot.Empty && becuttingBoard.Inventory[0].Empty) return base.OnBlockInteractStart(world, byPlayer, blockSel);

            CollectibleObject colObj = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack?.Collectible;
            
            if (colObj != null && colObj.Attributes["foodcutter"].AsBool(false) && !becuttingBoard.Inventory.Empty)
            {
                byPlayer.Entity.StartAnimation("axechop");
                return true;
            }
            api.Logger.Debug("Checkpoint Pealope");
            return becuttingBoard.OnInteract(byPlayer, blockSel);
        }

        public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            CollectibleObject colObj = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack?.Collectible;
            BECuttingBoard becuttingboard = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BECuttingBoard;
            BlockPos pos = blockSel.Position;
            api.Logger.Debug("Checkpoint Rat");

            if (!becuttingboard.Inventory[0].Empty)
            {
                if (playNextSound < secondsUsed)
                {
                    api.Logger.Debug("Checkpoint Chicken");
                    api.World.PlaySoundAt(new AssetLocation("sounds/block/chop2"), pos.X, pos.Y, pos.Z, null, true, 32, 1f);
                    playNextSound += 1.7f;
                }
                if (secondsUsed >= 2)
                {
                    ItemStack stack = new ItemStack(api.World.GetItem(new AssetLocation("vegetable-carrot")));
                    becuttingboard.Inventory.Clear();
                    byPlayer.InventoryManager.TryGiveItemstack(stack);
                    becuttingboard.updateMeshes();
                    becuttingboard.MarkDirty(true);
                    return false;
                }

                return true;
            }
            return false;
        }

        public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            api.Logger.Debug("Checkpoint Lion");
            playNextSound = 0.7f;
            byPlayer.Entity.StopAnimation("axechop");
        }

        private float playNextSound;
    }

}
        
