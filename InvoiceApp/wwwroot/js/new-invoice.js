$(document).ready(function () {
    enableSuggestions();
    changeTotals();
    $("#add-item").on("click", addNewInvoiceItem);
    $("#invoice-items-list").on("click", ".js-delete", deleteInvoiceItem);
    $("#invoice-items-list").on("change keyup cut paste", ".js-change", saveValue);
    $("form").on("change keyup cut paste", ".js-calculate", changeTotals);
    $("#Company_Name").on("change keyup cut paste", removeData);
});

var enableSuggestions = function () {
    var companies = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/api/company?query=%QUERY',
            wildcard: '%QUERY'
        }
    });

    $("#Company_Name").typeahead({
        minLength: 1,
        highlight: true,
        hint: false,
        autoSelect: false
    },
    {
        name: 'companies',
        display: 'name',
        source: companies,
        limit: Infinity
    }).on("typeahead:select typeahead:autocomplete", function (event, company) {
        $("#Company_Address").val(company.address).prop("readonly", true);
        $("#Company_PostalCode").val(company.postalCode).prop("readonly", true);
        $("#Company_City").val(company.city).prop("readonly", true);
        $("#Company_Country").val(company.country).prop("readonly", true);
        $("#Company_VATNumber").val(company.vatNumber).prop("readonly", true);
        $("#Company_Phone").val(company.phone).prop("readonly", true);
        $("#Company_Email").val(company.email).prop("readonly", true);
        $(this).attr("savedValue", $(this).val());
        
    });
}

var removeData = function () {
    if ($(this).attr("savedValue") != $(this).val()) {
        $("#Company_Address").val(null).prop("readonly", false);
        $("#Company_PostalCode").val(null).prop("readonly", false);
        $("#Company_City").val(null).prop("readonly", false);
        $("#Company_Country").val(null).prop("readonly", false);
        $("#Company_VATNumber").val(null).prop("readonly", false);
        $("#Company_Phone").val(null).prop("readonly", false);
        $("#Company_Email").val(null).prop("readonly", false);
    }
}

var addNewInvoiceItem = function () {
    var container = $("#invoice-items-list");
    var index = container.children("li").length;
    var item = `<li class="list-group-item">
                            <div class="row align-items-end">
                                <div class="form-group col-md-6">
                                    <span class="text-danger field-validation-valid" data-valmsg-for="InvoiceItems[#].Description" data-valmsg-replace="true"></span><br />
                                    <label for="InvoiceItems_#__Description">Description</label>
                                    <textarea class="form-control js-change" data-val="true" data-val-length="The field Description must be a string with a minimum length of 3 and a maximum length of 5000." data-val-length-max="5000" data-val-length-min="3" data-val-required="The Description field is required." id="InvoiceItems_#__Description" maxlength="5000" name="InvoiceItems[#].Description">
</textarea>
                                </div>
                                <div class="form-group col-md-2">
                                    <span class="text-danger field-validation-valid" data-valmsg-for="InvoiceItems[#].UnitPriceWithoutTax" data-valmsg-replace="true"></span><br />
                                    <label for="InvoiceItems_#__UnitPriceWithoutTax">Unit price without tax</label>
                                    <input class="form-control js-change js-calculate" type="text" data-val="true" data-val-number="The field Unit price without tax must be a number." data-val-range="Value for Unit price without tax must be greater than 0,01." data-val-range-max="1.7976931348623157E&#x2B;308" data-val-range-min="0.01" data-val-required="The Unit price without tax field is required." id="InvoiceItems_#__UnitPriceWithoutTax" name="InvoiceItems[#].UnitPriceWithoutTax" value="" />
                                </div>
                                <div class="form-group col-md-2">
                                    <span class="text-danger field-validation-valid" data-valmsg-for="InvoiceItems[#].Quantity" data-valmsg-replace="true"></span><br />
                                    <label for="InvoiceItems_#__Quantity">Quantity</label>
                                    <input class="form-control js-change js-calculate" type="number" data-val="true" data-val-range="Value for Quantity must be greater than 1." data-val-range-max="2147483647" data-val-range-min="1" data-val-required="The Quantity field is required." id="InvoiceItems_#__Quantity" name="InvoiceItems[#].Quantity" value="" />
                                </div>
                                <div class="form-group col-md-2 text-center">
                                    <button type="button" class="btn btn-danger js-delete">Delete Item</button>
                                </div>
                            </div>
                        </li>`
    item = item.replace(/\[#\]/g, '[' + index + ']')
        .replace(/_#__/g, '_' + index + '__');
    container.append(item);
    reenableFormValidation();
}

var reenableFormValidation = function () {
    $("form").data("validator", null);
    $.validator.unobtrusive.parse($("form"));
}

var deleteInvoiceItem = function () {
    var button = $(this);
    bootbox.confirm("Are you sure you want to delete this invoice item?", function (result) {
        if (result) {
            button.parents("li").remove();
            reindexInvoiceItems();
            changeTotals();
        }
    })
}

var reindexInvoiceItems = function () {
    $("#invoice-items-list").children("li").each(function (index) {
        $(this).html($(this).html().replace(/\[\d\]/g, '[' + index + ']')
            .replace(/_\d__/g, '_' + index + '__'));
        changeValueFromSavedValue();
    })
}

var changeValueFromSavedValue = function () {
    $(".js-change").each(function () {
        if ($(this).attr("savedValue") != undefined) {
            $(this).val($(this).attr("savedValue"));
        }
    })
}

var saveValue = function () {
    $(this).attr("savedValue", $(this).val());
}

var changeTotals = function () {
    var totalWithoutTax = calcTotalWithoutTax();
    $("#total-without-tax").text(totalWithoutTax.toFixed(2));
    var tax = calculateTax(totalWithoutTax);
    $("#tax").text(tax.toFixed(2));
    var total = calculateTotal(totalWithoutTax, tax);
    $("#total").text(total.toFixed(2));
}

var calcTotalWithoutTax = function () {
    var totalWithoutTax = 0
    $("#invoice-items-list").children("li").each(function (index) {
        var unitPriceId = "#InvoiceItems_" + index + "__UnitPriceWithoutTax";
        var quantiyId = "#InvoiceItems_" + index + "__Quantity";
        var unitPrice = $(unitPriceId).val();
        var quantity = $(quantiyId).val();
        var subTotal = unitPrice * quantity;
        totalWithoutTax = totalWithoutTax + subTotal;
    })
    return totalWithoutTax;
}

var calculateTax = function (totalWithoutTax) {
    var vat = $("#VATPercentage").val();
    var tax = totalWithoutTax * (vat / 100);
    return tax;
}

var calculateTotal = function (totalWithoutTax, tax) {
    return totalWithoutTax + tax;
}
