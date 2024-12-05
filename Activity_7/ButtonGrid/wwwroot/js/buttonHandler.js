$(document).ready(function () {
    $(document).on("mousedown", ".game-button button", function (event) {
        event.preventDefault();
        var buttonId = $(this).val();
        switch (event.which) {
            case 1: // Left mouse button
                doLeftClick(buttonId);
                break;
            case 3: // Right mouse button
                doRightClick(buttonId);
                break;
            default:
                console.log(`Unhandled mouse button on button ID: ${buttonId}`);
        }
    });

    function doLeftClick(buttonId) {
        $.ajax({
            type: "POST",
            url: "/Button/PartialPageUpdate",
            data: { id: buttonId },
            success: function (result) {
                $(`.game-button[data-id="${buttonId}"]`).html($(result).html());
            },
            error: function (xhr, status, error) {
                console.error(`AJAX error for button ID: ${buttonId} - ${error}`);
            }
        });
    }

    function doRightClick(buttonId) {
        $.ajax({
            type: "POST",
            url: "/Button/RightClickShowOneButton",
            data: { id: buttonId },
            success: function (result) {
                $(`.game-button[data-id="${buttonId}"]`).html($(result).html());
            },
            error: function (xhr, status, error) {
                console.error(`AJAX error for button ID: ${buttonId} - ${error}`);
            }
        });
    }

    // Prevent default context menu on right-click
    $(document).on("contextmenu", ".game-button button", function (event) {
        event.preventDefault();
    });
});
