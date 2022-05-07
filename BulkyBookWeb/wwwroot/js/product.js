var dataTable;
$(document).ready(function () {
    loadDataTable();
});

//Keep in mind that JSON is case sensitive
function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url":"/Admin/Products/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category", "width": "15%" }

        ]
    });
}