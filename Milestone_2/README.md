# CST-350 Milestone 2 - Playable Game

## Cover Sheet
**Student Name:** Alex Frear  
**Date:** 11/15/2024  
**Program:** College of Science, Engineering, and Technology, Grand Canyon University  
**Course:** CST-350 Programming in C# III  
**Instructor:** Brandon Bass  

## Screencast Video
<div>
    <a href="https://www.loom.com/share/your-video-link">
      <p>CST 350 - Milestone 2 Minesweeper Functionality - Watch Video</p>
    </a>
(Tip: Right-click and select "Open link in new tab" to watch the video without leaving the page)
    <a href="https://www.loom.com/share/your-video-link">
      <img style="max-width:300px;" src="https://cdn.loom.com/sessions/thumbnails/your-video-link-thumbnail.gif">
    </a>
  </div>

## Application Overview

- **Below are screenshots demonstrating the different steps and pages created as part of Milestone 2 - MinesweeperMVC Application.**

### 1. Home Page
<img src="../Screenshots/HomePage.png" width="700"/>

*This screenshot shows the home page, providing navigation links for Login, Register, and Start Game.*

---

### 2. Login and Registration Pages

#### Login Page
<img src="../Screenshots/LoginPage.png" width="700"/>

*This screenshot shows the Login page where users can enter their credentials.*

#### Login Page with Validation
<img src="../Screenshots/LoginPageValidation.png" width="700"/>

*This screenshot demonstrates input validation on the Login page for incorrect or incomplete entries.*

#### Successful Login
<img src="../Screenshots/LoginSuccess.png" width="700"/>

*This screenshot shows the page users see after successfully logging in.*

#### Registration Page
<img src="../Screenshots/RegistrationPage.png" width="700"/>

*This screenshot shows the Registration page for new users.*

#### Registration Page with Validation
<img src="../Screenshots/RegistrationPageValidation.png" width="700"/>

*This screenshot demonstrates input validation on the Registration page.*

---

### 3. Game Setup

#### Start Game Page
<img src="../Screenshots/StartGamePage.png" width="700"/>

*This screenshot shows the Start Game page where users can select board size and difficulty level.*

---

### 4. Gameplay

#### Minesweeper Game Board
<img src="../Screenshots/MinesSweeperBoardPage.png" width="700"/>

*This screenshot shows the Minesweeper game board during gameplay. It includes left-click functionality to reveal cells.*

#### User Access Restriction
<img src="../Screenshots/UserBoardRestriction.png" width="700"/>

*Each user's game session is stored separately. Logging in as a different user allows them to start a new game without affecting the other user's session.*

---

### 5. Game Outcomes

#### Win Page
<img src="../Screenshots/GameWinPage.png" width="700"/>

*This screenshot shows the Win page displayed when the user successfully completes the game. It includes the calculated score.*

#### Loss Page
<img src="../Screenshots/GameLossPage.png" width="700"/>

*This screenshot shows the Loss page displayed when the user uncovers a mine. It includes revealed mines and a failure message.*

---

## Summary of Key Concepts

In Milestone 2, I expanded the functionality of the MinesweeperMVC application by adding core gameplay features. I developed a fully interactive Minesweeper game board as a 2D grid using Razor pages, applying image resources to make the interface both visually appealing and intuitive. I implemented left-click functionality so that users can interact with the cells, and each click updates the button state according to the game logic. The Start Game page was enhanced to allow users to configure the game by selecting the board size and difficulty level.

I ensured that access to the game board was restricted to the specific user who initiated the session, which maintained a personalized gameplay experience. Each user's game session is stored separately, so logging in as a different user allows them to start a new game without interfering with others' sessions. Additionally, I created dedicated pages for both win and loss outcomes, with scores calculated based on factors like time elapsed, board size, and difficulty level.

To achieve these features, I utilized ASP.NET Core MVC architecture, separating concerns between the Models, Controllers, and Views. This structure allowed me to write clean, maintainable code and handle user interactions effectively. I also incorporated static resources such as CSS and images to improve the visual design and overall user experience. This milestone builds on the foundational login and registration functionality from Milestone 1, paving the way for future enhancements, such as right-click actions for flagging cells and the ability to save and restore game progress.
