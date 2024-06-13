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
            "url": `/Admin/ProcesadorTarjeta/ObtenerTodos?id=${id}` 
        },
        "columns": [
            { "data": "id", "width": "20%"},
            { "data": "tarjeta.nombre", "width": "40%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a onclick=Delete("/Admin/ProcesadorTarjeta/Delete/${data}") class="btn btn-danger text-color-white background-color-red btn-outline-red" style="cursor:pointer">
                                <i class="bi bi-trash3-fill"></i>
                            </a>
                        </div>
                    `;
                }, "width":"40%"
            }
        ]
    })
}



function obtenerIdDesdeURL() {
    var urlParams = window.location.pathname.split('/'); 
    return urlParams[urlParams.length - 1]; 
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