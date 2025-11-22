# Project Grupowy

## Getting Started

### Launching the Project

To get a local copy of the project, clone the repository:
```bash
git clone <repository-url>
```

Then, open the `Project_grupowy` directory in the Unity Editor.

## Project Structure

This project follows a standard Unity folder structure. Here are some key directories:

*   **`Assets/`**: This is the main folder for all project-specific content including:
    *   **`Assets/Scenes/`**: Contains all Unity scene files (`.unity`).
    *   **`Assets/Scripts/`**: (Proposed) Location for all C# scripts and game logic.
    *   **`Assets/Sprites/`**: (Proposed) Location for 2D image assets.
    *   **`Assets/Prefabs/`**: (Proposed) Location for reusable GameObjects.
    *   **`Assets/Materials/`**: (Proposed) Location for materials.
    *   **`Assets/Animations/`**: (Proposed) Location for animation assets.
    *   **`Assets/Audio/`**: (Proposed) Location for audio files.
    *   **`Assets/InputSystem_Actions.inputactions`**: Input Action Asset for the new Unity Input System.
    *   **`Assets/Settings/`**: Contains project-specific settings like `UniversalRP.asset` and `Renderer2D.asset`.

*   **`ProjectSettings/`**: Contains various Unity project configuration files (e.g., Physics, Quality, Tag Manager). These are generally committed to version control.

*   **`Packages/`**: Managed by the Unity Package Manager (UPM). Contains `manifest.json` and `packages-lock.json` which track project dependencies.

*   **`Library/`**: Contains locally cached asset data, imported assets, and other generated files. This folder is typically ignored by version control (`.gitignore`).

*   **`UserSettings/`**: Contains user-specific settings for the Unity Editor. This folder is typically ignored by version control (`.gitignore`).

### Adding Code (Feature Branch Workflow)
We follow a feature branch workflow for adding new code.

1.  **Create a new branch:**
    ```bash
    git checkout -b feature/your-feature-name
    ```
2.  **Make your changes.**
3.  **Commit your changes:**
    ```bash
    git add .
    git commit -m "feat: Add your feature description"
    ```
4.  **Push your branch:**
    ```bash
    git push origin feature/your-feature-name
    ```
5.  **Create a Pull Request** on GitHub to merge your feature branch into `main`.