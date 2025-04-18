#!/bin/bash -eu
# shellcheck disable=SC2129

# This script will detect changes between the current hash and another hash.
# The other hash is by default the previous commit, but can also be any other commit (for a PR it could be the commit before the PR).

set -o pipefail

WHITE=$(tput setaf 7 2>/dev/null || true)
BLUE=$(tput setaf 6 2>/dev/null || true)
RED=$(tput setaf 9 2>/dev/null || true)
CLEAR=$(tput sgr0 2>/dev/null || true)

# Clone files on AFPS, instead of copying them. Much faster.
CP="cp"
if df -t apfs / >/dev/null 2>&1; then
	CP="cp -c"
fi

# Change detection:
#
# * Changes in current PR/commit (if a PR/CI) - api + generator
# * Changes vs stable - api
# * Changes between different assemblies (Xamarin vs .NET, Microsoft.iOS vs Microsoft.MacCatalyst)
#

function show_help ()
{
	echo "$(basename "$0"): Detect changes between the current hash (which must already be built) and another hash."
	echo "Usage is: $(basename "$0") --base=[TREEISH] [options]"
	echo "   -h, -?, --help               Displays the help."
	echo "   -b, --base=[HASH]            The hash to compare the currently built assemblies against. The default is the previous commit (HEAD^)."
	echo "   --pull-request=[NUMBER]      The pull request to compare against. This assumes that: a) the current hash is the tip of the pull request b) --base has not been specified."  I
	echo "   --keep-build                 Don't remove the cloned & built working copy of the other hash once done."
	echo "   --use-existing-build         Use an existing build (kept with --keep-build) instead of building it again."
	echo "   --skip-generator-diff        Don't do the generator diff vs another commit."
	echo "   --skip-api-diff              Don't do the api diff vs another commit."
	echo "   --skip-stable-api-comparison Don't do the api diff vs stable version."
	echo "   --output-dir=[DIR]           The output directory."
	echo "   --skip-dirty-check           Skip check that the working copy does not have dirty files."
	echo ""
}

CURRENT_HASH=HEAD
BASE_HASH=
OUTPUT_DIR=
PULL_REQUEST_ID=
PULL_REQUEST_TARGET_BRANCH=
USE_EXISTING_BUILD=
KEEP_BUILD=
ENABLE_GENERATOR_DIFF=1
ENABLE_API_DIFF=1
ENABLE_STABLE_API_COMPARISON=1
SKIP_DIRTY_CHECK=
GH_COMMENTS_FILE=
while ! test -z "${1:-}"; do
	case "$1" in
		--help|-\?|-h)
			show_help
			exit 0
			;;
		-v | --verbose)
			set -x
			shift
			;;
		--base=*|-b=*)
			BASE_HASH="${1#*=}"
			shift
			;;
		--base|-b)
			BASE_HASH="$2"
			shift 2
			;;
		--pull-request=*)
			PULL_REQUEST_ID="${1#*=}"
			shift
			;;
		--pull-request)
			PULL_REQUEST_ID="$2"
			shift 2
			;;
		--pull-request-target-branch=*)
			PULL_REQUEST_TARGET_BRANCH="${1#*=}"
			shift
			;;
		--pull-request-target-branch)
			PULL_REQUEST_TARGET_BRANCH="$2"
			shift 2
			;;
		--output-dir=*)
			OUTPUT_DIR="${1#*=}"
			shift
			;;
		--output-dir)
			OUTPUT_DIR="$2"
			shift 2
			;;
		--use-existing-build)
			USE_EXISTING_BUILD=1
			shift
			;;
		--keep-build)
			KEEP_BUILD=1
			shift
			;;
		--skip-generator-diff)
			ENABLE_GENERATOR_DIFF=
			shift
			;;
		--skip-api-diff)
			ENABLE_API_DIFF=
			shift
			;;
		--skip-stable-api-comparison)
			ENABLE_STABLE_API_COMPARISON=
			shift
			;;
		--skip-dirty-check)
			SKIP_DIRTY_CHECK=1
			shift
			;;
		*)
			echo "${RED}Error: Unknown argument: $1${CLEAR}"
			exit 1
			;;
	esac
done

