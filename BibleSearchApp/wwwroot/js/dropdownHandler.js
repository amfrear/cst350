// wwwroot/js/dropdownHandler.js

function initDropdowns(testamentDropdownId, bookDropdownId, chapterDropdownId, booksDataId, chapterCountsDataId) {
    const testamentDropdown = document.getElementById(testamentDropdownId);
    const bookDropdown = document.getElementById(bookDropdownId);
    const chapterDropdown = document.getElementById(chapterDropdownId);
    const booksDataElement = document.getElementById(booksDataId);
    const chapterCountsDataElement = document.getElementById(chapterCountsDataId);

    if (!testamentDropdown || !bookDropdown || !chapterDropdown || !booksDataElement || !chapterCountsDataElement) {
        return;
    }

    const booksData = JSON.parse(booksDataElement.textContent);
    const chapterCounts = JSON.parse(chapterCountsDataElement.textContent);

    function filterBooks(testament) {
        const filteredBooks = testament === "All Testaments"
            ? booksData
            : booksData.filter(book => book.Testament === testament);
        bookDropdown.innerHTML = '<option value="">All Books</option>';

        filteredBooks.forEach(book => {
            const option = document.createElement('option');
            option.value = book.Id;
            option.textContent = book.Name;
            bookDropdown.appendChild(option);
        });
    }

    function updateChapters(bookId) {
        const chapterCount = bookId ? (chapterCounts[bookId] || 150) : 150;
        chapterDropdown.innerHTML = '<option value="">All Chapters</option>';

        for (let i = 1; i <= chapterCount; i++) {
            const option = document.createElement('option');
            option.value = i;
            option.textContent = i;
            chapterDropdown.appendChild(option);
        }
    }

    // Initial population
    filterBooks(testamentDropdown.value);
    updateChapters(bookDropdown.value);

    // Event listeners
    testamentDropdown.addEventListener('change', function () {
        const selectedTestament = this.value;
        filterBooks(selectedTestament);
        updateChapters(null);
    });

    bookDropdown.addEventListener('change', function () {
        const selectedBookId = this.value;
        updateChapters(selectedBookId);
    });
}

// Initialize both Keyword Search and Reference Search dropdowns
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
