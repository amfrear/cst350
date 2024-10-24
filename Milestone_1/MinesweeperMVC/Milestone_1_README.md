# MinesweeperMVC - Milestone 1

## Cover Sheet
**Student Name:** Alex Frear  
**Date:** 10/28/2024  
**Program** College of Science, Engineering, and Technology, Grand Canyon University  
**Course:** CST-350 Programming in C# III  
**Instructor:** Brandon Bass  

## Screencast Demonstration
[Screencast Link - Placeholder](#)  
*This link will be updated once the screencast is available.*  

## GitHub Repository
Link to the [MinesweeperMVC GitHub Repository](https://gitlab.com/amfrear1/cst350/-/tree/main/Milestone_1/MinesweeperMVC?ref_type=heads).  

## Application Screenshots

### 1. Registration Page
<img src="Milestone_1/Milestone1Screenshots/Registration_Form_Browser.png" width="700"/>

*This screenshot shows the registration form where new users can enter their details to create an account.*

### 2. Registration Form Validation
<img src="Milestone_1/Milestone1Screenshots/Registration_Form_Validation_Browser.png" width="700"/>

*This screenshot shows the validation errors on the registration form, indicating that all fields are required.*

### 3. Registration Successful
<img src="Milestone_1/Milestone1Screenshots/Registration_Success_Browser.png" width="700"/>

*This screenshot shows the success message after the user has successfully registered an account.*

### 4. Login Page - Entering Credentials
<img src="Milestone_1/Milestone1Screenshots/Login_Page_Browser.png" width="700"/>

*This screenshot shows the login form where the user enters their username and password to log into the application.*

### 5. Login Page - Invalid Credentials
<img src="Milestone_1/Milestone1Screenshots/Login_InvalidCredentials_Browser.png" width="700"/>

*This screenshot shows the error message displayed when the user enters an incorrect username or password on the login page.*

### 6. Login Successful
<img src="Milestone_1/Milestone1Screenshots/Login_Successful_Browser.png" width="700"/>

*This screenshot shows the success message after the user logs in successfully.*

### 7. Start Game (Logged In)
<img src="Milestone_1/Milestone1Screenshots/StartGame_LoggedIn_Browser.png" width="700"/>

*This screenshot shows the Start Game page, which is only accessible to logged-in users. The page contains placeholder text.*

## Database Setup

To recreate the database, follow these steps:

1. **Install MySQL**: Ensure that you have MySQL installed on your local machine, along with **MySQL Workbench**.
2. **Download the SQL File**: The `minesweepermvc_db.sql` file is located here: [MinesweeperMVC Database Schema](https://gitlab.com/amfrear1/cst350/-/tree/main/Milestone_1/Database?ref_type=heads).
3. **Open MySQL Workbench**:
   - Connect to your MySQL instance.
   - Create a new database/schema.
4. **Import the SQL Dump**:
   - Use the Data Import feature in MySQL Workbench.
   - Select the `minesweepermvc_db.sql` file from the `/Database` folder.
   - Import the database into the schema you created.
5. **Update `appsettings.json`**:
   - Open the `appsettings.json` file in the project.
   - Update the connection string with your MySQL credentials:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "server=localhost;port=3306;database=your_db_name;user=your_user;password=your_password;"
   }
   ```

## Summary of Key Concepts

In Milestone 1 of the MinesweeperMVC project, we demonstrated the following key concepts:
- **Session Management**: We used session-based authentication to control user access, ensuring only logged-in users can access certain pages like the Start Game page. Session variables like the username were stored to track the logged-in status.
- **Login and Registration Forms**: These forms allow users to create new accounts and log into the application. We implemented server-side validation, hashed passwords for security, and stored user data in the MySQL database.
- **Routing and Navigation**: We configured routes for the login, register, and Start Game pages. We also dynamically updated the navigation bar based on the user's session status, displaying appropriate links like Login, Register, Start Game, and Logout based on whether the user was logged in or not.
- **ASP.NET MVC Architecture**: We applied the Model-View-Controller pattern to separate the responsibilities of data handling (Model), user interface (View), and business logic (Controller).

These core concepts provide the foundation for the MinesweeperMVC application, which we will build upon in future milestones.
