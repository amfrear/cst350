
# CST-350 Activity 5 - Full-Stack CRUD Application with N-Layer Design

## Cover Sheet
**Student Name:** Alex Frear  
**Date:** 11/20/2024  
**Program:** College of Science, Engineering, and Technology, Grand Canyon University  
**Course:** CST-350 Programming in C# III  
**Instructor:** Brandon Bass  

---

## Overview
This activity focuses on building a full-stack CRUD application using the n-layer design approach with ASP.NET MVC. The application includes features such as product creation, updates, deletion, searching, and dynamic configuration using appsettings.json. Below are the detailed steps, implementation screenshots, and key learnings from this activity.

---

## Part 1: Setting up the Database and Application

### 1. Creating the Products Database
<img src="Activity5Screenshots/ProductsDatabaseCreation.png" width="700"/>

*The database schema is set up to store product data, including fields for Name, Price, Description, CreatedAt, and ImageURL.*

### 2. Products Table Schema
<img src="Activity5Screenshots/ProductsTableSchema.png" width="700"/>

*The table schema shows the structure of the Products table with appropriate data types and constraints.*

### 3. ProductModel Class Code
<img src="Activity5Screenshots/ProductModelClassCode.png" width="700"/>

*The `ProductModel` class represents the data structure for products in the application.*

### 4. Initial Application Default Page
<img src="Activity5Screenshots/ProductsApp_DefaultPage.png" width="700"/>

*The default landing page of the application after initial setup.*

---

## Part 2: Implementing Product Creation

### 1. Homepage View
<img src="Activity5Screenshots/HomePage.png" width="700"/>

*The homepage displays options to navigate to product-related functionalities.*

### 2. Create Product Form (Empty)
<img src="Activity5Screenshots/CreateProductFormEmpty.png" width="700"/>

*The empty product creation form is presented to the user.*

### 3. Filled Create Product Form
<img src="Activity5Screenshots/CreateProductFormFilled.png" width="700"/>

*The form is filled with product details before submission.*

### 4. Product Table Updated with New Entry
<img src="Activity5Screenshots/ProductTableWithNewEntry.png" width="700"/>

*After submitting the product creation form, the new product is added to the database and displayed in the product table.*

### 5. Initial View of All Products Page
<img src="Activity5Screenshots/InitialViewAllProductsPage.png" width="700"/>

*The "View All Products" page displays the list of all products, including the newly added entry.*

---

## Part 3: Updating Products with Dynamic Tax Calculation

### 1. Dynamic Tax Update in Create Product Form
<img src="Activity5Screenshots/DynamicTaxUpdateCreateProductForm.png" width="700"/>

*The tax value is dynamically updated based on the product price entered in the form.*

### 2. Dynamic Tax Update in View All Products Page
<img src="Activity5Screenshots/DynamicTaxUpdateViewAllProductsPage.png" width="700"/>

*The dynamically calculated tax is displayed alongside product details on the "View All Products" page.*

### 3. Selecting Existing Images for Products
<img src="Activity5Screenshots/CreateProductPageSelectImage.png" width="700"/>

*The product creation form allows users to select existing images from a dropdown menu.*

### 4. Viewing Products with Existing Images
<img src="Activity5Screenshots/ViewAllProductsWithExistingImages.png" width="700"/>

*Products with selected images are displayed in the product grid view.*

---

## Part 4: Editing and Deleting Products

### 1. Edit Product Page
<img src="Activity5Screenshots/EditPage.png" width="700"/>

*The edit form is pre-filled with existing product details for easy updates.*

### 2. Deleting a Product Confirmation
<img src="Activity5Screenshots/DeleteProductConfirmation.png" width="700"/>

*The user is prompted to confirm the deletion of a product.*

### 3. Product Deleted from All Products Page
<img src="Activity5Screenshots/ProductDeletedFromViewAllProductsPage.png" width="700"/>

*The product is successfully deleted and removed from the "View All Products" page.*

### 4. Confirmation of Product Deletion in Grid View
<img src="Activity5Screenshots/GridViewAllProductsDeletionConfirmation.png" width="700"/>

*The grid view also reflects the deletion of the product.*

---

## Part 5: Searching for Products

### 1. Search Input Page
<img src="Activity5Screenshots/SearchProductsInputPage.png" width="700"/>

*The search input form allows users to search for products by name or description.*

### 2. Search Results Page
<img src="Activity5Screenshots/SearchProductsResultsPage.png" width="700"/>

*The search results page displays products matching the entered search criteria.*

---

## Part 6: Viewing Product Details

### 1. Single Product Details Page
<img src="Activity5Screenshots/SingleProductDetailsPage.png" width="700"/>

*The "Product Details" page provides detailed information about a single product.*

---

## Part 7: Centralized Configuration with appsettings.json

### 1. Centralized Configuration Settings
<img src="Activity5Screenshots/AppSettingsCentralizedConfiguration.png" width="700"/>

*The `appsettings.json` file centralizes configuration settings such as database connection strings and tax rates, enabling dynamic updates without recompiling.*

---

## Summary of Key Concepts
Through Activity 5, I gained hands-on experience in implementing a full-stack CRUD application using the n-layer design approach. The activity demonstrated the importance of separation of concerns, dependency injection, dynamic configuration, and centralized settings management. This comprehensive project provided valuable insights into building scalable, maintainable, and dynamic web applications.

