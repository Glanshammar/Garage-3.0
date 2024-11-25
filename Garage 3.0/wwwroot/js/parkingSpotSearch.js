function initializeParkingSpotSearch() {
    function updateSpotDetails(show, message, isError = false) {
        const spotDetails = $('#spotDetails');

        if (show) {
            spotDetails
                .removeClass('d-none alert-info alert-warning')
                .addClass(isError ? 'alert-warning' : 'alert-info')
                .find('#spotInfo')
                .html(message);
        } else {
            spotDetails.addClass('d-none');
        }
    }

    function updateParkingSpotSelect(spots) {
        const spotSelect = $('#ParkingSpotId');
        spotSelect.empty();
        spotSelect.append('<option value="">Select Parking Spot</option>');

        if (!spots || spots.length === 0) {
            updateSpotDetails(true, 'No available spots found matching your criteria.', true);
            return;
        }

        spots.forEach(spot => {
            spotSelect.append(`<option value="${spot.id}" 
                data-size="${spot.size}" 
                data-location="${spot.location}"
                data-cost="${spot.parkingCost}">
                Spot ${spot.spotNumber} - ${spot.size} - ${spot.location}
            </option>`);
        });

        updateSpotDetails(false);
    }

    // Search button click handler
    $('#searchSpots').click(function () {
        const size = $('#sizeFilter').val();
        const location = $('#locationFilter').val();

        $.get('/ParkedVehicles/SearchAvailableSpots', { size, location })
            .done(function (data) {
                updateParkingSpotSelect(data.spots);
            })
            .fail(function () {
                updateSpotDetails(true, 'An error occurred while searching for parking spots.', true);
            });
    });

    // Parking spot selection change handler
    $('#ParkingSpotId').change(function () {
        const selected = $(this).find(':selected');
        if (selected.val()) {
            const details = `
                Size: ${selected.data('size')}<br>
                Location: ${selected.data('location')}<br>
                Cost per hour: ${selected.data('cost')} kr
            `;
            updateSpotDetails(true, details);
        } else {
            updateSpotDetails(false);
        }
    });

    // Reset spot details when filters change
    $('#sizeFilter, #locationFilter').change(function () {
        updateSpotDetails(false);
    });
}