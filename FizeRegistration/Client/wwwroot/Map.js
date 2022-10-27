
/*var latLngMarkerValue = { latLng: null }*/
let markers = [];

var directionsService;
var directionsRenderer;
function initMap() {
    const myLatlng = { lat: -25.363, lng: 131.044 };
    directionsService = new google.maps.DirectionsService();
    directionsRenderer = new google.maps.DirectionsRenderer();
    var map = new google.maps.Map(document.getElementById("map"), {
        zoom: 6,
        center: myLatlng,
    });
    map.addListener("click", (e) => {
        placeMarker(e.latLng)
    });
    directionsRenderer.setMap(map);

    function placeMarker(latLng) {
        let marker = new google.maps.Marker({
            position: latLng,
            map: map
        });

        markers.push(marker)
    //debugger


        google.maps.event.addListener(marker, "click", function () {
          
            deleteMarkers(marker)
        });
        
    }
   
}

function calcRoute() {
   
    let start = markers[0].position;
    let end = markers[1].position;
    let request = {
        origin: start,
        destination: end,
        travelMode: 'DRIVING',
        unitSystem: google.maps.UnitSystem.METRIC
    };
    //debugger
    directionsService.route(request, function (result, status) {
        if (status == 'OK') {
            directionsRenderer.setDirections(result);

            var route = response.routes[0];
            //  alert(route.legs[1].duration.text);
            var summaryPanel = document.getElementById('directions_panel');
           summaryPanel.innerHTML = '';
            // For each route, display summary information.
            for (var i = 0; i < route.legs.length; i++) {
                var routeSegment = i + 1;
                //summaryPanel.innerHTML += '<b>Route Segment: ' + routeSegment + '</b><br>';
                //summaryPanel.innerHTML += route.legs[i].start_address + ' to ';
                //summaryPanel.innerHTML += route.legs[i].end_address + '<br>';
                summaryPanel.innerHTML += route.legs[i].duration.text + '<br>';
                summaryPanel.innerHTML += route.legs[i].distance.text + '<br><br>';
            }
           
        }
   });
    //directionsResult.routes[0].legs;
}



function deleteMarkers(marker) {
    //debugger
    var index = markers.indexOf(marker);
    markers[index].setMap(null);
    let NewMarkers = markers.splice(index, 1);
    markers = NewMarkers;

   /* System.arraycopy(array, index + 1, array, index, array.length - index - 1);*/
}
window.initMap = initMap;
