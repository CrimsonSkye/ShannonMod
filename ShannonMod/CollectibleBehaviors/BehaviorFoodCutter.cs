using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace ShannonMod.CollectibleBehaviors
{
    class BehaviorFoodCutter : CollectibleBehavior {
        ICoreAPI api;
        ICoreClientAPI capi;

        public override void Initialize(JsonObject properties)
        {
            base.Initialize(properties);
        }

        public BehaviorFoodCutter(CollectibleObject collObj) : base(collObj)
        {
            this.collObj = collObj;
        }

        public override void OnLoaded(ICoreAPI api)
        {
            this.api = api;
            this.capi = (api as ICoreClientAPI);
            interactions = ObjectCacheUtil.GetOrCreate(api, "vtaxeInteractions", () =>
            {
                return new WorldInteraction[] {
                    new WorldInteraction()
                    {
                        ActionLangCode = "shannonmod:itemhelp-knife-cut",
                        HotKeyCode = "sprint",
                        MouseButton = EnumMouseButton.Right
                    }
                };
            });
        }

        public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
        {
            Block interactedBlock = api.World.BlockAccessor.GetBlock(blockSel.Position);
            if ((interactedBlock.FirstCodePart() == "food" && interactedBlock.Variant["type"]=="placed"))
            handHandling = EnumHandHandling.PreventDefault;
            {
                byEntity.StartAnimation("knifecut");
            }
        }

        //public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandling handling)
        //{
        //    return base.OnHeldInteractStep(secondsUsed, slot, byEntity, blockSel, entitySel, ref handling);
        //}
        
        WorldInteraction[] interactions = null;
    }

    }
