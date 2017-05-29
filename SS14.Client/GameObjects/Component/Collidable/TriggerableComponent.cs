﻿using SS14.Shared.IoC;
using SS14.Shared.GameObjects;

namespace SS14.Client.GameObjects
{
    [IoCTarget]
    public class TriggerableComponent : CollidableComponent
    {
        public override string Name => "Triggerable";
        public TriggerableComponent()
        {
            isHardCollidable = false;
        }
    }
}