function report_error_line ()
{
	echo "$@"
	if test -n "$GH_COMMENTS_FILE"; then
		# remove color codes when writing to failure file
		printf ":fire: Failed to compare API and create generator diff :fire:\\n" >> "$GH_COMMENTS_FILE"
		printf "\\n"  >> "$GH_COMMENTS_FILE"
		# shellcheck disable=SC2001
		echo "$@" | sed $'s,\x1b\\[[0-9;]*[a-zA-Z],,g' >> "$GH_COMMENTS_FILE"
	fi
}

# Figure out the pull request
if test -z "$BASE_HASH"; then
	if test -n "$PULL_REQUEST_ID"; then
		echo "Computing API diff for pull request #$BLUE$PULL_REQUEST_ID$CLEAR"
		if test -z "$PULL_REQUEST_TARGET_BRANCH"; then
			echo "The target branch (using --pull-request-target-branch=) was not specified, trying to compute it."
			PR_FILENAME=pr-$PULL_REQUEST_ID.json
			curl --silent --fail --location --connect-timeout 15 --show-error https://api.github.com/repos/dotnet/macios/pulls/$PULL_REQUEST_ID -o "$PR_FILENAME"
			BASE_REF=$(python3 -c 'import json,sys;print(json.load(sys.stdin)["base"]["ref"]);' < "$PR_FILENAME")
			rm -f "$PR_FILENAME"
			PULL_REQUEST_TARGET_BRANCH=origin/"$BASE_REF"
			echo "Computed pull request target branch: $BLUE$PULL_REQUEST_TARGET_BRANCH$CLEAR"
		fi

		git fetch --no-tags --progress -- https://github.com/dotnet/macios +refs/pull/"$PULL_REQUEST_ID"/*:refs/remotes/origin/pr/"$PULL_REQUEST_ID"/*
		# The current hash is either a merge commit from GH, or the commit just before the merge commit.
		# However, we don't know if refs/pull/PULL_REQUEST_ID/merge or refs/pull/PULL_REQUEST_ID/head are pointing to the right place,
		# because someone might have pushed more commits to the pull request. So we need some computations here...
		if ! GH_MERGE=$(git log -1 --pretty=%H refs/remotes/origin/pr/"$PULL_REQUEST_ID"/merge); then
			# this may happen if the PR has a conflict with the target branch
			GH_MERGE=conflict
		fi
		GH_HEAD=$(git log -1 --pretty=%H refs/remotes/origin/pr/"$PULL_REQUEST_ID"/head)
		CURRENT_HEAD=$(git log -1 --pretty=%H HEAD)

		MERGE_COMMIT_SUBJECT=$(git show refs/remotes/origin/pr/"$PULL_REQUEST_ID"/merge --pretty=%s | head -1)
		echo "Computing the merge base:"
		echo "    GH_MERGE=$BLUE$GH_MERGE$CLEAR"
		echo "    CURRENT_HEAD=$BLUE$CURRENT_HEAD$CLEAR"
		echo "    GH_HEAD=$BLUE$GH_HEAD$CLEAR"
		echo "    Tip of GitHub's ${BLUE}pull/$PULL_REQUEST_ID/merge$CLEAR branch: $BLUE$MERGE_COMMIT_SUBJECT$CLEAR"

		if [[ "$GH_MERGE" == "$CURRENT_HEAD" ]]; then
			# we're at the merge commit.
			MERGE_BASE=$(git merge-base "$CURRENT_HEAD^1" "$PULL_REQUEST_TARGET_BRANCH" )
			echo "We're a merge commit (1), computed merge base between $BLUE$CURRENT_HEAD^1$CLEAR and $BLUE$PULL_REQUEST_TARGET_BRANCH$CLEAR: $BLUE$MERGE_BASE$CLEAR"
		elif [[ "$GH_HEAD" == "$CURRENT_HEAD" ]]; then
			# we're at the tip of the branch
			MERGE_BASE=$(git merge-base "$CURRENT_HEAD" "$PULL_REQUEST_TARGET_BRANCH" )
			echo "We're at the tip of a branch (1), computed merge base between $BLUE$CURRENT_HEAD$CLEAR and $BLUE$PULL_REQUEST_TARGET_BRANCH$CLEAR: $BLUE$MERGE_BASE$CLEAR"
		elif [[ "$MERGE_COMMIT_SUBJECT" =~ ^Merge" "[a-f0-9]{40}" into "[a-f0-9]{40}$ ]]; then
			# we're a merge commit, get the previous commit
			MERGE_BASE=$(git merge-base "$CURRENT_HEAD^1" "$PULL_REQUEST_TARGET_BRANCH" )
			echo "We're a merge commit (2), computed merge base between $BLUE$CURRENT_HEAD^1$CLEAR and $BLUE$PULL_REQUEST_TARGET_BRANCH$CLEAR: $BLUE$MERGE_BASE$CLEAR"
		else
			# we're at the tip of the branch
			MERGE_BASE=$(git merge-base "$CURRENT_HEAD" "$PULL_REQUEST_TARGET_BRANCH" )
			echo "We're at the tip of a branch (2), computed merge base between $BLUE$CURRENT_HEAD$CLEAR and $BLUE$PULL_REQUEST_TARGET_BRANCH$CLEAR: $BLUE$MERGE_BASE$CLEAR"
		fi

		BASE_HASH=$MERGE_BASE
	else
		BASE_HASH=HEAD^
	fi
