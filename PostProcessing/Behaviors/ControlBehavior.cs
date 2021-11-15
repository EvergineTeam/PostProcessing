using System;
using System.Linq;
using Evergine.Common.Input;
using Evergine.Common.Input.Keyboard;
using Evergine.Components.Graphics3D;
using Evergine.Framework;
using Evergine.Framework.Graphics;

namespace PostProcessing.Behaviors
{
    public class ControlBehavior : Behavior
    {
        [BindComponent]
        private PostProcessingGraphRenderer volume;

        private bool EnabledSSAO;
        private bool EnabledSSR;
        private bool EnabledDoF;
        private bool EnabledBloom;
        private bool EnabledTonemapping;
        private PostProcessingNodePortDirectiveType ssaoProperty;
        private PostProcessingNodePortDirectiveType ssrProperty;
        private PostProcessingNodePortDirectiveType dofProperty;
        private PostProcessingNodePortDirectiveType bloomProperty;
        private PostProcessingNodePortDirectiveType toneProperty;

        protected override bool OnAttached()
        {
            var result = base.OnAttached();

            this.EnabledSSAO = true;
            this.EnabledSSR = true;
            this.EnabledDoF = true;
            this.EnabledBloom = true;
            this.EnabledTonemapping = true;

            var enabledNodes = volume.ppGraph.Description.Nodes.FindAll(n => n.Name == "Enabled");

            // SSAO
            var ssaoEnabledNode = enabledNodes.First();
            this.ssaoProperty = ssaoEnabledNode.Inputs[0].Type as PostProcessingNodePortDirectiveType;

            // SSR
            var ssrEnabledNode = enabledNodes.Skip(1).Take(1).Single();
            this.ssrProperty = ssrEnabledNode.Inputs[0].Type as PostProcessingNodePortDirectiveType;

            // Dof
            var dofEnabledNode = enabledNodes.Skip(5).Take(1).Single();
            this.dofProperty = dofEnabledNode.Inputs[0].Type as PostProcessingNodePortDirectiveType;

            // Bloom
            var bloomEnabledNode = enabledNodes.Skip(6).Take(1).Single();
            this.bloomProperty = bloomEnabledNode.Inputs[0].Type as PostProcessingNodePortDirectiveType;

            // Tonemapping
            var toneEnabledNode = enabledNodes.Skip(7).Take(1).Single();
            this.toneProperty = toneEnabledNode.Inputs[0].Type as PostProcessingNodePortDirectiveType;

            return result;
        }

        private void RefreshProperty(PostProcessingNodePortDirectiveType property, bool value)
        {
            property.Value = value ? property.Directives[1] : property.Directives[0];
        }

        protected override void Update(TimeSpan gameTime)
        {
            var display = this.Managers.RenderManager.ActiveCamera3D.Display;
            if (display == null)
            {
                return;
            }

            var keyboardDispacher = display.KeyboardDispatcher;

            if (keyboardDispacher == null)
            {
                return;
            }

            if (keyboardDispacher.ReadKeyState(Keys.D1) == ButtonState.Pressing)
            {
                this.EnabledSSAO = !this.EnabledSSAO;
                this.RefreshProperty(this.ssaoProperty, this.EnabledSSAO);
            }

            if (keyboardDispacher.ReadKeyState(Keys.D2) == ButtonState.Pressing)
            {
                this.EnabledSSR = !this.EnabledSSR;
                this.RefreshProperty(this.ssrProperty, this.EnabledSSR);
            }

            if (keyboardDispacher.ReadKeyState(Keys.D3) == ButtonState.Pressing)
            {
                this.EnabledDoF = !this.EnabledDoF;
                this.RefreshProperty(this.dofProperty, this.EnabledDoF);
            }

            if (keyboardDispacher.ReadKeyState(Keys.D4) == ButtonState.Pressing)
            {
                this.EnabledBloom = !this.EnabledBloom;
                this.RefreshProperty(this.bloomProperty, this.EnabledBloom);
            }

            if (keyboardDispacher.ReadKeyState(Keys.D5) == ButtonState.Pressing)
            {
                this.EnabledTonemapping = !this.EnabledTonemapping;
                this.RefreshProperty(this.toneProperty, this.EnabledTonemapping);
            }
        }
    }
}
