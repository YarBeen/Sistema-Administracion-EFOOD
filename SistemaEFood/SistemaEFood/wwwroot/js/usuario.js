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
            "url": "/Admin/Usuario/ObtenerTodos"
        },
        "columns": [
            { "data": "email", "width": "30%" },
            { "data": "userName", "width": "20%" },
        
            { "data": "role", "width": "30%" },
            {
                "data": {id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    let hoy = new Date().getTime();
                    let bloqueo = new Date(data.lockoutEnd).getTime();
                   

                    if (bloqueo > hoy) {
                        //el usuario esta bloqueo
                        return `<div class="text-center">
                           
                            <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-danger text-white" style="cursor:pointer">
                            <i class="bi bi-unlock-fill"></i> Activar
                        </a>
                        </div >

                           ` ;
                    }
                    else {
                        return `<div class="text-center">
                           
                            <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-success text-white" style="cursor:pointer">
                            <i class="bi bi-lock-fill"></i> Desactivar
                        </a>
                        </div >

                           `;
                    }
                }
            }
           
          /*  {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href= "/Admin/Usuario/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer", width:150px>
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            <a onclick=Delete("/Admin/Usuario/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer", width:150px>
                                <i class="bi bi-trash3-fill"></i>
                            </a>
                        </div>
                    `;
                }, "width":"20%"
            }*/
        ]
    })
}


function BloquearDesbloquear(id) {
   /* swal({
        title: "Esta seguro de bloquear el usuario?",
        text: "Este registro no se podrá recuper",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar)
        {*/
            $.ajax({
                type: "POST",
                url: '/Admin/Usuario/BloquearDesbloquear',
                data: JSON.stringify(id),
                contentType: "application/json",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        //}
  

}