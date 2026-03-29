# Project Grupowy

## Definition of Done (DoD)

A Product Backlog Item (User Story) is only considered "Done" and ready to be closed when all of the following conditions are met by the development team:

1. **Code & Assets Integrated:** All necessary code is written, cleanly formatted, and integrated into the Unity project without breaking existing features.

2. **No Console Errors:** The Unity Console displays zero errors and no critical warnings during gameplay.

3. **Acceptance Criteria Met:** Every specific Acceptance Criterion defined in the GitHub Issue has been manually tested and passed.

4. **Peer Reviewed:** The feature or code changes have been reviewed and approved by at least one other team member (e.g., via a GitHub Pull Request).

5. **Merged to Main:** The branch containing the finished feature is successfully merged into the main branch on GitHub with all merge conflicts resolved.

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