// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;

namespace osu.Game.Users
{
    public class DiscordUser: IEquatable<DiscordUser>
    {
        [JsonProperty(@"id")]
        public string Id;

        [JsonProperty(@"username")]
        public string Username;

        [JsonProperty(@"discriminator")]
        public string Discriminator;

        [JsonProperty(@"avatar")]
        public string Avatar;

        [JsonProperty(@"banner")]
        public string Banner;

        public string FormattedName => $"{Username}#{Discriminator}";

        public bool Equals(DiscordUser other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Id == other.Id;
        }

        public static DiscordUser empty => new DiscordUser {
            Id = "0",
            Username = "empty",
            Discriminator = "0000",
            Banner = null,
        };
    }
}
