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
        private object activeTool;

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            BECuttingBoard becuttingBoard = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BECuttingBoard;

            if (becuttingBoard == null) return base.OnBlockInteractStart(world, byPlayer, blockSel);
            if (byPlayer.InventoryManager.ActiveHotbarSlot.Empty && becuttingBoard.Inventory[0].Empty) return base.OnBlockInteractStart(world, byPlayer, blockSel);
            return becuttingBoard.OnInteract(byPlayer, blockSel);

            CollectibleObject colObj = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack?.Collectible;
            if (colObj != null && colObj.HasBehavior<BehaviorFoodCutter>() && !becuttingBoard.Inventory.Empty)
            {
                byPlayer.Entity.StartAnimation("knifecut");
                return true;
            }

        }

        public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            CollectibleObject colObj = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack?.Collectible;
           BECuttingBoard becuttingboard = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BECuttingBoard;
           BlockPos pos = blockSel.Position;
           EnumTool enumTool = EnumTool.Knife;
           float num;

            if (colObj != null && colObj.HasBehavior<BehaviorFoodCutter>() && becuttingboard.Inventory.Empty)
          {
              
          }

           return false;
      }
    }
}
