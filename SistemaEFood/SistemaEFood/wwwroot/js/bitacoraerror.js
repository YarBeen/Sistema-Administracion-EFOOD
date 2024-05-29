let datatable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
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
            "url": "/Admin/BitacoraError/ObtenerPorFecha"
        },
        "columns": [
            { "data": "id", "width": "20%"},
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
            { "data": "mensaje", "width": "20%" },
        ]
    })
}


function Delete(url) {
    swal({
        title: "Esta seguro de Eliminar el producto?",
        text: "Este registro no se podrá recuper",
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
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })

}