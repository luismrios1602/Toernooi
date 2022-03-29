function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    window.history.pushState(null, "", window.location.pathname);//Elimino los parametros
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
