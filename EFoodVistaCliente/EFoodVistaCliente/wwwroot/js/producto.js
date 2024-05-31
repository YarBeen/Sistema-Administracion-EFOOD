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
            "url": "https://localhost:7138/Inventario/Producto/ObtenerTodos"
        },
        "columns": [
            { "data": "id", "width": "20%" },
            { "data": "nombre", "width": "40%" },
            {
                "data": "imagenUrl",
                "render": function (data) {
                    return `
                        <div class="text-center">
                           
                            <img src="${data}" alt="Product Image" style="max-width: 100px; max-height: 100px;">
                           
                        </div>
                    `;
                }, "width": "40%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                           
                            <a href="/Admin/ProductoPrecio/Index/${data}" class="btn btn-primary text-white" style="cursor:pointer">
                                <i class="bi bi-tags"></i> Precios
                            </a>
                           
                        </div>
                    `;
                }, "width": "40%"
            }
        ]
    })
}