elif test -n "$PULL_REQUEST_ID"; then
	echo "${RED}Can't specify both --base and --pull-request.${CLEAR}"
	exit 1
fi

ROOT_DIR=$(git rev-parse --show-toplevel)

# Go to the root directory of the git repo, so that we don't run into any surprises with paths.
# Also make ROOT_DIR an absolute path.
cd "$ROOT_DIR"
ROOT_DIR=$(pwd)

if test -z "$OUTPUT_DIR"; then
	OUTPUT_DIR="$ROOT_DIR/tools/comparison"
fi
mkdir -p "$OUTPUT_DIR"

# Only show git colors locally. The normal "has-controlling-terminal" doesn't work, because
# we always capture the output to indent it (thus the git processes never have a controlling
# terminal)
if test -z "${BUILD_REVISION:-}"; then
	GIT_COLOR=--color=always
	GIT_COLOR_P=(-c "color.status=always")
else
	GIT_COLOR=
	GIT_COLOR_P=(-c "color.status=auto")
fi

GENERATOR_DIFF_FILE=
APIDIFF_FILE=
STABLE_API_COMPARISON_FILE=

OUTPUT_RESULTS_DIR=$OUTPUT_DIR/results
OUTPUT_TMP_DIR=$OUTPUT_DIR/tmp
OUTPUT_SRC_DIR=$OUTPUT_TMP_DIR/src

GH_COMMENTS_FILE=$OUTPUT_RESULTS_DIR/gh-comment.md

rm -f "$GH_COMMENTS_FILE"
mkdir -p "$(dirname "$GH_COMMENTS_FILE")"

if test -z "$SKIP_DIRTY_CHECK"; then
	if [ -n "$(git status --porcelain --ignore-submodule)" ]; then
		report_error_line "${RED}** Error: Working directory isn't clean - check build log for more information.${CLEAR}"
		# The funny GIT_COLOR_P syntax is explained here: https://stackoverflow.com/a/61551944/183422
		git ${GIT_COLOR_P[@]+"${GIT_COLOR_P[@]}"} status --ignore-submodules | sed 's/^/    /' || true
		git ${GIT_COLOR_P[@]+"${GIT_COLOR_P[@]}"} diff --ignore-submodules | sed 's/^/    /' || true
		exit 1
	fi
fi

# Resolve any treeish hash value (for instance HEAD^4) to the unique (MD5) hash
RESOLVED_BASE_HASH=$(git log -1 --pretty=%H "$BASE_HASH")

if [[ "$RESOLVED_BASE_HASH" == "$BASE_HASH" ]]; then
	echo "Comparing the changes between $BLUE$BASE_HASH$CLEAR and $BLUE$CURRENT_HASH$CLEAR:"
else
	echo "Comparing the changes between $BLUE$BASE_HASH$CLEAR ($BLUE$RESOLVED_BASE_HASH$CLEAR) and $BLUE$CURRENT_HASH$CLEAR:"
fi
git log "$BASE_HASH..$CURRENT_HASH" --oneline $GIT_COLOR | sed 's/^/    /'

BASE_HASH=$RESOLVED_BASE_HASH

# We'll clone macios again into a different directory, and build it

