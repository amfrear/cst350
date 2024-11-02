# CST-350 Milestone 1 - Login and Registration Modules

## Cover Sheet
**Student Name:** Alex Frear  
**Date:** 11/02/2024  
**Program:** College of Science, Engineering, and Technology, Grand Canyon University  
**Course:** CST-350 Programming in C# III  
**Instructor:** Brandon Bass  

## Screencast Video
<div>
    <a href="https://www.loom.com/share/9be9cb9eda3245258c0205d148a190e1">
      <p>CST 350 - Milestone 1 Login and Registration Modules - Watch Video</p>
    </a>
(Tip: Right-click and select "Open link in new tab" to watch the video without leaving the page)
    <a href="https://www.loom.com/share/9be9cb9eda3245258c0205d148a190e1">
      <img style="max-width:300px;" src="https://cdn.loom.com/sessions/thumbnails/9be9cb9eda3245258c0205d148a190e1-807bf83d0e3d5b89-full-play.gif">
    </a>
  </div>

## Application Overview

- **Below are screenshots demonstrating the different steps and pages created as part of Milestone 1 - MinesweeperMVC Application.**

### 1. Registration Page with Validation
<img src="../Screenshots/RegisterFormValidation.png" width="700"/>

*This screenshot shows the Registration page with all form fields requiring validation. The user must enter a valid First Name, Last Name, Sex, Age, State, Email Address, Username, and Password to register.*

### 2. Registration Form (Empty)
<img src="../Screenshots/RegisterPage.png" width="700"/>

*This shows the initial view of the Registration page when the user first accesses it.*

### 3. Registration Success
<img src="../Screenshots/RegisterSuccessPage.png" width="700"/>

*This screenshot shows the Registration Success page, confirming that the account was successfully created.*

### 4. Login Page
<img src="../Screenshots/LoginPage.png" width="700"/>

*The Login page is used by users to authenticate and access the application using their Username and Password.*

### 5. Login Success Page
<img src="../Screenshots/LoginSuccessPage.png" width="700"/>

*This screenshot shows the Login Success page that the user is redirected to after a successful login.*

### 6. Start Game Page (Restricted Access)
<img src="../Screenshots/StartGameRestrictedPage.png" width="700"/>

*This screenshot shows the "Start Game" page. It is only accessible to users who are logged in. Here, the user can proceed to play Minesweeper after logging in.*

## Summary of Key Concepts

In Milestone 1, I built the foundational components of the MinesweeperMVC application using ASP.NET Core MVC. I implemented user registration and login functionality, including input validation, using ASP.NET Identity. Additionally, I managed user sessions with ASP.NET Core to allow restricted access to specific pages, such as the "Start Game" page. The application was designed using the MVC (Model-View-Controller) architecture, which helped separate data, user interface, and application logic.

To enhance usability, I extended the navigation bar to include links for "Register," "Login," "Logout," and "Start Game," providing easy access to core features. I also used ViewData to transfer information between controllers and views, making user interactions and page transitions smooth.

This milestone focused on building user management and authentication features, setting up the foundation for implementing the core Minesweeper gameplay features in subsequent milestones.
