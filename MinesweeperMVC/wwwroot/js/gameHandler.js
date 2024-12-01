$(document).ready(function () {
    console.log("gameHandler.js loaded and running");

    $(document).ready(function () {
        // Prevent the context menu on right-click for grid cells
        $(document).on('contextmenu', '.grid-cell', function (event) {
            event.preventDefault(); // Disable the default context menu
            console.log("Context menu prevented.");
        });

        // Handle right-click (toggle flag)
        $(document).on('mousedown', '.grid-cell', function (event) {
            if (event.which === 3) { // Right mouse button
                event.preventDefault(); // Prevent default action (redundant safeguard)
                console.log("Right-click detected, preventing default context menu.");

                var $button = $(this);
                var row = $button.data('row');
                var col = $button.data('col');

                console.log("Right-clicked on cell at row " + row + ", col " + col);

                // Send an AJAX request to toggle the flag
                $.ajax({
                    url: '/Game/ToggleFlag',
                    type: 'POST',
                    data: { row: row, col: col },
                    success: function (response) {
                        console.log("Flag toggled successfully");
                        $('#game-container').replaceWith(response);
                    },
                    error: function () {
                        console.error("Error toggling flag");
                        alert('Error toggling flag.');
                    }
                });

                return false; // Prevent propagation and default behavior
            }
        });
    });

    // Attach event listener for left-click (reveal cell)
    $(document).on('click', '.grid-cell', function (event) {
        event.preventDefault(); // Prevent default action

        var $button = $(this);
        var row = $button.data('row');
        var col = $button.data('col');

        console.log("Left-clicked on cell at row " + row + ", col " + col);

        // Send an AJAX request to reveal the cell
        $.ajax({
            url: '/Game/RevealCell',
            type: 'POST',
            data: { row: row, col: col },
            success: function (response) {
                console.log("Cell revealed successfully");
                $('#game-container').replaceWith(response);
            },
            error: function () {
                console.error("Error revealing cell");
                alert('Error revealing cell.');
            }
        });
    });
});
