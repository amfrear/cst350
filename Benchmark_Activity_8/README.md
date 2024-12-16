# Benchmark - Activity 8 Bible Verse Application

## Cover Sheet
**Student Name:** Alex Frear  
**Date:** 12/13/2024  
**Program:** College of Science, Engineering, and Technology, Grand Canyon University  
**Course:** CST-350 Programming in C# III  
**Instructor:** Brandon Bass  

---

## Design Documents
- **This section demonstrates the design and planning process, showcasing the UML diagram, wireframes, and ER diagram. Each element highlights the structure and intended functionality of the application.**

### 1. UML Diagram
The UML diagram illustrates the structure of the application, including classes, properties, and methods.

<img src="BibleSearchApp_UML.png" width="700"/>
The application structure is well-defined through this UML diagram.

### 2. Wireframes
The wireframes represent the user interface design for the application.

<img src="BibleSearchApp_Wireframe.png" width="700"/>
The user interface plan provides a clear vision of the layout and navigation.

### 3. ER Diagram
The ER diagram outlines the database structure, showing relationships between tables (`asv_books`, `asv_verses`, and `notes`).

<img src="BibleDb_EER.png" width="700"/>
The ER diagram defines database relationships clearly.

---

## Application Screenshots
- **This section provides screenshots of the application's functionality, with captions explaining each step of the user experience.**

### 1. Home Page (Initial View)
**Description:** Displays options for keyword-based and reference-based searches.

<img src="BibleSearchAppScreenshots/1-HomePageInitial-Browser.png" width="700"/>
The initial view offers intuitive navigation for users.

### 2. Home Page with Keyword Search Results
**Description:** Displays search results when a keyword is entered.

<img src="BibleSearchAppScreenshots/2-HomePageKeywordSearchResults-Browser.png" width="700"/>
Search results are displayed dynamically based on the keyword.

### 3. Home Page with Reference Search Results
**Description:** Displays results for a reference-based search by book and chapter.

<img src="BibleSearchAppScreenshots/3-HomePageReferenceSearchResults-Browser.png" width="700"/>
Reference-based searches return accurate chapter results.

### 4. Home Page with Combined Search Results
**Description:** Displays results for both keyword and reference-based searches together.

<img src="BibleSearchAppScreenshots/4-HomePageKeyword+ReferenceSearchResults-Browser.png" width="700"/>
Combined results enhance user search capabilities.

### 5. View Details Page (Initial View)
**Description:** Displays details for a selected Bible verse along with its associated notes.

<img src="BibleSearchAppScreenshots/5-ViewDetailsPageInitial-Browser.png" width="700"/>
Verse details are presented clearly for user review.

### 6. Adding a Note
**Description:** Allows a user to add a note for a specific verse.

<img src="BibleSearchAppScreenshots/6-ViewDetailsPageAddNote-Browser.png" width="700"/>
Users can annotate verses with personal notes.

### 7. Editing a Note
**Description:** Displays the edit note functionality.

<img src="BibleSearchAppScreenshots/7-ViewDetailsPageEditNote-Browser.png" width="700"/>
Users can easily edit existing notes.

### 8. Deleting a Note
**Description:** Demonstrates deleting an existing note.

<img src="BibleSearchAppScreenshots/8-ViewDetailsPageDeleteNote-Browser.png" width="700"/>
Note deletion functionality is intuitive and efficient.

### 9. Reference Search Results Page: Back to Chapter Button
**Description:** Highlights the "Back to Chapter" button functionality.

<img src="BibleSearchAppScreenshots/9-ReferenceSearchResultsPageAfterClickingBackToChapterButton-Browser.png" width="700"/>
Users can return to the chapter with a single click.

### 10. AJAX Functionality for Search Results
**Description:** Developer tools showing AJAX during a search query.

<img src="BibleSearchAppScreenshots/10-HomePageSearchResultsAjax-BrowserDevTools.png" width="700"/>
AJAX enables smooth, asynchronous search updates.

### 11. AJAX for Adding Notes
**Description:** AJAX functionality during note addition.

<img src="BibleSearchAppScreenshots/11-VerseDetailsPageAddNoteAjax-BroswerDevTools.png" width="700"/>
Notes are added dynamically without refreshing the page.

### 12. AJAX for Editing Notes
**Description:** AJAX functionality during note editing.

<img src="BibleSearchAppScreenshots/12-VerseDetailsPageEditNoteAjax-BroswerDevTools.png" width="700"/>
Edit updates are reflected instantly through AJAX.

### 13. AJAX for Deleting Notes
**Description:** AJAX functionality during note deletion.

<img src="BibleSearchAppScreenshots/13-VerseDetailsPageDeleteNoteAjax-BroswerDevTools.png" width="700"/>
Notes are removed seamlessly using AJAX.

---

## Summary of Key Concepts

### Application Design and Development
Developing the BibleSearchApp provided me with valuable experience in planning and structuring a software project. Creating UML diagrams, wireframes, and an ER diagram helped me approach the application systematically. These design documents allowed me to anticipate potential issues and stay organized throughout the process. I also deepened my understanding of the Model-View-Controller (MVC) architecture, which helped me cleanly separate concerns. By organizing the application into models, views, and controllers, I was able to focus on designing intuitive user interfaces, managing complex logic, and maintaining efficient data access.

### Database Design and Relationships
The BibleSearchApp required me to design and implement a relational database to store books, verses, and user notes. This was an excellent opportunity to enhance my skills in database design, particularly in modeling relationships and writing efficient queries. The experience of linking related data, such as connecting notes to specific verses, taught me how to structure databases for easy data retrieval and scalability.

### AJAX Integration and User Experience
One of the most rewarding aspects of this project was implementing AJAX for features like search results and note management. By using AJAX, I was able to create a more responsive and modern application that updates dynamically without full-page reloads. Integrating JavaScript with server-side ASP.NET Core controllers gave me a clearer understanding of how to handle asynchronous client-server communication effectively. This experience improved my ability to create applications that feel smooth and user-friendly.

### Real-World Relevance and Impact
The purpose of this project was particularly meaningful to me, as the BibleSearchApp is designed to address real-world needs. By providing features like keyword and reference searches and allowing users to annotate verses with personal notes, the app supports both personal Bible study and group discussions. Knowing that this tool could enhance accessibility and spiritual growth made the development process more fulfilling and gave me a deeper appreciation for the impact of well-designed software.

---
