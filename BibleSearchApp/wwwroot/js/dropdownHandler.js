document.addEventListener('DOMContentLoaded', function () {
    const testamentDropdown = document.getElementById('testamentDropdown'); // Testament dropdown element
    const bookDropdown = document.getElementById('bookDropdown'); // Book dropdown element
    const chapterDropdown = document.getElementById('chapterDropdown'); // Chapter dropdown element

    // Parse books data passed from the server
    const booksData = JSON.parse(document.getElementById('booksData').textContent);

    // Parse chapter counts data passed from the server
    const chapterCounts = JSON.parse(document.getElementById('chapterCountsData').textContent);

    // Function to filter books based on testament
    function filterBooks(testament) {
        const filteredBooks = booksData.filter(book => testament === "All Testaments" || book.Testament === testament);
        bookDropdown.innerHTML = '<option value="">All Books</option>'; // Reset dropdown

        // Populate book dropdown with filtered books
        filteredBooks.forEach(book => {
            const option = document.createElement('option');
            option.value = book.Id;
            option.textContent = book.Name;
            option.setAttribute('data-testament', book.Testament);
            bookDropdown.appendChild(option);
        });
    }

    // Function to update chapter dropdown
    function updateChapters(bookId) {
        const chapterCount = chapterCounts[bookId] || 150; // Default to 150 if no book selected
        chapterDropdown.innerHTML = '<option value="">All Chapters</option>'; // Reset chapters

        // Populate chapter dropdown with the correct number of chapters
        for (let i = 1; i <= chapterCount; i++) {
            const option = document.createElement('option');
            option.value = i;
            option.textContent = i;
            chapterDropdown.appendChild(option);
        }
    }

    // Listen for changes in the testament dropdown
    testamentDropdown.addEventListener('change', function () {
        const selectedTestament = this.value; // Get the selected testament
        filterBooks(selectedTestament);
    });

    // Listen for changes in the book dropdown
    bookDropdown.addEventListener('change', function () {
        const selectedBookId = this.value; // Get the selected book ID
        updateChapters(selectedBookId);
    });
});
