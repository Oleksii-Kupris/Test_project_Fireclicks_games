Fireclicks Games Test Assignment – Unity + ASP.NET WebAPI

Overview

This project is a Unity + ASP.NET integration test demonstrating scene management, UI virtualization, logging, encrypted token handling, and client–server communication.

The Unity client communicates with a local WebAPI server, sending encrypted tokens and receiving per-user request counts.

Core Structure

Unity

Scenes:
    •    EntryPoint — entry scene (boot, FSM start)
    •    GameLoop — main scene with tabs and gameplay logic

Game States:
    1.    InitializationState — generates and encrypts a user token
    2.    LoadingState — shows loading screen and transitions to the main scene
    3.    GameLoopState — enables gameplay logic and CPU frame-time overlay
    
Features
    •    Uses Addressables for all scene and UI loading (CpuOverlay, LoadingCanvas)
    •    Virtualized UI List capable of displaying thousands of elements using a lightweight object pool
    •    Grouped List Tab built from a RarityDatabase ScriptableObject.
Each rarity entry (code, color, count) can be configured directly in the asset,
allowing full control over the displayed pattern (Green → Purple → Gold, etc.)
    •    Big List Tab can display any custom number of sequential elements,
not limited to 1000 — the number is passed dynamically to the list binding method.
    •    Request Tab sends the encrypted token to the local API and displays how many times the user has made a request.
    •    CPU Frame Time Overlay shows real CPU frame time excluding v-sync, displayed across all scenes.
    •    Custom Logger with two output modes:
    •    Logs to Unity console (in Editor)
    •    Logs to file (Application.persistentDataPath/log.txt) in builds
    •    AES-256 Token Encryption with CBC mode and PKCS7 padding
    •    Persistent token storage using PlayerPrefs

Running the Server
    1.    Open the Server folder in Visual Studio or Rider.
    2.    Ensure that the .NET SDK version is 7.0 (not 8.0).
    3.    Open Properties/launchSettings.json and confirm the configuration: "applicationUrl": "http://localhost:5119"
    4.    Run the project (Debug or Run configuration).
    5.    If the browser opens at https://localhost:7097/weatherforecast, the API is running correctly.
    
Running the Unity Client
    1.    Open the Unity project in Unity 6000.2.7f2.
    2.    Set the EntryPoint scene as the first scene in Build Settings.
    3.    Press Play.
    4.    Wait for the loading screen to transition to the main GameLoop scene.
    5.    Use the available tabs:
    •    Big List Tab — displays a dynamically generated list of elements (default 1000, configurable in code).
    •    Grouped List Tab — displays items with colors and counts defined in the RarityDatabase asset.
    •    Request Tab — sends encrypted token requests to the local WebAPI server and shows the request count.

Each request is stored per-user on the server side and increases with each click.
Results are logged both in the Unity console and in the local log file.
    
