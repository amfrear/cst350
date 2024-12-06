# CST-350 Milestone 4 - Save/Restore Game Progress and REST API Features

## Cover Sheet
**Student Name:** Alex Frear  
**Date:** 12/06/2024  
**Program:** College of Science, Engineering, and Technology, Grand Canyon University  
**Course:** CST-350 Programming in C# III  
**Instructor:** Brandon Bass  

## Screencast Videos
<div>
    <a href="https://www.loom.com/share/338e115b93e14562b7bf6bf70231afa3">
      <p>CST350 - Milestone 4 - Code Review - Watch Video</p>
    </a>
    <a href="https://www.loom.com/share/338e115b93e14562b7bf6bf70231afa3">
      <img style="max-width:300px;" src="https://cdn.loom.com/sessions/thumbnails/338e115b93e14562b7bf6bf70231afa3-b886ccc4159d32c1-full-play.gif">
    </a>
  </div>

<div>
    <a href="https://www.loom.com/share/2e09d228753c468a8e7de6b41a06f016">
      <p>CST350 -Milestone 4 - Application Running - Watch Video</p>
    </a>
    <a href="https://www.loom.com/share/2e09d228753c468a8e7de6b41a06f016">
      <img style="max-width:300px;" src="https://cdn.loom.com/sessions/thumbnails/2e09d228753c468a8e7de6b41a06f016-5368a1b6857f3d78-full-play.gif">
    </a>
  </div>

---

## Application Overview

- **Below are screenshots demonstrating the features implemented as part of Milestone 4 - Save/Restore Game Progress and REST API Features.**

---

### 1. Database Table - Games
<img src="Milestone4Screenshots/1_gamesTableViewInMySqlWorkbench_MySqlWorkbench.png" width="700"/>

*This screenshot shows the `Games` table in the database, which stores the game state, user ID, date saved, and serialized game data.*

---

### 2. Initial Game State
<img src="Milestone4Screenshots/2_NewGameInitialState_Browser.png" width="700"/>

*This screenshot displays the initial game board state before saving progress.*

---

### 3. Game Saved Successfully
<img src="Milestone4Screenshots/3_NewGameSuccessfulSave_Browser.png" width="700"/>

*This screenshot demonstrates a successful game save operation. The game state is serialized and stored in the database.*

---

### 4. Saved Games Page Updated
<img src="Milestone4Screenshots/4_SavedGamesPageUpdatedWithNewSavedGame_Browser.png" width="700"/>

*This screenshot shows the `Saved Games` page updated with the newly saved game.*

---

### 5. Load Game Confirmation
<img src="Milestone4Screenshots/5_LoadGameVerificationOnSavedGamesPage_Browser.png" width="700"/>

*This screenshot shows the confirmation dialog displayed when the user selects a saved game to load.*

---

### 6. Loading a Game
<img src="Milestone4Screenshots/6_GameLoadSuccess_Browser.png" width="700"/>

*This screenshot demonstrates the successful loading of a previously saved game.*

---

### 7. Updating a Saved Game
<img src="Milestone4Screenshots/7_GameUpdatedSameGameSave_Browser.png" width="700"/>

*This screenshot shows the functionality of updating the same saved game to reflect the latest state.*

---

### 8. Deleting a Saved Game
<img src="Milestone4Screenshots/8_GameDeleteVerification_Browser.png" width="700"/>

*This screenshot shows the confirmation message after a user selects a saved game for deletion.*

---

### 9. Saved Games Page Updated After Deletion
<img src="Milestone4Screenshots/9_UpdatedSavedGamesPageAfterDelete.png" width="700"/>

*This screenshot shows the `Saved Games` page updated after a game was deleted.*

---

### 10. REST API - List All Saved Games
<img src="Milestone4Screenshots/10_InitialShowSavedGamesRequest_Postman.png" width="700"/>

*This screenshot shows the REST API endpoint `localhost/api/showSavedGames`, which lists all saved games.*

---

### 11. REST API - Retrieve a Single Game by ID
<img src="Milestone4Screenshots/11_ShowSavedGameByIdRequest_Postman.png" width="700"/>

*This screenshot shows the REST API endpoint `localhost/api/showSavedGames/{id}`, which retrieves a single saved game by its ID.*

---

### 12. REST API - Delete a Specific Game
<img src="Milestone4Screenshots/12_DeleteOneGameRequest_Postman.png" width="700"/>

*This screenshot shows the REST API endpoint `localhost/api/deleteOneGame/{id}`, which deletes a specific saved game.*

---

### 13. Verification After Deletion
<img src="Milestone4Screenshots/13_ShowSavedGamesRquestAfterSuccessfulDelete_Postman.png" width="700"/>

*This screenshot confirms the successful deletion of a saved game via the REST API.*

---

## Summary of Key Concepts

In Milestone 4, I focused on enhancing the MinesweeperMVC application by implementing features for saving and restoring game progress, along with REST API functionality. I added a "Save Game" button that serializes the game state, including the game board and user information, and stores it in a database. Additionally, I created a user-friendly interface that allows users to view, load, or delete saved games. On the backend, I developed RESTful API endpoints to list all saved games, retrieve specific games by ID, and delete games from the database. These enhancements allowed me to integrate data serialization, database operations, and REST API development into a single cohesive project. This milestone not only helped me solidify my understanding of these core concepts but also gave me valuable hands-on experience in building dynamic applications with robust backend services in ASP.NET Core.

---
