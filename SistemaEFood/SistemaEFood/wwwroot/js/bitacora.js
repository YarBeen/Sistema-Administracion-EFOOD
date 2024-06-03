let datatable;

$(document).ready(function () {
    loadDataTable();

    $('#btnBuscar').on('click', function () {
        filterDataTable();
    });

    $('#btnLimpiarFiltro').on('click', function () {
        clearDataTable();
    });
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Página",
            "zeroRecords": "Ningún Registro",
            "info": "Mostrar página _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtrado de _MAX_ registros totales)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/Bitacora/ConsultarConFiltro",
            "data": function (d) {
                d.fechainicial = $('#fechaInicio').val();
                d.fechafinal = $('#fechaFin').val();
            }
        },
        "columns": [
            { "data": "id", "width": "20%" },
            {
                "data": "fecha", "width": "40%",
                "render": function (data) {
                    var fecha = new Date(data);
                    var opciones = { year: 'numeric', month: 'short', day: 'numeric' };
                    return fecha.toLocaleDateString('es-ES', opciones);
                }
            },
            { "data": "hora", "width": "20%" },
            { "data": "nombreUsuario", "width": "20%" },
            { "data": "mensaje", "width": "20%" },
        ]
    });
}

function filterDataTable() {
    datatable.ajax.reload();
}

function clearDataTable() {
    $('#fechaInicio').val('');
    $('#fechaFin').val('');
    datatable.ajax.reload();
}

function Delete(url) {
    // Función Delete
}

