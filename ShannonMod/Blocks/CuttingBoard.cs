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
    { // Creates new class CuttingBoard that inherits methods and variables of class Block

        // Overrides parent method OnBlockInteractStart, using the CuttingBoard class OnBlockInteractStart method instead
        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {  
            // Defining new variable. Setting variable becuttingBoard to the value of EntityBlock class BECuttingBoard when block is placed in world.
            BECuttingBoard becuttingBoard = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BECuttingBoard;
            //Conditional statement. If the variable is null, then returns base class Cuttingboard Block
            if (becuttingBoard == null) return base.OnBlockInteractStart(world, byPlayer, blockSel);
            //Conditional statement. If the Player's hand slot is empty and the EBCuttingBoard block has 0 inventory, then return base class CuttingBoard block.
            if (byPlayer.InventoryManager.ActiveHotbarSlot.Empty && becuttingBoard.Inventory[0].Empty) return base.OnBlockInteractStart(world, byPlayer, blockSel);
            //Defining new variable. Setting variable colObj to the value of Player's itemstack in current active handslot. 
            CollectibleObject colObj = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack?.Collectible;
            //Conditional statement. If the variable colObj is not null, contains the attribute "foodcutter", and is currently in the EBCuttingBoard inventory slot, then returns method start animation "axechop" as true" 
            if (colObj != null && colObj.Attributes["foodcutter"].AsBool(false) && !becuttingBoard.Inventory.Empty)
            {
                byPlayer.Entity.StartAnimation("axechop");
                return true;
            }
            api.Logger.Debug("Checkpoint Pealope");
            //If none of those conditions are met, then returns EBCuttingBoard on interact" 
            return becuttingBoard.OnInteract(byPlayer, blockSel);
        }

        //Overrides parent method OnBlockInteractStep, using the CuttingBoard class OnBlockInteractStep method instead
        public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        { //Defining new variables for the method OnBlockInteractStep. colObj =  player's itemstack in active slot, becuttingboard = BECuttingBoard, and pos = Block's position in the world when selected.
            CollectibleObject colObj = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack?.Collectible;
            BECuttingBoard becuttingboard = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BECuttingBoard;
            BlockPos pos = blockSel.Position;
            api.Logger.Debug("Checkpoint Rat");
            //Conditional statement with two conditional statement within method to provide appropriate return values. First conditional statement: if variable becuttingboard does not have an empty inventory. 
            if (!becuttingboard.Inventory[0].Empty)
            {   //Second conditional statement. If the sound played is less than the seconds used, then play sound "chop2" and play next sound at 1.7f pace"
                if (playNextSound < secondsUsed)
                {
                    api.Logger.Debug("Checkpoint Chicken");
                    api.World.PlaySoundAt(new AssetLocation("sounds/block/chop2"), pos.X, pos.Y, pos.Z, null, true, 32, 1f);
                    playNextSound += 1.7f;
                }
                //Third conditional statement. If seconds used is greater than int 2, then retrieve item carrot, clear BECuttingBlock inventory, update visual meshes, triggers block change event, and returns false. 
                if (secondsUsed >= 2)
                {
                    ItemStack stack = new ItemStack(api.World.GetItem(new AssetLocation("vegetable-carrot")));
                    becuttingboard.Inventory.Clear();
                    byPlayer.InventoryManager.TryGiveItemstack(stack);
                    becuttingboard.updateMeshes();
                    becuttingboard.MarkDirty(true);
                    return false;
                }
                //If none of the secondary and third conditions are met, return true.
                return true;
            }
            //If the first condition isn't met, return false. 
            return false;
        }

        //Overrides parent method OnBlockInteractStop, using the CuttingBoard class OnBlockInteractStop method instead
        public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {// Within method, defines variable playNextSound as float value, then calls method StopAnimation to stop animation "axechop"
            api.Logger.Debug("Checkpoint Lion");
            playNextSound = 0.7f;
            byPlayer.Entity.StopAnimation("axechop");
        }
        // New variable defined for all methods referencing variable playNextSound
        private float playNextSound;
    }

}
        
