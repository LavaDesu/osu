// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Tournament.Models;
using osu.Game.Users;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Tournament.Components
{
    public class DrawableTeamWithPlayers : CompositeDrawable
    {
        public DrawableTeamWithPlayers(TournamentTeam team, TeamColour colour)
        {
            AutoSizeAxes = Axes.Both;
            Anchor = colour == TeamColour.Blue ? Anchor.TopRight : Anchor.TopLeft;
            Origin = colour == TeamColour.Blue ? Anchor.TopRight : Anchor.TopLeft;

            InternalChildren = new Drawable[]
            {
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(30),
                    Children = new Drawable[]
                    {
                        new DrawableTeamTitleWithHeader(team, colour)
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                        },
                        new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            RelativeSizeAxes = Axes.X,
                            ChildrenEnumerable = team?.Players
                                .OrderBy(p => p.Statistics?.GlobalRank ?? 10000000000)
                                .Select(p => createPlayerText(p, colour == TeamColour.Blue))
                                .Take(6) ?? Enumerable.Empty<Drawable>()
                        },
                    }
                },
            };

            TournamentSpriteText createPlayerText(User p, bool rightAligned = false) =>
                new TournamentSpriteText
                {
                    Anchor = rightAligned ? Anchor.TopRight : Anchor.TopLeft,
                    Origin = rightAligned ? Anchor.TopRight : Anchor.TopLeft,
                    Text = p.Username,
                    Font = OsuFont.Torus.With(size: 24, weight: FontWeight.SemiBold),
                    Colour = Color4.White,
                };
        }
    }
}
