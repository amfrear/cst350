# CST-350 Activity 1 - First Web Application

## Cover Sheet
**Student Name:** Alex Frear  
**Date:** 10/22/2024  
**Program** College of Science, Engineering, and Technology, Grand Canyon University  
**Course:** CST-350 Programming in C# III  
**Instructor:** Brandon Bass  

## Part 1: Tools Installation and Default App

- **Below are screenshots demonstrating the different steps and pages created as part of Activity 1, Part 1 - First Web Application in ASP.NET Core.**

### 1. Project Structure
<img src="Activity_1/Activity1Screenshots/Part1Screenshots/01_Project_Structure.png" width="700"/>

*This screenshot shows the project structure of the ASP.NET Core web application in Visual Studio, highlighting the different folders and files used in this project.*

### 2. Default Application View
<img src="Activity_1/Activity1Screenshots/Part1Screenshots/02_Default_Application.png" width="700"/>

*The initial view of the ASP.NET Core web application before any modifications were made, showing the home page with the default template.*

### 3. Modified Privacy Page
<img src="Activity_1/Activity1Screenshots/Part1Screenshots/03_Modified_Privacy.png" width="700"/>

*The customized Privacy page created during this activity, demonstrating changes made to the page content.*

### 4. ViewData Added to Privacy Page
<img src="Activity_1/Activity1Screenshots/Part1Screenshots/04_ViewData_Privacy.png" width="700"/>

*This screenshot shows the added custom message from the `HomeController` to the Privacy page using `ViewData`.*

### 5. About Me Page
<img src="Activity_1/Activity1Screenshots/Part1Screenshots/05_AboutMe_Page.png" width="700"/>

*A new "About Me" page was created and added to the web application, showcasing basic personal information.*

### 6. About Me Link Added to Navbar
<img src="Activity_1/Activity1Screenshots/Part1Screenshots/06_Navbar_AboutMe_Link.png" width="700"/>

*A new link was added to the navigation bar for the "About Me" page, allowing easy access to the newly created page.*

### 7. Projects Page
<img src="Activity_1/Activity1Screenshots/Part1Screenshots/07_NewPages_Projects.png" width="700"/>

*The Projects page demonstrates additional content created to practice extending the web application.*

### 8. Contact Me Page
<img src="Activity_1/Activity1Screenshots/Part1Screenshots/08_NewPages_ContactMe.png" width="700"/>

*The Contact Me page showcases another extension of the web application to demonstrate basic user interaction capabilities.*

### Summary of Key Concepts (Part 1)
In Part 1 of this activity, we explored creating a simple ASP.NET Core web application using Visual Studio. Key concepts covered include setting up a project structure, modifying views, adding new pages to the application, and integrating `ViewData` to dynamically pass data from the controller to the view. We also learned how to extend navigation by adding new links to the navbar, demonstrating basic routing and controller action usage in an MVC framework.

---
<br>

## Part 2: Controllers and Views

- **Below are screenshots demonstrating the different steps and pages created as part of Activity 1, Part 2 - Extending the ASP.NET Core Web Application.**

### 9. Products Index Page
<img src="Activity_1/Activity1Screenshots/Part2Screenshots/09_Products_Index_Page.png" width="700"/>

*This screenshot shows the initial Products page in the web application. This view was created by adding a new controller named `ProductsController` and generating the "Index" view for it.*

### 10. Products Message Output
<img src="Activity_1/Activity1Screenshots/Part2Screenshots/10_Products_Message_Output.png" width="700"/>

*This screenshot displays the output of the `Message` method within the `ProductsController`. The method was modified to return a simple message string that is displayed directly in the browser.*

### 11. JSON Output from ProductsController
<img src="Activity_1/Activity1Screenshots/Part2Screenshots/JSON_Output_ProductsController.png" width="700"/>

*This screenshot shows the JSON data generated from the `Data` method in the `ProductsController`. The JSON includes properties for `orderNumber`, `price`, and `quantity`, demonstrating how to return structured data directly to the browser.*

### 12. Details Page with Parameters
<img src="Activity_1/Activity1Screenshots/Part2Screenshots/Products_Details_With_Params_Browser.png" width="700"/>

*This screenshot shows the "Details" page of the ProductsController, demonstrating how passing parameters via the URL can affect the view. Here, the name and personality parameters are passed to display a personalized message with a personality rating.*

### 13. Message View with Razor
<img src="Activity_1/Activity1Screenshots/Part2Screenshots/Products_Message_View_Browser.png" width="700"/>

*This screenshot shows the output of the "Message" view, which was created to demonstrate the use of Razor views in ASP.NET Core. The view includes a sample message with basic HTML formatting.*

### Summary of Key Concepts (Part 2)
In Part 2 of this activity, we extended the web application by adding new functionality through controllers, views, and JSON data. We began by creating a new controller called `ProductsController` and generating views for different actions. We explored the use of parameters to pass information from the URL to the controller, which allowed us to dynamically adjust the view based on the inputs. Additionally, we demonstrated how to return JSON data from a controller method, showcasing the ability to serve data for use in APIs or other client applications. We also added and modified Razor views to enhance the user interface, incorporating basic HTML and CSS tags to create a more engaging experience for users. Through these exercises, we demonstrated the power of ASP.NET Core MVC to create interactive and data-driven web applications using routing, controllers, views, and JSON to build a flexible and robust web solution.
