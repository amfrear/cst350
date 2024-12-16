/*!
 * BibleSearchApp
 * 
 * File: notesHandler.js
 * Description: Manages the addition, deletion, and editing of notes associated with Bible verses.
 *              Handles form submissions and button actions via AJAX to interact with the server without reloading the page.
 * Author: Alex Frear
 * Created: 2024-04-27
 * License: MIT License
 */

/**
 * Initializes event listeners for managing notes, including adding, deleting, and editing notes.
 * This function sets up AJAX-based interactions to handle notes without requiring full page reloads.
 *
 * @returns {void}
 */
document.addEventListener('DOMContentLoaded', function () {
    // Select essential DOM elements by their IDs
    const notesContainer = document.getElementById('notes-container');
    const addNoteForm = document.getElementById('add-note-form');
    const saveEditBtn = document.getElementById('save-edit-btn');

    /**
     * Handles the submission of the Add Note form via AJAX.
     * Sends the new note data to the server and updates the notes container with the response.
     *
     * @param {Event} event - The form submission event.
     * @returns {void}
     */
    if (addNoteForm) {
        addNoteForm.addEventListener('submit', function (event) {
            event.preventDefault(); // Prevent the default form submission behavior

            // Extract form data
            const formData = new FormData(addNoteForm);
            const verseId = formData.get('verseId');
            const content = formData.get('content');

            // Send AJAX POST request to add the new note
            fetch('/Search/AddNoteAjax', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ verseId: parseInt(verseId), content })
            })
                .then(response => response.text()) // Parse the response as text (HTML)
                .then(html => {
                    if (notesContainer) {
                        notesContainer.innerHTML = html; // Update the notes container with the new HTML
                    }
                    addNoteForm.reset(); // Clear the form fields
                })
                .catch(error => {
                    console.error('Error adding note:', error); // Log any errors to the console
                });
        });
    }

    /**
     * Handles click events within the notes container for deleting and editing notes.
     * Utilizes event delegation to manage dynamically added buttons.
     *
     * @param {Event} event - The click event.
     * @returns {void}
     */
    if (notesContainer) {
        notesContainer.addEventListener('click', function (event) {
            // Handle Delete Note button clicks
            if (event.target.classList.contains('delete-note-btn')) {
                const noteId = event.target.getAttribute('data-note-id');

                // Send AJAX POST request to delete the note
                fetch('/Search/DeleteNoteAjax', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ noteId: parseInt(noteId) })
                })
                    .then(response => {
                        if (!response.ok) throw new Error('Failed to delete note.');
                        return response.text(); // Parse the response as text (HTML)
                    })
                    .then(html => {
                        notesContainer.innerHTML = html; // Update the notes container with the new HTML
                    })
                    .catch(error => {
                        console.error('Error deleting note:', error); // Log any errors to the console
                    });
            }

            // Handle Edit Note button clicks
            if (event.target.classList.contains('edit-note-btn')) {
                const noteId = event.target.getAttribute('data-note-id');
                const noteContent = event.target.getAttribute('data-note-content');

                // Populate the edit modal with the existing note data
                document.getElementById('edit-note-id').value = noteId;
                document.getElementById('edit-note-content').value = noteContent;

                // Initialize and show the Bootstrap modal for editing notes
                const editModal = new bootstrap.Modal(document.getElementById('editNoteModal'), {});
                editModal.show();
            }
        });
    }

    /**
     * Handles the Save Changes button click within the Edit Note modal.
     * Sends the updated note data to the server and updates the notes container with the response.
     *
     * @returns {void}
     */
    if (saveEditBtn) {
        saveEditBtn.addEventListener('click', function () {
            // Retrieve the edited note ID and new content from the modal inputs
            const noteIdElem = document.getElementById('edit-note-id');
            const noteContentElem = document.getElementById('edit-note-content');

            // Safety check to ensure modal elements exist
            if (!noteIdElem || !noteContentElem) return;

            const noteId = noteIdElem.value;
            const newContent = noteContentElem.value;

            // Send AJAX POST request to update the note
            fetch('/Search/EditNoteAjax', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ noteId: parseInt(noteId), content: newContent })
            })
                .then(response => {
                    if (!response.ok) throw new Error('Failed to edit note.');
                    return response.text(); // Parse the response as text (HTML)
                })
                .then(html => {
                    if (notesContainer) {
                        notesContainer.innerHTML = html; // Update the notes container with the new HTML
                    }

                    // Hide the edit modal after a successful update
                    const editModalEl = document.getElementById('editNoteModal');
                    const modalInstance = bootstrap.Modal.getInstance(editModalEl);
                    modalInstance.hide();
                })
                .catch(error => {
                    console.error('Error editing note:', error); // Log any errors to the console
                });
        });
    }
});
