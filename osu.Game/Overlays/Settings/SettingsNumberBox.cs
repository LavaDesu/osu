// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;

namespace osu.Game.Overlays.Settings
{
    public class SettingsNumberBox : SettingsItem<int?>
    {
        private bool disableAnimations;

        public SettingsNumberBox(bool disableAnimations = false)
        {
            this.disableAnimations = disableAnimations;
        }

        protected override Drawable CreateControl() => new NumberControl(disableAnimations)
        {
            RelativeSizeAxes = Axes.X,
            Margin = new MarginPadding { Top = 5 }
        };

        private sealed class NumberControl : CompositeDrawable, IHasCurrentValue<int?>
        {
            private readonly BindableWithCurrent<int?> current = new BindableWithCurrent<int?>();

            public Bindable<int?> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            public NumberControl(bool disableAnimations)
            {
                AutoSizeAxes = Axes.Y;

                OutlinedNumberBox numberBox;

                InternalChildren = new[]
                {
                    numberBox = new OutlinedNumberBox(disableAnimations)
                    {
                        RelativeSizeAxes = Axes.X,
                        CommitOnFocusLost = true
                    }
                };

                numberBox.Current.BindValueChanged(e =>
                {
                    int? value = null;

                    if (int.TryParse(e.NewValue, out var intVal))
                        value = intVal;

                    current.Value = value;
                });

                Current.BindValueChanged(e =>
                {
                    numberBox.Current.Value = e.NewValue?.ToString();
                });
            }
        }

        private class OutlinedNumberBox : OutlinedTextBox
        {
            private bool disableAnimations;
            public OutlinedNumberBox(bool disableAnimations)
            {
                this.disableAnimations = disableAnimations;
            }

            protected override bool CanAddCharacter(char character) => char.IsNumber(character) || character == '-';
            protected override Drawable GetDrawableCharacter(char c) {
                if (!disableAnimations)
                    return new FallingDownContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Child = new OsuSpriteText { Text = c.ToString(), Font = OsuFont.GetFont(size: CalculatedTextSize) },
                    };
                else
                    return new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Child = new OsuSpriteText { Text = c.ToString(), Font = OsuFont.GetFont(size: CalculatedTextSize) },
                    };
            }

        }
    }
}
