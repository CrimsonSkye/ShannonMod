﻿using ShannonMod.CollectibleBehaviors;
using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace ShannonMod.BlockEntities
{ // Creates new class BECuttingBoard that inherits methods and variables of class BlockEntityDisplay
    class BECuttingBoard : BlockEntityDisplay {
        // Defining variables. 
        public readonly InventoryGeneric inv;
        // Overrides variable Inventory to be equal to the value of int (using lambda)
        public override InventoryBase Inventory => inv;
        // Overrides variable InventoryClassName to be equal to the value of string "cuttingboard" (using lambda)
        public override string InventoryClassName => "cuttingboard";
        // Overrides variable AttributeTransformCode to be equal to the value of string "smCuttingBoardTransform" (using lambda)
        public override string AttributeTransformCode => "smCuttingBoardTransform";
        
        //Creates new method BECuttingBoard. 
        public BECuttingBoard()
         // Defining new variables within method. Both variables are equal to the value of the new instances of classes InventoryGeneric and MeshData. 
        { inv = new InventoryGeneric(1, "cuttingboard-slot", null, null);
            meshes = new MeshData[1];
        }

        //Overrides parent method Initialize, using the BECuttingBoard class Initialize method instead
        public override void Initialize(ICoreAPI api)
        {//Defines methods and variables. Calls Initialize method. Variable capi (Client API) is equal to the value of CoreClientAPI
            base.Initialize(api);
            this.capi = (api as ICoreClientAPI);
            //Conditional statement. If the client api is not null, the update visual meshes. 
            if (this.capi != null)
            {
                this.updateMeshes();
            }

        }
        //Accesses method OnInteract
        internal bool OnInteract(IPlayer byPlayer, BlockSelection blockSel)
        {//Defines variable. Variable activeHotbarSlot is equal to the value of Player's activehotbarslot. 
            ItemSlot activeHotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
            //Conditional statement. If the activeHotbarSlot is empty, then return instance of TryTake method when player interacts with EBCuttingBoard
            if (activeHotbarSlot.Empty)
            {
                return this.TryTake(byPlayer, blockSel);
            }
            //Defines variables. Variable collectible is equal to the value of Player's itemstack in activeHotbarSlot. Variable attribute is equal to the value of Attributes class
            CollectibleObject collectible = activeHotbarSlot.Itemstack.Collectible;
            JsonObject attributes = collectible.Attributes;
            //Conditional statement. If Itemstack is null, OR Itemstack does not have attributes "smCuttingBoardProps" and "cuttable", then return false.
            if(attributes==null || !attributes["smCuttingBoardProps"]["cuttable"].AsBool(false))
            {
                return false;
            }
            //Defines variables. Variable itemstack is equal to the value of Player's itemstack. Lists Assetlocation as reference for conditional statement. 
            ItemStack itemstack = activeHotbarSlot.Itemstack;
            AssetLocation assetLocation;
            //Conditional statement. If Player's itemstack is null, then assetLocation is null. 
            if (itemstack == null)
            {
                assetLocation = null;
            }
            else // Else continues the conditional statement. Defines variable block to be equal to the value of Block in Player's itemstack. New conditional statement, same as above. 
            {
                Block block = itemstack.Block;
                if (block == null)
                {
                    assetLocation = null;
                }
                else // Else continues the above conditional statement. Defines variable sounds to be equal to the value of Sounds class. 
                {
                    BlockSounds sounds = block.Sounds;
                    assetLocation = ((sounds != null) ? sounds.Place : null);
                } // Defines variable assetLocation. The variable is equal to a conditional statement is sounds not null. If not null gets the sounds from class Place ansd assigns it to the variable. If null, it returns null. 
            }

            if (this.TryPut(activeHotbarSlot, blockSel))
            {
                this.Api.World.PlaySoundAt((assetLocation!= null) ? assetLocation : new AssetLocation("sounds/player/build"), byPlayer.Entity, byPlayer, true, 16f, 1f);
                this.updateMeshes();
                return true;
            }
            return false;
        }
        //Accesses method TryPut
        private bool TryPut(ItemSlot slot, BlockSelection blockSel)
        { // Conditional statement, establishing a loop that counts the variable, defines the condition for the loop based on inventory count, then increases the value until the statement is false. 
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (this.Inventory[i].Empty) //conditional statement within the for-loop. If the player's inventory (variable i) is empty then do this. 
                {   //defines variable num3 as equal to the value of the result of method TryPutInto. also updates meshes, triggers block change events, and returns the value of num3 if greater than 0.  Otherwise returns false.
                    int num3 = slot.TryPutInto(this.Api.World, this.Inventory[i], 1);
                    this.updateMeshes();
                    base.MarkDirty(true, null);
                    return num3 > 0;
                }
            }
            return false;
        }
        //Accesses method TryTake
        private bool TryTake(IPlayer byPlayer, BlockSelection blockSel)
        { // Conditional statement, establishing a loop that counts the variable, defines the condition for the loop based on inventory count, then increases the value until the statement is false. 
            for (int i = 0; i < Inventory.Count; i++)
            { //conditional statement within the for-loop. If the player's inventory (variable i) is empty, then do this. 
                if (!this.Inventory[i].Empty)
                {
                    ItemStack itemStack = this.Inventory[i].TakeOut(1);
                    if (byPlayer.InventoryManager.TryGiveItemstack(itemStack, false))
                    { //Defines variables for both block and assetLocation, then conditional statement. If Player's itemstack is null, then assetLocation is null.
                        Block block = itemStack.Block;
                        AssetLocation assetLocation;
                        if (block == null)
                        {
                            assetLocation = null;
                        }
                        else //Else continues the above conditional statement. Defines variable sounds to be equal to the value of Sounds class.
                        {
                            BlockSounds sounds = block.Sounds;
                            assetLocation = ((sounds != null) ? sounds.Place : null); // The variable assetLocation is equal to a conditional statement is sounds not null. If not null gets the sounds from class Place ansd assigns it to the variable. If null, it returns null. 
                        }
                        // if none of the conditions are met, then returns method PlaySoundsAt. Also includes conditional statement where if assetlocation is not null, then retrieve new asset sound "build"
                        this.Api.World.PlaySoundAt((assetLocation != null) ? assetLocation : new AssetLocation("sounds/player/build"), byPlayer.Entity, byPlayer, true, 16f, 1f);
                    }
                    if (itemStack.StackSize > 0) //Conditional statement stating if the itemstack's stack size is greater than 0, then return method SpawnItemEntity. Otherwise, update visual meshes, trigger block change events, and return true. 
                    {
                        this.Api.World.SpawnItemEntity(itemStack, this.Pos.ToVec3d().Add(0.5, 0.5, 0.5), null);
                    }
                    base.MarkDirty(true, null);
                    this.updateMeshes();
                    return true;
                }
            }
            return false; // if the for-loop statement's are false, then return false. 
        }

       private Matrixf mat = new Matrixf(); // access variable Matrixf, define variable mat to create a new instance of Matrixf. Matrixf handles values related to the EntityBlock. 

    } 
};
