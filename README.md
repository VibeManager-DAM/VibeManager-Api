# VibeManager-Api

**VibeManager-Api** is the backend API developed to support the **Vibe** ecosystem. It connects the **VibeManager** Android application, the centralized **Vibe** database, and other services within the organization. This API manages users, sessions, music data, and interactions across platforms to deliver a seamless user experience.

## Features

- **User Management**: Handles registration, authentication, and profile management.
- **Session Tracking**: Manages listening sessions and activity logging across devices.
- **Real-Time Sync**: Ensures data consistency and synchronization between the mobile app and backend.
- **Admin Tools**: Offers administrative capabilities to monitor and maintain the system.

### Technologies Used

- **.NET Framework**: The framework used to build the API, ensuring performance and robustness.
- **C#**: The primary programming language for the API.
- **Entity Framework**: Used for managing database interactions.
- **JWT Authentication**: For secure user authentication using JSON Web Tokens.
- **SignalR**: For real-time communication (e.g., chat or notifications).

### Installation

1. Clone the repository:
   ```bash
   [git clone https://github.com/VibeManager/vibe-api.git](https://github.com/VibeManager-DAM/VibeManager-Api.git)
    ````

2. Open the project in your preferred IDE or code editor.

3. Install dependencies:

   ```bash
   npm install
   # or for Python/Dotnet/etc.
   ```

4. Configure environment variables:

   * Copy `.env.example` to `.env` and update your database and API keys accordingly.

5. Start the server:

   ```bash
   npm start
   # or equivalent for your tech stack
   ```

### Usage

This API is used primarily by the following:

* [**VibeManager Android App**](https://github.com/VibeManager-DAM/Vibe-Mobile)

### Contributing

Feel free to fork the repository, submit issues, or create pull requests to help improve the project. We welcome any contributions from the community!

### License

This project is licensed under the MIT License – see the [LICENSE](LICENSE) file for details.


