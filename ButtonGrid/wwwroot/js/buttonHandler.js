$(document).on('click', '.game-button button', function (event) {
    event.preventDefault(); // Prevent form submission

    var buttonId = $(this).val(); // Get the button ID from the clicked button's value

    // Add an alert to indicate the event is being handled
    alert(`The button click event is being handled by AJAX instead of the form submit. You clicked item ${buttonId}.`);

    $.ajax({
        url: '/Button/PartialPageUpdate', // Call the PartialPageUpdate method
        data: { id: buttonId }, // Pass the button ID as a parameter
        success: function (result) {
            // Add an alert to show the HTML returned from the server
            alert(`HTML returned from server:\n${result}`);

            // Replace the HTML of the button with the updated partial view
            $(`.game-button[data-id="${buttonId}"]`).html($(result).html());
        },
        error: function () {
            alert('Failed to update the button. Please try again.');
        }
    });
});
