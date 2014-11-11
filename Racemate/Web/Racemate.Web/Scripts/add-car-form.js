$(function () {
    $("#car-makes-select").change(function () {
        var $carModelSelect = $("#car-model-select");

        $.ajax({
            url: "/User/Garage/MakeModels",
            method: "GET",
            data: { makeId: $(this).val() },
            success: function (models) {
                $carModelSelect.html("");

                models.forEach(function (item) {
                    var option = $("<option>").val(item.Id).text(item.Name);

                    $carModelSelect.append(option);
                });
            },
            error: function () {
                console.log("An error occurred while processing data.");
            }
        });
    });
});