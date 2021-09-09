// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using osu.Game.Tournament.Components;
using osu.Game.Tournament.Models;
using osuTK;

namespace osu.Game.Tournament.Screens.TeamIntro
{
    public class TeamIntroScreen : TournamentMatchScreen, IProvideVideo
    {
        private Container mainContainer;

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            RelativeSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                new TourneyVideo("teamintro")
                {
                    RelativeSizeAxes = Axes.Both,
                    Loop = true,
                },
                mainContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                }
            };
        }

        protected override void CurrentMatchChanged(ValueChangedEvent<TournamentMatch> match)
        {
            base.CurrentMatchChanged(match);

            mainContainer.Clear();

            if (match.NewValue == null)
                return;

            mainContainer.Children = new Drawable[]
            {
                new RoundDisplay(match.NewValue)
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                },
                CreateTeam(match.NewValue.Team1.Value, TeamColour.Red),
                CreateTeam(match.NewValue.Team2.Value, TeamColour.Blue),
            };
        }

        const float y_offset = 170;
        const float x_offset = 230;
        const float flag_offset = 70;
        const float flag_scale = 2.5f;

        protected FillFlowContainer CreateTeam(TournamentTeam team, TeamColour colour) => new FillFlowContainer
        {
            Margin = new MarginPadding { Top = y_offset, Left = x_offset, Right = x_offset },
            Anchor = colour == TeamColour.Red ? Anchor.TopLeft : Anchor.TopRight,
            Origin = colour == TeamColour.Red ? Anchor.TopLeft : Anchor.TopRight,
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(flag_offset),

            Children = new Drawable[]
            {
                new DrawableTeamFlag(team)
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Scale = new Vector2(flag_scale),
                },
                new DrawableTeamWithPlayers(team, colour)
                {
                },
            }
        };
    }
}
