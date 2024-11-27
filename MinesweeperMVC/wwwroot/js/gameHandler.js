$(document).ready(function () {
    console.log("gameHandler.js loaded and running");

    // Handle right-click for toggling flags
    $(document).on('contextmenu', '.grid-cell', function (event) {
        event.preventDefault(); // Prevent the default right-click menu

        var $button = $(this);
        var row = $button.data('row');
        var col = $button.data('col');

        // Send an AJAX request to toggle the flag
        $.ajax({
            url: '/Game/ToggleFlag',
            type: 'POST',
            data: { row: row, col: col },
            success: function (response) {
                // Replace the game container with the updated content
                $('#game-container').replaceWith(response);
            },
            error: function () {
                alert('Error toggling flag.');
            }
        });
    });
});
