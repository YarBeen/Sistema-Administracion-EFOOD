let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar página _PAGE_ de _PAGES_",
            "infoEmpty": "No hay registros",
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
            "url": "/Admin/ProcesadorDePago/ObtenerTodos"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "procesador", "width": "25%" },
            { "data": "tipo", "width": "35%" },
            {
                "data": "estado",
                "render": function (data) {
                    return data ? "Activo" : "Inactivo";
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data, type, row) {
                    if (row.tipo === "Tarjeta de crédito o débito") {
                        return `
                            <div class="text-center">
                                <a href="/Admin/ProcesadorTarjeta/index/${data}" class="btn btn-primary text-white" style="cursor:pointer">
                                    <i class="bi bi-credit-card-fill"></i>
                                </a>
                            </div>
                        `;
                    } else {
                        return '';
                    }
                }, "width": "1%"
            },
            {
                "data": "id",
                "render": function (data, type, row) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/ProcesadorDePago/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i>  
                            </a>
                            <a onclick=Delete("/Admin/ProcesadorDePago/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash3-fill"></i>
                            </a> 
                        </div>
                    `;
                }, "width": "9%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "¿Estás seguro de eliminar el Procesador De Pago?",
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
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}

