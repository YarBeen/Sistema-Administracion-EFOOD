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
            "url": "/Admin/Pedido/ConsultarConFiltro",
            "data": function (d) {
                d.fechainicial = $('#fechaInicio').val();
                d.fechafinal = $('#fechaFin').val();
                d.estado = $('#estado').val();
            }
        },
        "columns": [
            { "data": "id", "width": "20%" },
            {
                "data": "fechaOrden", "width": "40%",
                "render": function (data) {
                    var fecha = new Date(data);
                    var opciones = { year: 'numeric', month: 'short', day: 'numeric' };
                    return fecha.toLocaleDateString('es-ES', opciones);
                }
            },
            { "data": "estado", "width": "20%" },
            {
                "data": "monto", "width": "20%",
                "render": function (data) {
                    return '₡' + parseFloat(data).toFixed(2);
                }
            }
        ]
    });
}


function filterDataTable() {
    datatable.ajax.reload();
}

function clearDataTable() {
    $('#fechaInicio').val('');
    $('#fechaFin').val('');
    $('#estado').val('');
    datatable.ajax.reload();
}