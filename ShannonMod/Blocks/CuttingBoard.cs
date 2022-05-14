using ShannonMod.BlockEntities;
using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common;

namespace ShannonMod.Blocks
{
    class CuttingBoard : Block {
        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            BECuttingBoard becuttingBoard = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BECuttingBoard;

            if (becuttingBoard == null) return base.OnBlockInteractStart(world, byPlayer, blockSel);
            if (byPlayer.InventoryManager.ActiveHotbarSlot.Empty && becuttingBoard.Inventory[0].Empty) return base.OnBlockInteractStart(world, byPlayer, blockSel);
            return becuttingBoard.OnInteract(byPlayer, blockSel);
        }


    }
}
