using Evergine.Common.Graphics;
using Evergine.Components.Graphics3D;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Framework.Services;
using Evergine.Mathematics;

namespace PostProcessing
{
    public class DemoScene : Scene
    {
		public override void RegisterManagers()
        {
        	base.RegisterManagers();
        	this.Managers.AddManager(new Evergine.Bullet.BulletPhysicManager3D());        	
        }

        protected override void CreateScene()
        {
        }
    }
}
