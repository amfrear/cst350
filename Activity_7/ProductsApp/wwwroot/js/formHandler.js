console.log("formHandler.js loaded and running");

// Event handler for the form submission
$(document).on("submit", "#editProductForm", function (event) {
    console.log("Form submission triggered!"); // Debug
    alert("Form submit is being handled by AJAX instead of the default form submission."); // Debug
    event.preventDefault();

    const form = $(this);
    const formData = new FormData(this);

    $.ajax({
        url: form.attr("action"), // Get the action URL from the form
        method: form.attr("method"), // Get the method (e.g., POST)
        data: formData, // Serialize the form data
        contentType: false, // Required for FormData
        processData: false, // Prevent automatic processing
        success: function (updatedCardHtml) {
            const productId = form.find("input[name='Id']").val(); // Get the product ID
            console.log(`Product with ID ${productId} updated successfully.`); // Debug log
            alert(`Product with ID ${productId} updated successfully.`); // Debug alert
            alert(`HTML to replace the product card:\n${updatedCardHtml}`); // Debug alert for HTML

            const productCard = $(`.card[data-id="${productId}"]`); // Find the product card
            productCard.replaceWith(updatedCardHtml); // Replace the card with the updated HTML

            $("#editProductModal").modal("hide"); // Close the modal
            $(".modal-backdrop").remove(); // Remove leftover backdrop
        },
        error: function (xhr) {
            console.error("Error:", xhr.responseText);
            alert("An error occurred while updating the product."); // Debug alert
        }
    });
});
