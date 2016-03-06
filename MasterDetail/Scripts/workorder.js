$(function() {
    $("a[data-modal=parts]").on("click", function () {
        $("#partsModalContent").load(this.href, function () {
            $("#partsModal").modal({ keyboard: true }, "show");

            $("#partchoice").submit(function () {
                if ($("#partchoice").valid()) {
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        data: $(this).serialize(),
                        success: function (result) {
                            if (result.success) {
                                $("#partsModal").modal("hide");
                                location.reload();
                            } else {
                                $("#MessageToClient").text("The data was not updated.");
                            }
                        },
                        error: function () {
                            $("#MessageToClient").text("The web server had an error.");
                        }
                    });
                    return false;
                }
            });

            $("#InventoryItemCode").autocomplete({
                minLength: 1,
                source: function(request, response) {
                    var url = $(this.element).data("url");
                    $.getJSON(url, { term: request.term }, function(data) {
                        response(data);
                    });
                },
                appendTo: $(".modal-body"),
                select: function(event, ui) {
                    $("#InventoryItemName").val(ui.item.InventoryItemName);
                    $("#UnitPrice").val(ui.item.UnitPrice);
                    recalculatePart();
                    $("#Quantity").select();
                },
                change: function(event, ui) {
                    if (!ui.item) {
                        $(event.target).val("");
                    }
                }
            });
        });
        return false;
    });

    $("a[data-modal=labors]").on("click", function () {
        $("#laborsModalContent").load(this.href, function () {
            $("#laborsModal").modal({ keyboard: true }, "show");

            $("#laborchoice").submit(function () {
                if ($("#laborchoice").valid()) {
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        data: $(this).serialize(),
                        success: function (result) {
                            if (result.success) {
                                $("#laborsModal").modal("hide");
                                location.reload();
                            } else {
                                $("#MessageToClient").text("The data was not updated.");
                            }
                        },
                        error: function () {
                            $("#MessageToClient").text("The web server had an error.");
                        }
                    });
                    return false;
                }
            });

            $("#ServiceItemCode").autocomplete({
                minLength: 1,
                source: function (request, response) {
                    var url = $(this.element).data("url");
                    $.getJSON(url, { term: request.term }, function (data) {
                        response(data);
                    });
                },
                appendTo: $(".modal-body"),
                select: function (event, ui) {
                    $("#ServiceItemName").val(ui.item.ServiceItemName);
                    $("#Rate").val(ui.item.Rate);
                    recalculateLabor();
                    $("#LaborHours").select();
                },
                change: function (event, ui) {
                    if (!ui.item) {
                        $(event.target).val("");
                    }
                }
            });
        });
        return false;
    });

    $("#partsModal").on("show.bs.modal", function () {
        recalculatePart();
    });

    $("#partsModal").on("shown.bs.modal", function () {
        $("#InventoryItemCode").focus();
    });

    $("#partsModal").on("hide.bs.modal", function () {
        location.reload();
    });

    $("#laborsModal").on("show.bs.modal", function () {
        recalculateLabor();
    });

    $("#laborsModal").on("shown.bs.modal", function () {
        $("#ServiceItemCode").focus();
    });

    $("#laborsModal").on("hide.bs.modal", function () {
        location.reload();
    });
});


function recalculatePart() {
    if (!$(".deleteform").exists()) {
        var quantity = parseInt(document.getElementById("Quantity").value).toFixed(0);
        var unitPrice = parseFloat(document.getElementById("UnitPrice").value).toFixed(2);

        if (isNaN(quantity)) {
            quantity = 0;
        }

        if (isNaN(unitPrice)) {
            unitPrice = 0;
        }

        document.getElementById("Quantity").value = quantity;
        document.getElementById("UnitPrice").value = unitPrice;

        document.getElementById("ExtendedPrice").value = numberWithCommas((quantity * unitPrice).toFixed(2));        
    }
}


function recalculateLabor() {
    if (!$(".deleteform").exists()) {
        var laborHours = parseFloat(document.getElementById("LaborHours").value).toFixed(2);
        var rate = parseFloat(document.getElementById("Rate").value).toFixed(2);

        if (isNaN(laborHours)) {
            laborHours = 0;
        }

        if (isNaN(rate)) {
            rate = 0;
        }

        document.getElementById("LaborHours").value = laborHours;
        document.getElementById("Rate").value = rate;

        document.getElementById("ExtendedPrice").value = numberWithCommas((laborHours * rate).toFixed(2));
    }
}


function numberWithCommas(n) {
    return n.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}


jQuery.fn.exists = function() {
    return this.length > 0;
}