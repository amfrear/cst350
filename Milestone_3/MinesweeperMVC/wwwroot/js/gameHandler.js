// Ensure the DOM is fully loaded before executing the script
$(document).ready(function () {
    console.log("gameHandler.js loaded and running");

    // Prevent the default context menu from appearing on right-click
    $(document).on('contextmenu', '.grid-cell', function (event) {
        event.preventDefault(); // Disable the browser's default context menu
        console.log("Context menu prevented."); // Log confirmation that the context menu was disabled
    });

    // Handle right-click events on grid cells (used to toggle flags)
    $(document).on('mousedown', '.grid-cell', function (event) {
        if (event.which === 3) { // Check if the right mouse button was clicked
            event.preventDefault(); // Safeguard to prevent default browser behavior
            console.log("Right-click detected, preventing default context menu."); // Log right-click detection

            // Get the data attributes for the clicked cell (row and column)
            var $button = $(this);
            var row = $button.data('row'); // Extract row number from data attribute
            var col = $button.data('col'); // Extract column number from data attribute

            console.log("Right-clicked on cell at row " + row + ", col " + col); // Log the cell coordinates

            // Send an AJAX POST request to toggle the flag on the server
            $.ajax({
                url: '/Game/ToggleFlag', // URL for the server-side flag toggle handler
                type: 'POST', // HTTP method
                data: { row: row, col: col }, // Data sent to the server (cell coordinates)
                success: function (response) {
                    console.log("Flag toggled successfully"); // Log success message
                    $('#game-container').replaceWith(response); // Replace the game container with the updated content
                },
                error: function () {
                    console.error("Error toggling flag"); // Log error if the request fails
                    alert('Error toggling flag.'); // Show an error alert to the user
                }
            });

            return false; // Prevent event propagation and any default behavior
        }
    });

    // Handle left-click events on grid cells (used to reveal cells)
    $(document).on('click', '.grid-cell', function (event) {
        event.preventDefault(); // Prevent any default behavior associated with the click

        // Get the data attributes for the clicked cell (row and column)
        var $button = $(this);
        var row = $button.data('row'); // Extract row number from data attribute
        var col = $button.data('col'); // Extract column number from data attribute

        console.log("Left-clicked on cell at row " + row + ", col " + col); // Log the cell coordinates

        // Send an AJAX POST request to reveal the cell on the server
        $.ajax({
            url: '/Game/RevealCell', // URL for the server-side cell reveal handler
            type: 'POST', // HTTP method
            data: { row: row, col: col }, // Data sent to the server (cell coordinates)
            success: function (response) {
                console.log("Cell revealed successfully"); // Log success message
                $('#game-container').replaceWith(response); // Replace the game container with the updated content
            },
            error: function () {
                console.error("Error revealing cell"); // Log error if the request fails
                alert('Error revealing cell.'); // Show an error alert to the user
            }
        });
    });
});
