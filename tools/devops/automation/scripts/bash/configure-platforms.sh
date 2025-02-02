#!/bin/bash -eux

env | sort

set -o pipefail
IFS=$'\n\t '

FILE=$(pwd)/tmp.txt

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=DOTNET_PLATFORMS
DOTNET_PLATFORMS=$(cat "$FILE")

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=ALL_DOTNET_PLATFORMS
ALL_DOTNET_PLATFORMS=$(cat "$FILE")

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=INCLUDE_IOS
INCLUDE_IOS=$(cat "$FILE")

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=INCLUDE_TVOS
INCLUDE_TVOS=$(cat "$FILE")

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=INCLUDE_MAC
INCLUDE_MAC=$(cat "$FILE")

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=INCLUDE_MACCATALYST
INCLUDE_MACCATALYST=$(cat "$FILE")

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=IOS_NUGET_OS_VERSION
IOS_NUGET_OS_VERSION=$(cat "$FILE")

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=TVOS_NUGET_OS_VERSION
TVOS_NUGET_OS_VERSION=$(cat "$FILE")

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=MACOS_NUGET_OS_VERSION
MACOS_NUGET_OS_VERSION=$(cat "$FILE")

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=MACCATALYST_NUGET_OS_VERSION
MACCATALYST_NUGET_OS_VERSION=$(cat "$FILE")

# print it out, so turn off echoing since that confuses Azure DevOps
set +x

echo "##vso[task.setvariable variable=DOTNET_PLATFORMS;isOutput=true]$DOTNET_PLATFORMS"
DISABLED_DOTNET_PLATFORMS=" $ALL_DOTNET_PLATFORMS "
for platform in $DOTNET_PLATFORMS; do
	PLATFORM_UPPER=$(echo "$platform" | tr '[:lower:]' '[:upper:]')
	echo "##vso[task.setvariable variable=INCLUDE_DOTNET_$PLATFORM_UPPER;isOutput=true]1"
	DISABLED_DOTNET_PLATFORMS=${DISABLED_DOTNET_PLATFORMS/ $platform / }

	VARIABLE="${PLATFORM_UPPER}_NUGET_VERSION_NO_METADATA"
	make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE="$VARIABLE"
	VALUE=$(cat "$FILE")
	echo "##vso[task.setvariable variable=$VARIABLE;isOutput=true]$VALUE"

	VARIABLE="${PLATFORM_UPPER}_NUGET_SDK_NAME"
	make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE="$VARIABLE"
	VALUE=$(cat "$FILE")
	echo "##vso[task.setvariable variable=$VARIABLE;isOutput=true]$VALUE"

	VARIABLE="${PLATFORM_UPPER}_NUGET_REF_NAME"
	make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE="$VARIABLE"
	VALUE=$(cat "$FILE")
	echo "##vso[task.setvariable variable=$VARIABLE;isOutput=true]$VALUE"

	VARIABLE="DOTNET_${PLATFORM_UPPER}_RUNTIME_IDENTIFIERS"
	make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE="$VARIABLE"
	VALUE=$(cat "$FILE")
	echo "##vso[task.setvariable variable=$VARIABLE;isOutput=true]$VALUE"

	RIDS=$VALUE
	for rid in $RIDS; do
		VARIABLE="${rid}_NUGET_RUNTIME_NAME"
		make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE="$VARIABLE"
		VALUE=$(cat "$FILE")
		echo "##vso[task.setvariable variable=$VARIABLE;isOutput=true]$VALUE"
	done
done
for platform in $DISABLED_DOTNET_PLATFORMS; do
	PLATFORM_UPPER=$(echo "$platform" | tr '[:lower:]' '[:upper:]')
	echo "##vso[task.setvariable variable=INCLUDE_DOTNET_$PLATFORM_UPPER;isOutput=true]"
done

echo "##vso[task.setvariable variable=INCLUDE_IOS;isOutput=true]$INCLUDE_IOS"
echo "##vso[task.setvariable variable=INCLUDE_TVOS;isOutput=true]$INCLUDE_TVOS"
echo "##vso[task.setvariable variable=INCLUDE_MAC;isOutput=true]$INCLUDE_MAC"
echo "##vso[task.setvariable variable=INCLUDE_MACCATALYST;isOutput=true]$INCLUDE_MACCATALYST"

echo "##vso[task.setvariable variable=IOS_NUGET_OS_VERSION;isOutput=true]$IOS_NUGET_OS_VERSION"
echo "##vso[task.setvariable variable=TVOS_NUGET_OS_VERSION;isOutput=true]$TVOS_NUGET_OS_VERSION"
echo "##vso[task.setvariable variable=MACOS_NUGET_OS_VERSION;isOutput=true]$MACOS_NUGET_OS_VERSION"
echo "##vso[task.setvariable variable=MACCATALYST_NUGET_OS_VERSION;isOutput=true]$MACCATALYST_NUGET_OS_VERSION"


set -x

rm -f "$FILE"
