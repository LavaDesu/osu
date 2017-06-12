﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace osu.Game.Graphics.UserInterface
{
    public class OsuContextMenu<TItem> : ContextMenu<TItem>
        where TItem : ContextMenuItem
    {
        protected override Menu<TItem> CreateMenu() => new CustomMenu();

        public class CustomMenu : Menu<TItem>
        {
            private const int fade_duration = 250;

            public CustomMenu()
            {
                CornerRadius = 5;
                ItemsContainer.Padding = new MarginPadding { Vertical = OsuContextMenuItem.MARGIN_VERTICAL };
                Masking = true;
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Shadow,
                    Colour = Color4.Black.Opacity(0.25f),
                    Radius = 4,
                };
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                Background.Colour = colours.ContextMenuGray;
            }

            protected override void AnimateOpen() => FadeIn(fade_duration, EasingTypes.OutQuint);
            protected override void AnimateClose() => FadeOut(fade_duration, EasingTypes.OutQuint);
        }
    }
}