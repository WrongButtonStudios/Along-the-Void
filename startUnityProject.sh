#!/bin/bash

# Source the user's environment
if [ -f "$HOME/.profile" ]; then
    . "$HOME/.profile"
fi

# Add common paths that might be missing in GUI environment
export PATH="$PATH:/usr/local/bin:/usr/bin:/bin:/usr/local/games:/usr/games:/snap/bin"

# Get the absolute path of the directory where the script is located
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Function to find Unity Editor
find_unity_editor() {
    local unity_versions=( $HOME/Unity/Hub/Editor/*/Editor/Unity )
    printf "%s\n" "${unity_versions[@]}" | sort -r | head -n 1
}

# Find Unity Editor
UNITY_EDITOR=$(find_unity_editor)

if [ -x "$UNITY_EDITOR" ]; then
    # Launch Unity with full command
    nohup "$UNITY_EDITOR" \
        -projectPath "$SCRIPT_DIR" \
        -buildTarget linux64 \
        -forceOpenGLCore \
        -disable-gpu-skinning \
        --no-sandbox \
        > /dev/null 2>&1 &
    
    # Store the PID
    UNITY_PID=$!
    
    # Wait a moment to see if the process is still running
    sleep 2
    if kill -0 $UNITY_PID 2>/dev/null; then
        notify-send "Unity Project Opener" "Opening Unity project at: $SCRIPT_DIR"
        exit 0
    else
        zenity --error --text="Unity failed to start. Please check your Unity installation."
        exit 1
    fi
else
    zenity --error --text="Unity Editor not found. Please make sure Unity is installed correctly."
    exit 1
fi
