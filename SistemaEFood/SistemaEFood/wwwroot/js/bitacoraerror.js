let datatable;

$(document).ready(function () {
    $('#btnBuscar').on('click', function () {
        var fechaInicio = $('#fechaInicio').val();
        var fechaFin = $('#fechaFin').val();

        if (fechaInicio && fechaFin) {
            loadDataTable(fechaInicio, fechaFin);
        } else {
            alert('Por favor, ingrese ambas fechas.');
        }
    });

    loadDataTable();
});

function loadDataTable(fechaInicio = null, fechaFin = null) {
    if (datatable) {
        datatable.destroy();
    }

    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
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
            "url": "/Admin/BitacoraError/ConsultarConFiltro",
            "data": {
                "fechainicial": fechaInicio,
                "fechafinal": fechaFin
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
            { "data": "numeroError", "width": "20%" },
            { "data": "mensaje", "width": "20%" }
        ]
    });
}
function clearDataTable() {
    $('#fechaInicio').val('');
    $('#fechaFin').val('');
    loadDataTable(); 
};
function Delete(url) {
    swal({
        title: "¿Está seguro de eliminar el registro?",
        text: "Este registro no se podrá recuperar",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}
