// Copyright 2019 Daniel Butum

#pragma once

#include "CoreMinimal.h"
#include "types.h"
#include "Tickable.h"
#include "Engine/DataTable.h"
#include "DiscordHelper.generated.h"

namespace discord {
class Core;
}

USTRUCT(BlueprintType)
struct FDiscordRichPrecisionData : public FTableRowBase
{
	GENERATED_BODY()

	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category = "Discord")
	FText Details;

	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category = "Discord")
	FString LargeImageTag;

	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category = "Discord")
	FString SmallImageTag;
};

/**
 * SDK Documentation: https://discordapp.com/developers/docs/game-sdk/sdk-starter-guide#code-primer-unreal-engine-4-cpp
 * Activity Documentation: https://discordapp.com/developers/docs/game-sdk/activities
 */
UCLASS()
class DISCORDGAMESDK_API UDiscordHelper : public UObject, public FTickableGameObject
{
	GENERATED_BODY()

public:
	bool Initialize(int64 ClientID, bool bDiscordRequired = false);

	virtual void BeginDestroy() override;

	// Tickable object interface
	virtual void Tick(float DeltaTime) override;
	virtual bool IsTickable() const override;
	virtual TStatId GetStatId() const override;

	int64 GetConnectedUserID();
	FString GetConnectedUserName();

	bool UpdatePlayActivity(const FString& State, const FDiscordRichPrecisionData& Data, int64 Timestamp);
	void ClearPlayActivity();

	void ValidateOrExit(std::function<void(discord::Result)> Callback);

	bool RegisterSteam(uint32 SteamAppID);

	DECLARE_DYNAMIC_MULTICAST_DELEGATE_ThreeParams(FOnUserConnectedSignature, const FString&, UserName, const int64, UserId, const FString&, UserDiscriminator);
	UPROPERTY(BlueprintAssignable)
	FOnUserConnectedSignature OnUserConnected;

private:
	discord::Core* Core = nullptr;
	bool bGotUserConnectedReply = false;
};
