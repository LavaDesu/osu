// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Net.Http;
using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.IO.Network;
using osu.Game.Configuration;
using osu.Game.Users;

namespace osu.Game.Online.API {
    public class DiscordAPI : Component
    {
        public Bindable<string> Token { get; private set; }

        public Bindable<DiscordUser> User = new Bindable<DiscordUser>(DiscordUser.empty);

        public BindableBool Ready { get; private set; }

        public DiscordAPI(OsuConfigManager osuConfig)
        {
            Logger.Log("load");
            Token = osuConfig.GetBindable<string>(OsuSetting.DiscordToken);
            Ready = new BindableBool(false);
            Token.BindValueChanged(_ => UpdateUser(), true);
        }

        public bool UpdateUser()
        {
            Ready.Value = false;
            using (var req = new GetDiscordMeRequest() { Token = "Bot " + Token })
            {
                try
                {
                    Logger.Log("req");
                    req.Perform();
                }
                catch
                {
                    Logger.Log("owie");
                    User.Value = null;
                    return false;
                }

                Ready.Value = true;
                User.Value = req.ResponseObject;
                return true;
            }
        }
    }

    public class GetDiscordMeRequest : JsonWebRequest<DiscordUser>
    {
        public string Token;

        public GetDiscordMeRequest()
        {
            Url = $@"https://discord.com/api/v9/users/@me";
        }

        protected override void PrePerform()
        {
            AddHeader("Authorization", Token);
        }

        protected override string UserAgent => "DiscordBot (testing!, 0.0.0)";
    }

    public class GetDiscordUserRequest : GetDiscordMeRequest
    {
        public readonly string TargetID;

        public GetDiscordUserRequest(string ID) {
            TargetID = ID;
            Url = $@"https://discord.com/api/v9/users/{TargetID}";
        }

        protected override string UserAgent => "DiscordBot (testing!, 0.0.0)";
    }
}
