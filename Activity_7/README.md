# CST-350 Activity 7 - REST API and Right Click Events

## Cover Sheet
**Student Name:** Alex Frear  
**Date:** 12/04/2024  
**Program:** College of Science, Engineering, and Technology, Grand Canyon University  
**Course:** CST-350 Programming in C# III  
**Instructor:** Brandon Bass  

---

## Part 1: RESTful Services and CRUD Operations
- **This section demonstrates the implementation and testing of RESTful endpoints for CRUD operations in the `ProductsApp`. Each step highlights the setup, execution, and verification of different API endpoints using both the browser and Postman.**

### 1. Testing `ShowAllProducts` Endpoint
The `ShowAllProducts` endpoint is tested in the browser to confirm it returns a `200 OK` status with all products.

<img src="Activity7Screenshots/ShowAllProducts_Status200_BrowserEndpoint.png" width="700"/>

*The browser displays all products successfully.*  

### 2. Testing `GetProductById` Endpoint
The `GetProductById` endpoint is tested in the browser and Postman to retrieve a specific product by its ID.

<img src="Activity7Screenshots/GetProductById_Status200_BrowserEndpoint.png" width="700"/>
*Browser displays a product retrieved successfully.*  

<img src="Activity7Screenshots/Postman_Get_GetProductById_Endpoint.png" width="700"/>
*Postman confirms the endpoint returns the product details correctly.*  

### 3. Testing `SearchForProducts` Endpoint
The `SearchForProducts` endpoint is tested in the browser for both successful searches and error handling when parameters are missing.

<img src="Activity7Screenshots/SearchForProducts_Status200_BrowserEndpoint.png" width="700"/>
*Browser displays search results successfully.*  

<img src="Activity7Screenshots/SearchForProducts_NoParamsError400_BrowserEndpoint.png" width="700"/>
*Browser displays a `400 Bad Request` error when required parameters are missing.*  

<img src="Activity7Screenshots/Postman_Get_SearchForProducts_Endpoint.png" width="700"/>
*Postman confirms successful search results.*  

<img src="Activity7Screenshots/Postman_Get_SearchForProducts_NoParamsError_Endpoint.png" width="700"/>
*Postman displays an error message when no parameters are provided.*  

### 4. Testing `CreateProduct` Endpoint
A new product is created using Postman, and its successful addition is verified by fetching all products.

<img src="Activity7Screenshots/Postman_CreateProduct_Status201.png" width="700"/>
*Postman confirms the product was created successfully.*  

<img src="Activity7Screenshots/Postman_CreateProductVerified_GetShowAllProducts.png" width="700"/>
*Postman verifies the new product is included in the list of all products.*  

### 5. Testing `UpdateProduct` Endpoint
An existing product is updated using Postman, and the changes are verified by fetching the updated product details.

<img src="Activity7Screenshots/Postman_PutUpdateProduct_Status204.png" width="700"/>
*Postman confirms the product update was successful.*  

<img src="Activity7Screenshots/Postman_UpdateProductVerified_GetGetProductById.png" width="700"/>
*Postman verifies the product’s details reflect the update.*  

### 6. Testing `DeleteProduct` Endpoint
A product is deleted using Postman, and its removal is confirmed by fetching all products.

<img src="Activity7Screenshots/Postman_DeleteProduct_Status204.png" width="700"/>
*Postman confirms the product was deleted successfully.*  

---

## Summary of Key Concepts (Part 1)
In this part of Activity 7, I developed and tested RESTful endpoints for CRUD operations in the `ProductsApp`. Each endpoint was verified using both browser-based tools and Postman. The activity reinforced my understanding of building and testing API endpoints and handling HTTP status codes for successful and error scenarios. This hands-on experience strengthened my skills in creating reliable and well-documented RESTful services for modern web applications.

---

## Part 2: Right Click Event and Button Updates
- **This section demonstrates the implementation of left-click and right-click functionality for the buttons in the `ButtonGrid` application. AJAX-based methods are used to dynamically update the button state and image without refreshing the page.**

### 1. Left-Click Functionality
The left-click functionality updates the button's state by incrementing it and cycling through different images.

#### Initial Button State
<img src="Activity7Screenshots/LeftClick_InitialButtonState.png" width="700"/>
*The button displays its initial state and image.*

#### Left-Click Event Triggered
<img src="Activity7Screenshots/LeftClick_EventTriggered.png" width="700"/>
*An alert confirms the left-click event was triggered.*

#### Left-Click Function Executed
<img src="Activity7Screenshots/LeftClick_DoLeftClickTriggered.png" width="700"/>
*The `doLeftClick` function is executed, sending an AJAX request to the server.*

#### HTML Returned from the Server
<img src="Activity7Screenshots/LeftClick_HTMLReturned.png" width="700"/>
*An alert displays the updated HTML returned by the server.*

#### Updated Button State
<img src="Activity7Screenshots/LeftClick_UpdatedButtonState.png" width="700"/>
*The button reflects the new state and updated image after the left-click.*

---

### 2. Right-Click Functionality
The right-click functionality updates the button's state by decrementing it and cycling through different images.

#### Initial Button State
<img src="Activity7Screenshots/RightClick_InitialButtonState.png" width="700"/>
*The button displays its initial state and image.*

#### Right-Click Event Triggered
<img src="Activity7Screenshots/RightClick_EventTriggered.png" width="700"/>
*An alert confirms the right-click event was triggered.*

#### Right-Click Function Executed
<img src="Activity7Screenshots/RightClick_DoRightClickTriggered.png" width="700"/>
*The `doRightClick` function is executed, sending an AJAX request to the server.*

#### HTML Returned from the Server
<img src="Activity7Screenshots/RightClick_HTMLReturned.png" width="700"/>
*An alert displays the updated HTML returned by the server.*

#### Updated Button State
<img src="Activity7Screenshots/RightClick_UpdatedButtonState.png" width="700"/>
*The button reflects the new state and updated image after the right-click.*

---

## Summary of Key Concepts (Part 2)
In this part of Activity 7, I implemented left-click and right-click functionality for dynamically updating button states in the `ButtonGrid` application. Using JavaScript and AJAX, I was able to handle mouse events and update the button state and image efficiently without a full-page reload. This exercise reinforced my understanding of client-server interactions and AJAX-based updates for responsive web applications.

---