function upon_exit ()
{
	if ! test -z "$GENERATOR_DIFF_FILE"; then
		echo "Generator diff: $GENERATOR_DIFF_FILE"
	fi
	if ! test -z "$APIDIFF_FILE"; then
		echo "API diff (vs $BASE_HASH): $APIDIFF_FILE"
	fi
	if ! test -z "$STABLE_API_COMPARISON_FILE"; then
		echo "API diff (vs stable): $STABLE_API_COMPARISON_FILE"
	fi

	# Clean up after ourselves
	if test -z "$KEEP_BUILD"; then
		rm -Rf "$OUTPUT_TMP_DIR"
	fi
}

trap upon_exit EXIT

echo "${BLUE}Current hash: ${WHITE}$(git log -1 --pretty="%h: %s" "$CURRENT_HASH")${BLUE}${CLEAR}"
echo "${BLUE}Base hash: ${WHITE}$(git log -1 --pretty="%h: %s" "$BASE_HASH")${BLUE}${CLEAR}"
echo ""

if test -z "$USE_EXISTING_BUILD"; then
	echo "💪 ${BLUE}Building ${WHITE}$BASE_HASH${BLUE} in ${WHITE}$OUTPUT_SRC_DIR${BLUE}${CLEAR} 💪"
	echo "    ${BLUE}Checking out ${WHITE}$(git log -1 --pretty="%h: %s" "$BASE_HASH")${BLUE} into ${WHITE}$OUTPUT_SRC_DIR${BLUE}...${CLEAR}"

	rm -Rf "$OUTPUT_SRC_DIR"
	mkdir -p "$OUTPUT_SRC_DIR"

	cd "$OUTPUT_SRC_DIR"

	echo "    ${BLUE}Getting ADR from existing clone to avoid having to setup auth to get it again...${CLEAR}"
	FILE="$ROOT_DIR/adr-path.raw"
	make print-variable-value-to-file VARIABLE=ADR_PATH FILE=$FILE -C "$ROOT_DIR/tools/devops"
	ADR_PATH=$(cat "$FILE")
	rm -f "$FILE"
	cp -cr "$ADR_PATH" .

	echo "    ${BLUE}Cloning macios...${CLEAR}"
	git clone https://github.com/dotnet/macios --reference "$ROOT_DIR" 2>&1 | sed 's/^/        /'
	cd macios
	git reset --hard "$BASE_HASH" 2>&1 | sed 's/^/        /'
	if test -f "$ROOT_DIR/configure.inc"; then
		cp "$ROOT_DIR/configure.inc" .
	fi
	echo "    ${BLUE}Building temporary working directory ${WHITE}$OUTPUT_SRC_DIR${BLUE}...${CLEAR}"
	make reset 2>&1 | sed 's/^/    /'
	make check-versions 2>&1 | sed 's/^/    /'
	if ! ./system-dependencies.sh 2>&1 | sed 's/^/        /'; then
		report_error_line "${RED}Error: The system requirements for the hash to compare against ($WHITE$BASE_HASH$CLEAR) are different than for the current hash. Comparison is currently not supported in this scenario.${CLEAR}"
		exit 1
	fi
	if ! make all -j8 2>&1 | sed 's/^/        /'; then
		report_error_line "${RED}Error: 'make' failed for the hash $WHITE$BASE_HASH$CLEAR.${CLEAR}"
		exit 1
	fi
	if ! make install -j8 2>&1 | sed 's/^/        /'; then
		report_error_line "${RED}Error: 'make install' failed for the hash $WHITE$BASE_HASH$CLEAR.${CLEAR}"
		exit 1
	fi
	echo "    ${BLUE}Build completed for ${WHITE}$BASE_HASH${BLUE}"
echo ""
else
	if ! test -d "$OUTPUT_SRC_DIR"; then
		report_error_line "${RED}Can't use an existing build, because the temporary build directory $WHITE$OUTPUT_SRC_DIR$RED does not exist.${CLEAR}"
		exit 1
	else
		echo "${BLUE}Using existing build in ${WHITE}$OUTPUT_SRC_DIR${BLUE}.${CLEAR}"
	fi
fi

