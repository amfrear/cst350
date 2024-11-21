function initializeForm(taxRate) {
    $(document).ready(function () {
        function updateComputedFields() {
            var price = parseFloat($("#Price").val());
            if (!isNaN(price)) {
                var tax = price * taxRate;
                var formattedTax = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(tax);

                $("#FormattedEstimatedTax").val(formattedTax);
                $("#EstimatedTax").val(tax);
            }
        }

        // Bind the update logic to the Price input field
        $("#Price").on("input change", updateComputedFields);

        // Initialize the computed fields
        updateComputedFields();
    });
}
