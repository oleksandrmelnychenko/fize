﻿//"use strict";
//Object.defineProperty(exports, "__esModule", { value: true });
//function initMap() {
//    var directionsService = new google.maps.DirectionsService();
//    var directionsRenderer = new google.maps.DirectionsRenderer();
//    var map = new google.maps.Map(document.getElementById("map"), {
//        zoom: 7,
//        center: { lat: 41.85, lng: -87.65 },
//    });
//    directionsRenderer.setMap(map);
//    var onChangeHandler = function () {
//        calculateAndDisplayRoute(directionsService, directionsRenderer);
//    };
//    document.getElementById("start").addEventListener("change", onChangeHandler);
//    document.getElementById("end").addEventListener("change", onChangeHandler);
//}
//function calculateAndDisplayRoute(directionsService, directionsRenderer) {
//    directionsService
//        .route({
//            origin: {
//                query: document.getElementById("start").value,
//            },
//            destination: {
//                query: document.getElementById("end").value,
//            },
//            travelMode: google.maps.TravelMode.DRIVING,
//        })
//        .then(function (response) {
//            directionsRenderer.setDirections(response);
//        })
//        .catch(function (e) { return window.alert("Directions request failed due to " + status); });
//}
//window.initMap = initMap;
