$(document).ready(fillUserTable());
function fillUserTable() {
    $('#Users').dataTable({
        "responsive": true,
        "processing": true,
        "serverSide": true,
        "filters": true,
        "bDestroy": true,
        "ajax": {
            "url": "/api/users",
            "type": "post",
            "datatype": "json"

        },
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false,
            }
        ],
        "columns": [
            { "data": "id", "name": "Id", "autowidth": true },
            { "data": "email", "name": "Email", "autowidth": true },
            { "data": "userName", "name": "UserName", "autowidth": true },
            { "data": "city", "name": "City", "autowidth": true },
            {
                "render": function (data, type, row) {
                    return `<button class="btn btn-danger" onclick=deleteUser('${row.id}')>
                                <i class="fa fa-trash"></i>
                                <div class="spinner-border text-white spinner-border-sm d-none" role="status" id="${row.id}">
                                    <span class="visually-hidden">Loading...</span>
                                </div>
                            </button>`;
                },
                "orderable": false
            }
        ]
    });
}
