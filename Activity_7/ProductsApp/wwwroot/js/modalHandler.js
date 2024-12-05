console.log("modalHandler.js loaded and running");

// Event handler for the Edit button
$(document).on("click", ".edit-button", function () {
    const productId = $(this).data("id");
    console.log(`Edit button clicked for product ID: ${productId}`); // Debug
    alert(`Edit button clicked for product ID: ${productId}`); // Debug

    // Load the modal content via AJAX
    $.ajax({
        url: `/Home/ShowUpdateModal?id=${productId}`,
        success: function (result) {
            alert(`Modal HTML Loaded:\n${result}`); // Debug alert
            $("#editProductModal .modal-content").html(result);
            const modal = new bootstrap.Modal(document.getElementById("editProductModal"));
            modal.show();
        },
        error: function (xhr, status, error) {
            console.error("Error loading modal:", error);
            alert("An error occurred while loading the modal."); // Debug alert
        }
    });
});

// Clean up modal and backdrop after hiding
$("#editProductModal").on("hidden.bs.modal", function () {
    $(".modal-backdrop").remove(); // Remove leftover backdrop
    $(this).find(".modal-content").html(""); // Clear modal content
});
