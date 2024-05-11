let datatable;


$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    
    var dropdown = document.getElementById("lineaComidaSelect");

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
            "url": `/Admin/Producto/ConsultarConFiltro`
        },
        "columns": [
            { "data": "id", "width": "40%" },
            { "data": "nombre", "width": "60%" }
        ]
    })
}

function refreshDataTable() {
    var dropdown = document.getElementById("lineaComidaSelect");

    datatable.clear().draw();

    datatable.ajax.url(`/Admin/Producto/ConsultarConFiltro?idLineaComida=${dropdown.value}`).load();
    
}

function clearDataTable() {

    datatable.clear().draw();

    datatable.ajax.url(`/Admin/Producto/ConsultarConFiltro`).load();

}