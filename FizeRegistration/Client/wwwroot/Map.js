
/*var latLngMarkerValue = { latLng: null }*/
var markers = [];
function initMap() {
    const myLatlng = { lat: -25.363, lng: 131.044 };

    var map = new google.maps.Map(document.getElementById("map"), {
        zoom: 4,
        center: myLatlng,
    });
    map.addListener("click", (e) => {
        placeMarker(e.latLng)
    });
   
    function placeMarker(latLng) {
        var marker = new google.maps.Marker({
            position: latLng,
            map: map
        });

        markers.push(marker)
        //setMapOnAll(map)

        google.maps.event.addListener(marker, "dblclick", function () {
            //info_Window.setContent(html);
            //info_Window.open(map, marker);      
            debugger
            deleteMarkers(marker)
        });
        
    }
    function setMapOnAll(map) {
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(map);
        }
    }
}

function deleteMarkers(marker) {
   
    var index = markers.indexOf(marker);
    markers[index].setMap(null);

}
window.initMap = initMap;
