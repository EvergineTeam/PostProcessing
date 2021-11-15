using System;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Mathematics;

namespace NET5Demo.Behaviors
{
    public class AutoFocusBehavior : Behavior
    {
        [BindComponent]
        protected Transform3D Transform = null;

        [BindComponent]
        protected Camera3D Camera = null;

        private Ray ray;

        public AutoFocusBehavior()
            : base(FamilyType.PriorityBehavior)
        { }

        protected override void Update(TimeSpan gameTime)
        {
            var from = this.Transform.Position;
            var dir = this.Transform.WorldTransform.Forward;
            dir.Normalize();

            this.ray.Position = from;
            this.ray.Direction = dir;

            var hitResult = this.Managers.PhysicManager3D.RayCast(ref ray, this.Camera.FarPlane);
            if (hitResult.Succeeded)
            {
                this.Camera.FocalDistance = Vector3.Distance(this.ray.Position, hitResult.Point);
            }
        }
    }
}