if test -n "$ENABLE_GENERATOR_DIFF"; then
	#
	# Generator diff vs another commit
	#
	echo "💪 ${BLUE}Computing generator diff${CLEAR} 💪"

	# We make a copy of the generated source code to compare against,
	# so that we can remove files we don't want to compare without
	# affecting that build.
	rm -rf "$OUTPUT_TMP_DIR/generator"
	mkdir -p "$OUTPUT_TMP_DIR/generator/build"
	echo "    ${BLUE}Copying ${WHITE}$ROOT_DIR/src/build${BLUE} to ${WHITE}$OUTPUT_TMP_DIR/generator/build/new${BLUE}...${CLEAR}"
	$CP -R "$ROOT_DIR/src/build" "$OUTPUT_TMP_DIR/generator/build/new"
	echo "    ${BLUE}Copying ${WHITE}$OUTPUT_SRC_DIR/macios/src/build${BLUE} to ${WHITE}$OUTPUT_TMP_DIR/generator/build/old${BLUE}...${CLEAR}"
	$CP -R "$OUTPUT_SRC_DIR/macios/src/build" "$OUTPUT_TMP_DIR/generator/build/old"

	# delete files we don't care are different
	echo "    ${BLUE}Deleting files from ${WHITE}$OUTPUT_TMP_DIR/generator${BLUE} we don't care about...${CLEAR}"
	find "$OUTPUT_TMP_DIR/generator" '(' \
		-name '*.AssemblyInfo.cs' -or \
		-name '*.sourcelink.json' -or \
		-name '*.csproj.FileListAbsolute.txt' -or \
		-name 'SourceLink.json' -or \
		-name 'ApiDefinition.*.csproj' -or \
		-name 'Microsoft.*.csproj' -or \
		-name 'Core.*.csproj' -or \
		-name 'compiler' -or \
		-name 'bgen' -or \
		-name '*.dll' -or \
		-name '*.editorconfig' -or \
		-name '*.pdb' -or \
		-name '*generated-sources' -or \
		-name 'generated_sources' -or \
		-name '*.exe' -or \
		-name '*.rsp' -or \
		-name 'AssemblyInfo.cs' -or \
		-name 'Constants.cs' -or \
		-name 'generator.csproj*' -or \
		-name 'bgen.csproj.*' -or \
		-name 'bgen.runtimeconfig.dev.json' -or \
		-name 'PublishOutputs.*.txt' -or \
		-name '*.binlog' -or \
		-name '*.cache' \
		')' -delete
	mkdir -p "$OUTPUT_RESULTS_DIR/generator-diff"
	GENERATOR_DIFF_FILE="$OUTPUT_RESULTS_DIR/generator-diff/generator.diff"
	GENERATOR_HTML_FILE="$OUTPUT_RESULTS_DIR/generator-diff/index.html"
	echo "    ${BLUE}Computing diff of the generated sources into ${WHITE}$GENERATOR_DIFF_FILE${BLUE}...${CLEAR}"
	cd "$OUTPUT_TMP_DIR/generator/build"
	git diff --no-index old new > "$GENERATOR_DIFF_FILE" || true
	"$ROOT_DIR/tools/diff-to-html" "$GENERATOR_DIFF_FILE" "$GENERATOR_HTML_FILE"
	echo "    ${BLUE}Computed generator diff: ${WHITE}$GENERATOR_HTML_FILE${BLUE}.${CLEAR}"
	echo ""
else
	echo "${BLUE}Skipped generator diff.${CLEAR}"
fi

