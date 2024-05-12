let datatable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    var id = obtenerIdDesdeURL(); // Obtener la ID de la URL
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": `/Admin/ProductoPrecio/ObtenerTodos?id=${id}`
        },
        "columns": [
            { "data": "id", "width": "20%" },
            { "data": "tipoPrecio.nombre", "width": "40%" },
            { "data": "monto", "width": "40%" }
        ]
    })
}

function obtenerIdDesdeURL() {
    var urlParams = window.location.pathname.split('/');
    return urlParams[urlParams.length - 1];
}

