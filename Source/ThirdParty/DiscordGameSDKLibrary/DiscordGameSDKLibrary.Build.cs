// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

using System;
using System.IO;
using UnrealBuildTool;
using EpicGames.Core;

public class DiscordGameSDKLibrary : ModuleRules
{
	public DiscordGameSDKLibrary(ReadOnlyTargetRules Target) : base(Target)
	{
		Type = ModuleType.External;
		PublicDefinitions.Add("DISCORD_DYNAMIC_LIB=1");

		string BaseDirectory = Path.GetFullPath(Path.Combine(ModuleDirectory, "..", "..", "ThirdParty", "DiscordGameSDKLibrary"));
		if (Target.Platform == UnrealTargetPlatform.Win64)
		{
			string lib = Path.Combine(BaseDirectory, "Win64");

			// Include headers
			PublicIncludePaths.Add(Path.Combine(BaseDirectory, "Include"));

			// Add the import library
			LinkStaticLibrary(Path.Combine(lib, "discord_game_sdk.dll.lib"));

			// Dynamic
			AddDllDependency(Path.Combine(lib, "discord_game_sdk.dll"));
			PublicDelayLoadDLLs.Add("discord_game_sdk.dll");
		}
		else if (Target.Platform == UnrealTargetPlatform.Linux)
		{
			string lib = Path.Combine(BaseDirectory, "Linux", "x86_64-unknown-linux-gnu");

			// Include headers
			PublicIncludePaths.Add(Path.Combine(BaseDirectory, "Include"));

			// Add the import library
			LinkStaticLibrary(Path.Combine(lib, "libdiscord_game_sdk.so") );
			AddDllDependency(Path.Combine(lib, "libdiscord_game_sdk.so"));
		}
		else if (Target.Platform == UnrealTargetPlatform.Mac)
		{
			string lib = Path.Combine(BaseDirectory, "Mac");

			// Include headers
			PublicIncludePaths.Add(Path.Combine(BaseDirectory, "Include"));

			// Add the import library
			LinkStaticLibrary(Path.Combine(lib, "discord_game_sdk.dylib") );
			AddDllDependency(Path.Combine(lib, "discord_game_sdk.dylib"));
		}
	}

	private void LinkStaticLibrary(string full_library_path)
	{
		if (!File.Exists(full_library_path))
		{
			Fail("Unable to locate any build library: " + full_library_path);
		}

		// Found a library; add it to the dependencies list
		PublicAdditionalLibraries.Add(full_library_path);
	}


	private void AddDllDependency(string full_dll_path)
	{
		if (!File.Exists(full_dll_path))
		{
			Fail("Unable to locate any build library: " + full_dll_path);
		}

		// Found a library; add it to the dependencies list
		RuntimeDependencies.Add(full_dll_path);
	}

	/**
	 * Print out a build message
	 */
	private void Trace(string msg)
	{
		Log.TraceInformationOnce("DiscordGameSDKLibrary: {msg}", msg);
	}

	private void Fail(string message)
	{
		Trace(message);
		throw new Exception(message);
	}
}