APIDIFF_RESULTS_RELATIVE_DIR=previous-api-comparison
APIDIFF_RESULTS_DIR=$OUTPUT_RESULTS_DIR/$APIDIFF_RESULTS_RELATIVE_DIR
APIDIFF_TMP_DIR=$OUTPUT_TMP_DIR/$APIDIFF_RESULTS_RELATIVE_DIR
if test -n "$ENABLE_API_DIFF"; then
	#
	# API diff vs another commit
	#
	#   First we calculate the apidiff references for the hash we're comparing against
	#   Then we restore the original hash, and finally we calculate the api diff.
	#
	echo "💪 ${BLUE}Computing API diff vs ${WHITE}${BASE_HASH}${BLUE}${CLEAR} 💪"
	APIDIFF_FILE=$APIDIFF_RESULTS_DIR/api-diff.html
	APIDIFF_MARKDOWN=$APIDIFF_RESULTS_DIR/api-diff.md

	( # use a sub-shell to be able to set environment variables here and not affect the rest of the script
		export APIDIFF_DIR="$APIDIFF_TMP_DIR"
		export OUTPUT_DIR="$APIDIFF_RESULTS_DIR"

		rm -rf "$APIDIFF_RESULTS_DIR"
		mkdir -p "$APIDIFF_RESULTS_DIR"

		# get all the currently selected platforms
		FILE="$ROOT_DIR/tools/devops/DOTNET_PLATFORMS.txt"
		make print-variable-value-to-file VARIABLE=DOTNET_PLATFORMS FILE=$FILE -C "$ROOT_DIR/tools/devops"
		DOTNET_PLATFORMS=($(cat $FILE))
		rm -f "$FILE"

		shopt -s nullglob

		# Point our apidiff to the dlls of the other commit
		for platform in "${DOTNET_PLATFORMS[@]}"; do
			dlls=($OUTPUT_SRC_DIR/macios/_build/Microsoft.$platform.Ref.*/ref/net*/Microsoft.$platform.dll)
			if [[ ${#dlls[@]} != 1 ]]; then
				report_error_line "${RED}Unable to find exactly one assembly, found ${#dlls[@]} assemblies:${CLEAR}"
				report_error_line "${dlls[@]}"
				exit 1
			fi

			# store the first dll in the COMPARISON_DLL_<platform> variable
			printf -v "COMPARISON_DLL_$platform" "%s" "${dlls[0]}"
			# export it for the apidiff makefile
			eval "export COMPARISON_DLL_$platform"
		done

		# Clean up a bit before we do anything
		( cd "$ROOT_DIR/tools/apidiff" && git clean -xfdq )

		# Now compare the current build against those references
		echo "    ${BLUE}Running apidiff...${CLEAR}"
		if ! make all-local -C "$ROOT_DIR/tools/apidiff" -j8 2>&1 | sed 's/^/        /'; then
			EC=${PIPESTATUS[0]}
			report_error_line "${RED}Failed to run apidiff${CLEAR}"
			exit "$EC"
		fi
	)
	echo "    ${BLUE}Computed API diff vs ${WHITE}${BASE_HASH}${BLUE}:${CLEAR}"
	echo "        ${WHITE}$APIDIFF_FILE${CLEAR}"
	echo "        ${WHITE}$APIDIFF_MARKDOWN	${CLEAR}"
	echo ""
else
	echo "${BLUE}Skipped API diff vs ${WHITE}${BASE_HASH}${BLUE}.${CLEAR}"
fi

STABLE_API_COMPARISON_RESULTS_RELATIVE_DIR=stable-api-comparison
STABLE_API_COMPARISON_RESULTS_DIR="$OUTPUT_RESULTS_DIR/$STABLE_API_COMPARISON_RESULTS_RELATIVE_DIR"
STABLE_API_COMPARISON_TMP_DIR="$OUTPUT_TMP_DIR/$STABLE_API_COMPARISON_RESULTS_RELATIVE_DIR"
if test -n "$ENABLE_STABLE_API_COMPARISON"; then
	echo "💪 ${BLUE}Computing API diff vs stable${CLEAR} 💪"
	rm -Rf "$STABLE_API_COMPARISON_RESULTS_DIR" "$STABLE_API_COMPARISON_TMP_DIR"
	mkdir -p "$STABLE_API_COMPARISON_RESULTS_DIR" "$STABLE_API_COMPARISON_TMP_DIR"

	# Clean up a bit before we do anything
	( cd "$ROOT_DIR/tools/apidiff" && git clean -xfdq )

	# Now compare the current build against those references
	echo "    ${BLUE}Running apidiff...${CLEAR}"
	make -j8 -C "$ROOT_DIR/tools/apidiff" APIDIFF_DIR="$STABLE_API_COMPARISON_TMP_DIR" OUTPUT_DIR="$STABLE_API_COMPARISON_RESULTS_DIR" 2>&1 | sed 's/^/        /'

	# remove empty files
	find "$STABLE_API_COMPARISON_RESULTS_DIR" -size 0 -print0 | xargs -0 rm
	STABLE_API_COMPARISON_FILE=$STABLE_API_COMPARISON_RESULTS_DIR/api-diff.html
	STABLE_API_COMPARISON_MARKDOWN=$STABLE_API_COMPARISON_RESULTS_DIR/api-diff.md

	echo "    ${BLUE}Computed API diff vs stable:${CLEAR}"
	echo "        ${WHITE}$STABLE_API_COMPARISON_FILE${CLEAR}"
	echo "        ${WHITE}$STABLE_API_COMPARISON_MARKDOWN${CLEAR}"
	echo ""
else
	echo "${BLUE}Skipped API diff vs stable.${CLEAR}"
fi

if [ -z ${INCLUDE_IOS+x} ]; then INCLUDE_IOS=$(make -C "$ROOT_DIR"/tools/devops print-variable VARIABLE=INCLUDE_IOS | sed 's/.*=//'); fi
if [ -z ${INCLUDE_TVOS+x} ]; then INCLUDE_TVOS=$(make -C "$ROOT_DIR"/tools/devops print-variable VARIABLE=INCLUDE_TVOS | sed 's/.*=//'); fi
if [ -z ${INCLUDE_MAC+x} ]; then INCLUDE_MAC=$(make -C "$ROOT_DIR"/tools/devops print-variable VARIABLE=INCLUDE_MAC | sed 's/.*=//'); fi
if [ -z ${DOTNET_PLATFORMS+x} ]; then DOTNET_PLATFORMS=$(make -C "$ROOT_DIR"/tools/devops print-variable VARIABLE=DOTNET_PLATFORMS | sed 's/.*=//'); fi
if [ -z ${DOTNET_TFM+x} ]; then DOTNET_TFM=$(make -C "$ROOT_DIR"/tools/devops print-variable VARIABLE=DOTNET_TFM | sed 's/.*=//'); fi

# Create the GH comment

if test -n "$ENABLE_API_DIFF"; then
	if grep BreakingChangesDetected "$APIDIFF_RESULTS_DIR/api-diff.md" >/dev/null 2>&1; then
		EMOJII=":heavy_exclamation_mark:"
		MSG=" (Breaking changes)"
	else
		EMOJII=":white_check_mark:"
		MSG=""
	fi
	echo "## $EMOJII API diff for current PR / commit$MSG" >> "$GH_COMMENTS_FILE"
	echo "" >> "$GH_COMMENTS_FILE"
	sed < "$APIDIFF_RESULTS_DIR/api-diff.md" -e "s/[[]vsdrops[]][\(]/[vsdrops]($APIDIFF_RESULTS_RELATIVE_DIR\//g" -e "s/[[]gist[]][\(]/[gist]($APIDIFF_RESULTS_RELATIVE_DIR\//g" >> "$GH_COMMENTS_FILE"
	echo "" >> "$GH_COMMENTS_FILE"
fi

if test -n "$ENABLE_STABLE_API_COMPARISON"; then
	if grep BreakingChangesDetected "$STABLE_API_COMPARISON_RESULTS_DIR/api-diff.md" >/dev/null 2>&1; then
		EMOJII=":heavy_exclamation_mark:"
		MSG=" (Breaking changes)"
	else
		EMOJII=":white_check_mark:"
		MSG=""
	fi
	echo "## $EMOJII API diff vs stable$MSG" >> "$GH_COMMENTS_FILE"
	echo "" >> "$GH_COMMENTS_FILE"
	sed < "$STABLE_API_COMPARISON_RESULTS_DIR/api-diff.md" -e "s/[[]vsdrops[]][\(]/[vsdrops]($STABLE_API_COMPARISON_RESULTS_RELATIVE_DIR\//g" -e "s/[[]gist[]][\(]/[gist]($STABLE_API_COMPARISON_RESULTS_RELATIVE_DIR\//g"  >> "$GH_COMMENTS_FILE"
	echo "" >> "$GH_COMMENTS_FILE"
fi

if test -n "$ENABLE_GENERATOR_DIFF"; then
	if test -s "$GENERATOR_DIFF_FILE"; then
		echo "## :information_source: Generator diff" >> "$GH_COMMENTS_FILE"
		echo "" >> "$GH_COMMENTS_FILE"
		echo "Generator Diff: [vsdrops](generator-diff/index.html) (html) [vsdrops](generator-diff/generator.diff) (raw diff) [gist](generator-diff/generator.diff) (raw diff) - Please review changes)" >> "$GH_COMMENTS_FILE"
	else
		echo "## :white_check_mark: Generator diff" >> "$GH_COMMENTS_FILE"
		echo "" >> "$GH_COMMENTS_FILE"
		echo "Generator diff is empty" >> "$GH_COMMENTS_FILE"
	fi
fi

