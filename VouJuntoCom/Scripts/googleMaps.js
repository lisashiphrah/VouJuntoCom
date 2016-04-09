var map;
var directionsService;
var directionsRenderer;

$(document).ready(function () {
    loadMap();
});

//Inicializa mapas
function loadMap() {
    google.load('maps', '3.7', {
        'other_params':
        'sensor=false&language=pt-BR',
        'callback': loadMap_callback
    });
}
//Callback da inicialização
function loadMap_callback() {

    directionsService = new google.maps.DirectionsService();
    var centeredPlace = new google.maps.LatLng(-30.02770, -51.228740);

    map = new google.maps.Map(document.getElementById('map_canvas'), {
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        center: centeredPlace,
        zoom: 13,
        panControl: false,
        zoomControl: false,
        scaleControl: false,
        streetViewControl: false,
        overviewMapControl: false
    });
}
//Reset das configurações do mapa, caso o usuário opte por criar uma nova rota
function restartMap() {
    map = new google.maps.Map(document.getElementById('map_canvas'), {
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        center: new google.maps.LatLng(-30.02770, -51.228740),
        zoom: 13,
        panControl: false,
        zoomControl: false,
        scaleControl: false,
        streetViewControl: false,
        overviewMapControl: false
    });

    directionsRenderer = new google.maps.DirectionsRenderer({
        preserveViewport: false,
        draggable: true
    });

    $('#waysDirections').html('');
    $('#steps').val('');
    directionsRenderer.setMap(null);
    directionsRenderer.setMap(map);
}

//Evento para criação de nova rota.
function createRoute() {

    restartMap();

    var directionsRequest = {
        origin: document.getElementById('inputFrom').value,
        destination: document.getElementById('inputTo').value,
        travelMode: google.maps.TravelMode.DRIVING,
        unitSystem: google.maps.UnitSystem.METRIC
    };

    directionsRenderer.setPanel(document.getElementById('waysDirections'));
    directionsService.route(directionsRequest, function (result, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            directionsRenderer.setDirections(result);
        }
    });

    google.maps.event.addListener(directionsRenderer, 'directions_changed', function () {
        computeTotalDistanceSteps(directionsRenderer.directions);
        setRidePrice(directionsRenderer.directions);
    });
}

//Define todo o caminho percorrido pelo usuário e adiciona no input 'steps' para armazenamento
function computeTotalDistanceSteps(result) {
    var myroute = result.routes[0];
    document.getElementById('steps').value = "";
    for (i = 0; i < myroute.legs.length; i++) { //caminha por cada perna
        var directionsLeg = myroute.legs[i];
        for (u = 0; u < directionsLeg.steps.length; u++) { //caminha por cada passo da perna
            document.getElementById('steps').value = document.getElementById('steps').value + directionsLeg.steps[u].path + "\n\n";
        }
    }
}
//Calcula o preço da corrida.
function setRidePrice(result) {
    var totalDistance = computeTotalDistance(result);
    var selectedRadio = getSelectedRadio();
    if (selectedRadio == 0) { //Intramunicipais -> R$ 1,00 a cada 4km
        labelPrice = totalDistance / 4;
    }
    else { //Intermunicipais -> R$ 1,00 a cada 6km
        labelPrice = totalDistance / 6;
    }
    displayDivPrice(labelPrice);
    $('#Distance').val(totalDistance);
}
//Calcula distância total percorrida.
function computeTotalDistance(result) {

    var total = 0;
    var myroute = result.routes[0];
    for (i = 0; i < myroute.legs.length; i++) {
        total += myroute.legs[i].distance.value;
    }
    total = total / 1000.
    return total;
}
//Verifica qual radio foi selecionado: Intramunicipais ou Intermunicipais
function getSelectedRadio() {
    var radios = document.getElementsByTagName('input');
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].type === 'radio' && radios[i].checked) {
            return radios[i].value;
        }
    }
}
//Imprime valor da corrida na div de waysDirections
function displayDivPrice(price) {
    var priceString = 'R$ ' + price.toFixed(2);
    priceString = priceString.replace('.', ',');
    $('#priceHidden').val(price.toFixed(2).replace('.', ','));
    if ($('#waysDirections').html() == '') { //se for primeira vez
        $('#waysDirections').prepend('<div class="Price">' +
                        '<label>Valor da carona por pessoa:</label><br />' +
                        '<label id="labelPrice">' + priceString + '</label></div>');
    }
    else {
        $('.Price').html('<label>Valor da carona por pessoa:</label><br />' +
                       '<label id="labelPrice">' + priceString + '</label>');
    }
}