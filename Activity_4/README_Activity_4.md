# CST-350 Activity 4 - Dependency Injection, Data Validation, and ButtonGrid

## Cover Sheet
**Student Name:** Alex Frear  
**Date:** 11/7/2024  
**Program:** College of Science, Engineering, and Technology, Grand Canyon University  
**Course:** CST-350 Programming in C# III  
**Instructor:** Brandon Bass  

---

## Part 1: Dependency Injection Overview
- **This section demonstrates the implementation of dependency injection in `UserController` to use the `IUserManager` interface, allowing for flexibility in selecting different data sources.**

### 1. Configuring Dependency Injection in `UserController`
<img src="../Activity4Screenshots/Part1Screenshots/DependencyInjectionSetup.png" width="700"/>

*The `UserController` class is updated to accept an `IUserManager` instance through constructor injection, allowing for dependency injection of different user manager classes.*

### 2. Setting Up `UserDAO` Injection in `Program.cs`
<img src="../Activity4Screenshots/Part1Screenshots/UserDAOInjection.png" width="700"/>

*The initial setup in `Program.cs` injects `UserDAO` as the implementation of `IUserManager`, using `AddSingleton` to provide a single instance throughout the application.*

### 3. Testing with SQL-Backed Data Source
<img src="../Activity4Screenshots/Part1Screenshots/SQLLoginSuccess.png" width="700"/>

*The application initially uses `UserDAO`, allowing successful login for users stored in the SQL database.*

### 4. Switching to `UserCollection` for In-Memory Data
<img src="../Activity4Screenshots/Part1Screenshots/UserCollectionInjection.png" width="700"/>

*The dependency injection configuration in `Program.cs` is updated to inject `UserCollection` as the `IUserManager` implementation, switching the data source to a hard-coded collection of users.*

### 5. Testing with In-Memory Data Source
<img src="../Activity4Screenshots/Part1Screenshots/InMemoryLoginSuccess.png" width="700"/>

*With `UserCollection` injected, the application uses hard-coded data (e.g., "Harry" and "Megan"), demonstrating the flexibility of dependency injection by switching the data source without modifying `UserController`.*

### 6. Reverting Back to SQL Data Source
<img src="../Activity4Screenshots/Part1Screenshots/UserDAOReinjection.png" width="700"/>

*The dependency injection is set back to `UserDAO` to use the SQL database once again, showing how easily the data source can be switched through configuration.*

---

## Summary of Key Concepts (Part 1)
In Part 1 of Activity 4, I implemented dependency injection in the `UserController` class, allowing for flexible selection between different data sources (`UserDAO` for SQL and `UserCollection` for in-memory data). By configuring `Program.cs` to inject `IUserManager` with either `UserDAO` or `UserCollection`, I demonstrated the use of dependency injection to decouple the controller from the specific data implementation. This process enabled easy switching between data sources and provided a practical understanding of dependency injection, enhancing the modularity and testability of the application.

---

## Part 2: Data Validation and Form Enhancements

- **This section demonstrates the addition of validation rules, new address fields, and custom display names in the `AppointmentModel` to meet specified requirements.**

### 1. Initial Validation Errors for Empty Fields
<img src="../Activity4Screenshots/Part2Screenshots/AppointmentForm_ValidationErrors.png" width="700"/>

*The form displays initial validation errors for required fields, ensuring that no field is left blank.*

### 2. Custom Labels and Validation Errors
<img src="../Activity4Screenshots/Part2Screenshots/AppointmentForm_CustomLabels_ValidationErrors.png" width="700"/>

*The appointment form shows customized labels for each field and displays validation error messages when fields are left empty.*

### 3. Additional Validation Rules
<img src="../Activity4Screenshots/Part2Screenshots/AppointmentForm_AdditionalValidationRules.png" width="700"/>

*This screenshot shows the form with additional validation rules for net worth and pain level. Errors are shown if the net worth is below $90,000 or the pain level is 5 or below, in line with the business requirements.*

### 4. Successful Submission Displaying Appointment Details
<img src="../Activity4Screenshots/Part2Screenshots/AppointmentDetails_SubmissionSuccess.png" width="700"/>

*The final appointment details are displayed, showing a successful submission with all required information and restrictions met.*

### 5. Validation Errors for Added Fields and Restrictions
<img src="../Activity4Screenshots/Part2Screenshots/FormValidationErrors_AddedFieldsAndRestrictions.png" width="700"/>

*This screenshot demonstrates validation errors for new fields (such as ZIP code, email, and phone number) and specific restrictions on net worth and pain level.*

### 6. Successful Submission with New Fields and Restrictions
<img src="../Activity4Screenshots/Part2Screenshots/AppointmentDetails_SuccessAddedFieldsAndRestrictions.png" width="700"/>

*After filling in all required fields correctly, the form successfully submits, and the appointment details are displayed, including the new address fields and validation-compliant values.*

---

## Summary of Key Concepts (Part 2)
In Part 2 of Activity 4, I implemented additional validation rules in the `AppointmentModel` to ensure data integrity and enforce business logic. Custom display names were added to enhance form readability, and new fields for address, email, and phone were included. The model was further enhanced to reject appointments if the patient's net worth is below $90,000 or if the pain level is 5 or lower, as per the specified requirements. This part of the activity provided experience in creating custom validation messages, expanding form fields, and applying business rules to data entry.

---

## Part 3: Button Grid Game Implementation with Success Message

- **This section demonstrates the creation of a button grid that changes state upon clicks. The goal is to have all buttons in the same color, at which point a success message is displayed.**

### 1. Setting up the Button Grid with Initial State
<img src="../Activity4Screenshots/Part3Screenshots/Button_Grid_Index_Page_Displaying_Button_Data.png" width="700"/>

*The initial setup of the button grid displays each button with its unique `Id`, `ButtonState`, and corresponding image based on its color.*

### 2. Displaying the Button Grid with Timestamp
<img src="../Activity4Screenshots/Part3Screenshots/ButtonGrid_View_Display_with_Timestamp.png" width="700"/>

*The grid view displays a live timestamp, providing a dynamic element to the page along with the clickable buttons.*

### 3. Applying Flexbox for Grid Layout
<img src="../Activity4Screenshots/Part3Screenshots/ButtonGrid_FlexLayout_StyledButtons.png" width="700"/>

*The button grid layout is enhanced using Flexbox, ensuring the buttons are aligned in rows. CSS styling is applied to improve visual consistency and spacing.*

### 4. Inspecting HTML Elements
<img src="../Activity4Screenshots/Part3Screenshots/ButtonGrid_HTML_Elements_Inspect.png" width="700"/>

*The HTML generated by the Razor view is inspected to confirm each button’s structure, attributes, and functionality.*

### 5. Game Success Message Display
<img src="../Activity4Screenshots/Part3Screenshots/GameSuccessMessage_Displayed.png" width="700"/>

*Once all buttons in the grid are set to the same color, a success message is displayed to the user, indicating that the game goal has been achieved.*

---

## Summary of Key Concepts (Part 3)
In Part 3, I created a button grid where each button changes state upon a click, cycling through a set of colors. Using Flexbox for layout ensured that the grid remained visually organized. Additionally, a success message is displayed when all buttons reach the same color, demonstrating simple game logic and enhancing user experience through visual feedback.

---
