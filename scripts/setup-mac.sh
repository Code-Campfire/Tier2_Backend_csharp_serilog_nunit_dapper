#!/bin/bash

# Codefire C# (.NET) Serilog, nUnit, Dapper Setup Script for Mac/Linux
echo "===== Starting Codefire C# (.NET) Setup for Mac/Linux ====="

# Navigate to project root (regardless of where the script is run from)
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"
cd "$PROJECT_DIR"
echo "Working directory: $(pwd)"

# Check if .NET SDK is installed
if ! command -v dotnet &> /dev/null; then
    echo ".NET SDK is not installed. Please install .NET SDK before proceeding."
    echo "Visit https://dotnet.microsoft.com/download to download and install .NET SDK."
    
    # Try to open the download page if possible
    if command -v open &> /dev/null; then
        echo "Opening the .NET download page in your browser..."
        open "https://dotnet.microsoft.com/en-us/download/dotnet/8.0"
    elif command -v xdg-open &> /dev/null; then
        echo "Opening the .NET download page in your browser..."
        xdg-open "https://dotnet.microsoft.com/en-us/download/dotnet/8.0"
    fi
    
    exit 1
fi

# Check .NET version
DOTNET_VERSION=$(dotnet --version)
DOTNET_MAJOR_VERSION=$(echo "$DOTNET_VERSION" | cut -d. -f1)

echo ".NET SDK version detected: $DOTNET_VERSION"

# Check for .NET 8
if [ "$DOTNET_MAJOR_VERSION" -lt 8 ]; then
    echo "This project requires .NET 8.0 or higher. You have .NET $DOTNET_VERSION installed."
    echo "Please install .NET 8.0 SDK from: https://dotnet.microsoft.com/en-us/download/dotnet/8.0"
    
    # Try to open the download page if possible
    if command -v open &> /dev/null; then
        echo "Opening the .NET 8 download page in your browser..."
        open "https://dotnet.microsoft.com/en-us/download/dotnet/8.0"
    elif command -v xdg-open &> /dev/null; then
        echo "Opening the .NET 8 download page in your browser..."
        xdg-open "https://dotnet.microsoft.com/en-us/download/dotnet/8.0"
    fi
    
    # Ask if user wants to continue anyway
    read -p "Do you want to continue anyway? This may not work correctly. (y/n) " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
fi

echo "===== Restoring NuGet packages ====="
dotnet restore

echo "===== Building solution ====="
dotnet build --no-restore

echo "===== Running tests ====="
dotnet test --no-build

echo "===== Testing the API setup ====="
echo "Attempting to start the API for 5 seconds to test the setup..."
# Change directory to the API project
cd src/CodeFire.API

# Function to run a command with a timeout without requiring the 'timeout' command
run_with_timeout() {
    # Start the command in the background
    "$@" &
    PID=$!
    
    # Sleep for 5 seconds
    sleep 5
    
    # Kill the process if it's still running
    if kill -0 $PID 2>/dev/null; then
        kill $PID 2>/dev/null || kill -9 $PID 2>/dev/null
        echo "API started successfully (stopped after 5 seconds as expected)."
        return 0
    else
        wait $PID
        if [ $? -eq 0 ]; then
            echo "API started and exited successfully."
            return 0
        else
            echo "Warning: There was an issue starting the API."
            return 1
        fi
    fi
}

# Run dotnet with our custom timeout function
run_with_timeout dotnet run --no-build

# Go back to project root
cd "$PROJECT_DIR"

echo ""
echo "===== Setup Complete! ====="
echo "To run the API, navigate to the API project directory and use: dotnet run"
echo "  cd $(pwd)/src/CodeFire.API"
echo "  dotnet run"
echo "This will start the API at https://localhost:5001 and http://localhost:5000"
echo ""
echo "To run tests, use: dotnet test"
echo "  cd $(pwd)"
echo "  dotnet test"
echo "" 