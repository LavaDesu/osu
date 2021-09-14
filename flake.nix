{
  description = "nix dev environment";

  inputs.flake-utils.url = "github:numtide/flake-utils";

  outputs = { self, nixpkgs, flake-utils }:
    flake-utils.lib.eachDefaultSystem(system:
    let
      version = "2021.907.0-lava4";

      baseBuild = "-f net5.0 -v minimal /property:Version=${version} osu.Desktop -- $@";
        
      pkgs = nixpkgs.legacyPackages.${system};
      deps = with pkgs; [
        openssl icu ffmpeg_4 alsa-lib SDL2 lttng-ust numactl
      ];

      fixSdl = pkgs.writeScript "osu-fixsdl" ''
        ln -sft osu.Desktop/bin/Debug/net5.0/linux-x64/ ${pkgs.SDL2}/lib/libSDL2${pkgs.stdenv.hostPlatform.extensions.sharedLibrary}
        ln -sft osu.Desktop/bin/Release/net5.0/linux-x64/ ${pkgs.SDL2}/lib/libSDL2${pkgs.stdenv.hostPlatform.extensions.sharedLibrary}
      '';
      buildScript = pkgs.writeScript "osu-build" ''
        rm -f osu.Desktop/bin/Debug/net5.0/linux-x64/libSDL2.so
        dotnet build -c Debug -r linux-x64 ${baseBuild} && ${fixSdl}
      '';
      buildReleaseScript = pkgs.writeScript "osu-build-rel" ''
        rm -f osu.Desktop/bin/Release/net5.0/linux-x64/libSDL2.so
        dotnet build -c Release -r linux-x64 ${baseBuild} && ${fixSdl}
      '';
      publishScript = pkgs.writeScript "osu-publish" ''
        rm -f osu.Desktop/bin/Release/net5.0/linux-x64/libSDL2.so
        dotnet publish -c Release -r linux-x64 --self-contained false ${baseBuild} && ${fixSdl}
      '';
      publishWinScript = pkgs.writeScript "osu-publish-win" ''
        dotnet publish -c Release -r win-x64 --self-contained false -o build-win ${baseBuild}
      '';
      runScript = pkgs.writeScript "osu-run" ''
        ${buildScript} && dotnet run --no-build -c Debug -f net5.0 -r linux-x64 -p osu.Desktop -v minimal -- $@
      '';
      runReleaseScript = pkgs.writeScript "osu-run-rel" ''
        ${buildReleaseScript} && dotnet run --no-build -c Release -f net5.0 -r linux-x64 -p osu.Desktop -v minimal -- $@
      '';

      scripts = pkgs.stdenvNoCC.mkDerivation {
        pname = "osu-scripts";
        version = "1.0.0";
        dontUnpack = true;
        installPhase = ''
          mkdir $out
          cp ${fixSdl} $out/osu-fixsdl
          cp ${buildScript} $out/osu-build
          cp ${buildReleaseScript} $out/osu-build-rel
          cp ${publishScript} $out/osu-publish
          cp ${publishWinScript} $out/osu-publish-win
          cp ${runScript} $out/osu-run
          cp ${runReleaseScript} $out/osu-run-rel
        '';
      };
    in {
      devShell = pkgs.mkShell {
        nativeBuildInputs = with pkgs; [
          icu
          dotnetCorePackages.sdk_5_0
          dotnetCorePackages.net_5_0
        ];

        DOTNET_CLI_TELEMETRY_OPTOUT = 1;
        DRI_PRIME = 1;
        LD_LIBRARY_PATH = pkgs.lib.makeLibraryPath deps;
        shellHook = ''
          export PATH="${scripts}:$PATH"
        '';
      };
      defaultApp = { type = "app"; program = "${runScript}"; };

      apps.build = { type = "app"; program = "${buildScript}"; };
      apps.build-release = { type = "app"; program = "${buildReleaseScript}"; };
      apps.fix = { type = "app"; program = "${fixSdl}"; };
      apps.start = { type = "app"; program = "${runScript}"; };
      apps.start-release = { type = "app"; program = "${runReleaseScript}"; };
    });
}
