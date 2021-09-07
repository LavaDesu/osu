// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Tournament.Screens.Setup
{
    internal class DiscordTokenBox : ActionableInfo
    {
        private const int minimum_window_height = 480;
        private const int maximum_window_height = 2160;

        // HACK: kinda a hack but hey it works
        // reasoning: reload in SetupScreen would reload the box again, so the box would
        // hide when you type something in it :/
        private static bool hidden = true;

        public new Action<string> Action;
        public Action<string> OnCommit;
        public Bindable<string> Current { get => textBox.Current; set => textBox.Current = value; }
        public String DefaultText;
        private OsuTextBox textBox;

        public DiscordTokenBox()
            : base()
        {
        }

        protected override Drawable CreateComponent()
        {
            var drawable = base.CreateComponent();
            FlowContainer.Insert(-1, textBox = new OsuTextBox {
                PlaceholderText = "Put token here!",
                Width = 600,
                CommitOnFocusLost = true,
            });

            textBox.OnCommit += (_, __) => {
                OnCommit?.Invoke(textBox.Text);
            };

            if (hidden)
                textBox.Hide();

            base.Action = () => {
                if (hidden)
                    textBox.FadeIn(200);
                else
                    textBox.FadeOut(200);

                hidden = !hidden;

                Action?.Invoke(textBox.Text);
            };

            return drawable;
        }
    }
}
