let datatable;

$(document).ready(function () {
    loadDataTable();

    $('#btnBuscar').on('click', function () {
        filterDataTable();
    });

    $('#btnLimpiarFiltro').on('click', function () {
        clearDataTable();
    });

    $('#btnEliminar').on('click', function () {
        eliminarPedidos();
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
            },
            {
                "data": null,
                "width": "10%",
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    // Agregar checkbox solo si el estado es "En Curso"
                    if (row.estado === "En Curso") {
                        return '<input type="checkbox" class="form-check-input" name="chkPedido" value="' + row.id + '">';
                    } else {
                        return ''; // Limpiar la columna si no es "En Curso"
                    }
                }
            }
        ],
        "createdRow": function (row, data, dataIndex) {
            // Agregar checkbox solo si el estado es "En Curso"
            if (data.estado === "En Curso") {
                $('td:eq(4)', row).html('<input type="checkbox" class="form-check-input  name="chkPedido" value="' + data.id + '">');
            } else {
                $('td:eq(4)', row).html(''); // Limpiar la columna si no es "En Curso"
            }
        }
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


function eliminarPedidos() {
    var idPedidos = [];
    $('#tblDatos input[type="checkbox"]:checked').each(function () {
        idPedidos.push(parseInt($(this).val()));
    });
    console.log(idPedidos);
    if (idPedidos.length === 0) {
        swal("¡Error!", "Por favor, seleccione al menos un pedido para eliminar.", "error");
        return;
    }

    swal({
        title: "¿Está seguro de que desea eliminar los pedidos seleccionados?",
        text: "Esta acción no se puede deshacer.",
        icon: "warning",
        buttons: ["Cancelar", "Eliminar"],
        dangerMode: true,
    }).then((eliminar) => {
        if (eliminar) {
            $.ajax({
                url: '/Admin/Pedido/EliminarPedidos',
                type: 'POST',
                dataType: 'json',
                data: { ids: idPedidos },
                success: function (response) {
                    if (response.success) {
                        swal("¡Éxito!", "Los pedidos se eliminaron correctamente.", "success");
                        datatable.ajax.reload();
                    } else {
                        swal("¡Error!", "Ocurrió un error al eliminar los pedidos: " + response.message, "error");
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    swal("¡Error!", "Ocurrió un error al eliminar los pedidos: " + errorThrown, "error");
                }
            });
        }
    });
}
