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
        const float x_offset = 280;
        const float flag_x_offset = 30;
        const float flag_y_offset = 70;
        const float flag_scale = 2.5f;

        protected FillFlowContainer CreateTeam(TournamentTeam team, TeamColour colour) => new FillFlowContainer
        {
            Margin = colour == TeamColour.Red
                ? new MarginPadding { Top = y_offset, Left = x_offset }
                : new MarginPadding { Top = y_offset, Right = x_offset },
            Anchor = colour == TeamColour.Red ? Anchor.TopLeft : Anchor.TopRight,
            Origin = colour == TeamColour.Red ? Anchor.TopLeft : Anchor.TopRight,
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(flag_y_offset),

            Children = new Drawable[]
            {
                new DrawableTeamFlag(team)
                {
                    Anchor = colour == TeamColour.Red ? Anchor.TopLeft : Anchor.TopRight,
                    Origin = colour == TeamColour.Red ? Anchor.TopLeft : Anchor.TopRight,
                    Margin = colour == TeamColour.Red
                        ? new MarginPadding { Left = flag_x_offset }
                        : new MarginPadding { Right = flag_x_offset },
                    Scale = new Vector2(flag_scale),
                },
                new DrawableTeamWithPlayers(team, colour)
            }
        };
    }
}
