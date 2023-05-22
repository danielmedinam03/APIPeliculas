var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblUsuarios').DataTable({

        "ajax": {
            "url": "/Usuarios/GetAllUsuarios",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "Id", "width": "20%" },
            { "data": "NombreUsuario", "width": "40%" },
            { "data": "Password", "width": "20%" }
        ]
    });
}
