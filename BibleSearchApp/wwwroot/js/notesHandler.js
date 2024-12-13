document.addEventListener('DOMContentLoaded', function () {
    const notesContainer = document.getElementById('notes-container');
    const addNoteForm = document.getElementById('add-note-form');
    const saveEditBtn = document.getElementById('save-edit-btn');

    // Handle Add Note submission via AJAX only if the form exists
    if (addNoteForm) {
        addNoteForm.addEventListener('submit', function (event) {
            event.preventDefault();
            const formData = new FormData(addNoteForm);
            const verseId = formData.get('verseId');
            const content = formData.get('content');

            fetch('/Search/AddNoteAjax', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ verseId: parseInt(verseId), content })
            })
                .then(response => response.text())
                .then(html => {
                    if (notesContainer) {
                        notesContainer.innerHTML = html;
                    }
                    addNoteForm.reset(); // Clear the form
                })
                .catch(console.error);
        });
    }

    // Event delegation for delete and edit buttons only if notesContainer exists
    if (notesContainer) {
        notesContainer.addEventListener('click', function (event) {
            if (event.target.classList.contains('delete-note-btn')) {
                const noteId = event.target.getAttribute('data-note-id');
                fetch('/Search/DeleteNoteAjax', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ noteId: parseInt(noteId) })
                })
                    .then(response => {
                        if (!response.ok) throw new Error('Failed to delete note.');
                        return response.text();
                    })
                    .then(html => {
                        notesContainer.innerHTML = html;
                    })
                    .catch(console.error);
            }

            if (event.target.classList.contains('edit-note-btn')) {
                const noteId = event.target.getAttribute('data-note-id');
                const noteContent = event.target.getAttribute('data-note-content');

                // Populate the modal fields
                document.getElementById('edit-note-id').value = noteId;
                document.getElementById('edit-note-content').value = noteContent;

                // Show the modal
                const editModal = new bootstrap.Modal(document.getElementById('editNoteModal'), {});
                editModal.show();
            }
        });
    }

    // Handle Edit Modal "Save Changes" button click only if it exists
    if (saveEditBtn) {
        saveEditBtn.addEventListener('click', function () {
            const noteIdElem = document.getElementById('edit-note-id');
            const noteContentElem = document.getElementById('edit-note-content');

            if (!noteIdElem || !noteContentElem) return; // Safety check

            const noteId = noteIdElem.value;
            const newContent = noteContentElem.value;

            fetch('/Search/EditNoteAjax', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ noteId: parseInt(noteId), content: newContent })
            })
                .then(response => {
                    if (!response.ok) throw new Error('Failed to edit note.');
                    return response.text();
                })
                .then(html => {
                    if (notesContainer) {
                        notesContainer.innerHTML = html;
                    }
                    // Hide the modal after successful update
                    const editModalEl = document.getElementById('editNoteModal');
                    const modalInstance = bootstrap.Modal.getInstance(editModalEl);
                    modalInstance.hide();
                })
                .catch(console.error);
        });
    }
});
