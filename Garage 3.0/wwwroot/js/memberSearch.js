$(document).ready(function () {
    // Initialize DataTable with minimal features
    var table = $('#membersTable').DataTable({
        "dom": 'rt<"bottom"p>', // Only show table and pagination
        "ordering": true,
        "pageLength": 10,
        "searching": true, // Enable searching but we'll use our custom input
        "language": {
            "paginate": {
                "first": "First",
                "last": "Last",
                "next": "Next",
                "previous": "Previous"
            }
        }
    });

    // Custom search functionality
    $('#memberSearch').on('keyup', function () {
        table.search(this.value).draw();
    });

    // Vehicle count filter
    $('#vehicleFilter').on('change', function () {
        var filterValue = $(this).val();

        $.fn.dataTable.ext.search.pop(); // Remove previous filter

        if (filterValue !== '') {
            $.fn.dataTable.ext.search.push(function (settings, data) {
                var vehicleCount = parseInt(data[1]); // Column index for vehicle count

                switch (filterValue) {
                    case "0":
                        return vehicleCount === 0;
                    case "1-2":
                        return vehicleCount >= 1 && vehicleCount <= 2;
                    case "3+":
                        return vehicleCount >= 3;
                    default:
                        return true;
                }
            });
        }

        table.draw();
    });
});