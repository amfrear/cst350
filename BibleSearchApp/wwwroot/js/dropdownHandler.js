/*!
 * BibleSearchApp
 * 
 * File: dropdownHandler.js
 * Description: Handles the initialization and management of dropdown menus for testaments, books, and chapters.
 *              This includes filtering books based on the selected testament and updating chapter counts accordingly.
 * Author: Alex Frear
 * Created: 2024-04-27
 * License: MIT License
 */

/**
 * Initializes dropdown menus for testaments, books, and chapters.
 *
 * @param {string} testamentDropdownId - The DOM element ID for the testament dropdown.
 * @param {string} bookDropdownId - The DOM element ID for the book dropdown.
 * @param {string} chapterDropdownId - The DOM element ID for the chapter dropdown.
 * @param {string} booksDataId - The DOM element ID containing the books data in JSON format.
 * @param {string} chapterCountsDataId - The DOM element ID containing the chapter counts data in JSON format.
 * @returns {void}
 */
function initDropdowns(testamentDropdownId, bookDropdownId, chapterDropdownId, booksDataId, chapterCountsDataId) {
    const testamentDropdown = document.getElementById(testamentDropdownId);
    const bookDropdown = document.getElementById(bookDropdownId);
    const chapterDropdown = document.getElementById(chapterDropdownId);
    const booksDataElement = document.getElementById(booksDataId);
    const chapterCountsDataElement = document.getElementById(chapterCountsDataId);

    // Ensure all necessary elements are present in the DOM
    if (!testamentDropdown || !bookDropdown || !chapterDropdown || !booksDataElement || !chapterCountsDataElement) {
        console.warn('One or more dropdown elements or data elements are missing.');
        return;
    }

    // Parse the JSON data from the DOM elements
    const booksData = JSON.parse(booksDataElement.textContent);
    const chapterCounts = JSON.parse(chapterCountsDataElement.textContent);

    /**
     * Filters the books dropdown based on the selected testament.
     *
     * @param {string} testament - The selected testament to filter books by.
     * @returns {void}
     */
    function filterBooks(testament) {
        // Determine which books to display based on the selected testament
        const filteredBooks = testament === "All Testaments"
            ? booksData
            : booksData.filter(book => book.Testament === testament);

        // Reset the books dropdown with a default option
        bookDropdown.innerHTML = '<option value="">All Books</option>';

        // Populate the books dropdown with the filtered books
        filteredBooks.forEach(book => {
            const option = document.createElement('option');
            option.value = book.Id;
            option.textContent = book.Name;
            bookDropdown.appendChild(option);
        });
    }

    /**
     * Updates the chapters dropdown based on the selected book.
     *
     * @param {string|null} bookId - The ID of the selected book. If null, defaults to 150 chapters.
     * @returns {void}
     */
    function updateChapters(bookId) {
        // Determine the number of chapters for the selected book
        const chapterCount = bookId ? (chapterCounts[bookId] || 150) : 150;

        // Reset the chapters dropdown with a default option
        chapterDropdown.innerHTML = '<option value="">All Chapters</option>';

        // Populate the chapters dropdown with the appropriate number of chapters
        for (let i = 1; i <= chapterCount; i++) {
            const option = document.createElement('option');
            option.value = i;
            option.textContent = i;
            chapterDropdown.appendChild(option);
        }
    }

    // Initial population of books and chapters dropdowns based on default selections
    filterBooks(testamentDropdown.value);
    updateChapters(bookDropdown.value);

    // Event listener for changes in the testament dropdown
    testamentDropdown.addEventListener('change', function () {
        const selectedTestament = this.value;
        filterBooks(selectedTestament);
        updateChapters(null); // Reset chapters when testament changes
    });

    // Event listener for changes in the book dropdown
    bookDropdown.addEventListener('change', function () {
        const selectedBookId = this.value;
        updateChapters(selectedBookId);
    });
}

// Initialize both Keyword Search and Reference Search dropdowns once the DOM is fully loaded
document.addEventListener('DOMContentLoaded', function () {
    // Initialize Keyword Search dropdowns
    initDropdowns(
        'testamentDropdown',
        'bookDropdown',
        'chapterDropdown',
        'booksData',
        'chapterCountsData'
    );

    // Initialize Reference Search dropdowns
    initDropdowns(
        'refTestamentDropdown',
        'refBookDropdown',
        'refChapterDropdown',
        'refBooksData',
        'refChapterCountsData'
    );
});
